using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace RealisticEarthquake.Common.Systems
{
    // Бонус по запросу: во время землетрясения мелкие интерактивные объекты вокруг игрока
    // могут случайно "среагировать", как будто по ним ударил игрок - горшки бьются, факелы падают,
    // паутина рвётся, перекати-кактусы срываются с места и катятся, ловушки срабатывают.
    // Работает только у авторитетной стороны (сервер/синглплеер), обычные изменения тайлов синхронизируются как обычно.
    public static class EnvironmentDisruptionSystem
    {
        // Сколько случайных точек вокруг игрока проверяем за один тик.
        private const int ProbesPerPlayerPerTick = 3;
        private const int SearchRadiusTiles = 24;

        // Шанс срабатывания для каждой категории объектов (в долях, за одну "удачную" проверку тайла).
        private const float PotBreakChance = 0.12f;
        private const float TorchBreakChance = 0.06f;
        private const float CobwebBreakChance = 0.18f;
        private const float TumbleweedChance = 0.22f;
        private const float TrapChance = 0.04f;

        // "Tumbleweed" в разных версиях tModLoader может отсутствовать как именованная константа TileID,
        // поэтому ищем его ID по внутреннему имени тайла во время выполнения - так код не сломает сборку,
        // даже если этого тайла вообще нет в текущей версии (просто перекати-кактус не будет обрабатываться).
        private static readonly int? TumbleweedTileType = ResolveTileIdByName("Tumbleweed");

        private static int? ResolveTileIdByName(string internalName)
        {
            if (TileID.Search.TryGetId(internalName, out int id))
                return id;
            return null;
        }

        public static void Tick(Player player)
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                return; // тайлы меняет только сервер/синглплеер

            int centerX = (int)(player.Center.X / 16f);
            int centerY = (int)(player.Center.Y / 16f);

            for (int i = 0; i < ProbesPerPlayerPerTick; i++)
            {
                int x = centerX + Main.rand.Next(-SearchRadiusTiles, SearchRadiusTiles + 1);
                int y = centerY + Main.rand.Next(-SearchRadiusTiles, SearchRadiusTiles + 1);

                if (!WorldGen.InWorld(x, y, 5))
                    continue;

                Tile tile = Main.tile[x, y];
                if (tile == null || !tile.HasTile)
                    continue;

                int type = tile.TileType;

                if (type == TileID.Pots)
                {
                    if (Main.rand.NextFloat() < PotBreakChance)
                        BreakTile(x, y);
                }
                else if (type == TileID.Torches)
                {
                    if (Main.rand.NextFloat() < TorchBreakChance)
                        BreakTile(x, y);
                }
                else if (type == TileID.Cobweb)
                {
                    if (Main.rand.NextFloat() < CobwebBreakChance)
                        BreakTile(x, y, noItem: true);
                }
                else if (TumbleweedTileType.HasValue && type == TumbleweedTileType.Value)
                {
                    if (Main.rand.NextFloat() < TumbleweedChance)
                        BreakTile(x, y); // при разрушении выпадает предмет "Перекати-кактус", который катится по земле как физический объект
                }
                else if (type == TileID.Traps || type == TileID.Boulder)
                {
                    // TileID.Traps - общий ID для дротиковых/копейных/шипастых/огненных ловушек (различаются только стилем тайла).
                    // TileID.Boulder - ловушка-валун.
                    if (Main.rand.NextFloat() < TrapChance)
                        TriggerTrap(x, y);
                }
            }
        }

        private static void BreakTile(int x, int y, bool noItem = false)
        {
            WorldGen.KillTile(x, y, fail: false, effectOnly: false, noItem: noItem);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendTileSquare(-1, x, y, 3);
        }

        private static void TriggerTrap(int x, int y)
        {
            // Тот же механизм, которым обычно "щёлкает" нажимная плита - запускает провод/ловушку в этой точке.
            Wiring.HitSwitch(x, y);

            if (Main.netMode == NetmodeID.Server)
                NetMessage.SendData(MessageID.HitSwitch, -1, -1, null, x, y);
        }
    }
}
