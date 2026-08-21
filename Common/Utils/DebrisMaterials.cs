using System.Collections.Generic;
using Terraria.ID;

namespace RealisticEarthquake.Common.Utils
{
    // Описание одного "осыпающегося" материала.
    public struct DebrisMaterial
    {
        public string Name;
        public int TileType;         // ID настоящего тайла в мире (для проверки потолка)
        public int TextureItemID;    // ID предмета, чью иконку используем как текстуру обломка (чтобы не рисовать свою)
        public float Hardness;       // множитель урона/прочности. Чем больше - тем твёрже порода
        public int DustType;         // тип пыли для эффекта разрушения

        public DebrisMaterial(string name, int tileType, int textureItemId, float hardness, int dustType)
        {
            Name = name;
            TileType = tileType;
            TextureItemID = textureItemId;
            Hardness = hardness;
            DustType = dustType;
        }
    }

    public static class DebrisMaterials
    {
        // Только "рыхлые" породы могут осыпаться. Камень, руды и всё, что твёрже камня - надёжное укрытие.
        // Твёрдость расставлена согласно ТЗ: снег - мягче всего, земля/грязь/глина - средне,
        // песчаник/затвердевший песок/лёд - самые твёрдые из "рыхлых" материалов.
        public static readonly Dictionary<int, DebrisMaterial> SoftTiles = new()
        {
            // Мягкие породы
            [TileID.SnowBlock] = new DebrisMaterial("Снег", TileID.SnowBlock, ItemID.SnowBlock, 0.5f, DustID.Snow),
            [TileID.Slush] = new DebrisMaterial("Слякоть", TileID.Slush, ItemID.SlushBlock, 0.55f, DustID.Snow),

            // Средние породы
            [TileID.Dirt] = new DebrisMaterial("Земля", TileID.Dirt, ItemID.DirtBlock, 1.0f, DustID.Dirt),
            [TileID.Mud] = new DebrisMaterial("Грязь", TileID.Mud, ItemID.MudBlock, 1.0f, DustID.Mud),
            [TileID.Silt] = new DebrisMaterial("Ил", TileID.Silt, ItemID.SiltBlock, 1.05f, DustID.Dirt),
            [TileID.Ash] = new DebrisMaterial("Пепел", TileID.Ash, ItemID.AshBlock, 1.1f, DustID.Ash),
            [TileID.ClayBlock] = new DebrisMaterial("Глина", TileID.ClayBlock, ItemID.ClayBlock, 1.25f, DustID.Clentaminator_Blue),

            // === ДОБАВЛЕНО: Блоки с растительностью (трава, мох, грибы) ===
            // Трава на земле (поверхность)
            [TileID.Grass] = new DebrisMaterial("Трава", TileID.Grass, ItemID.DirtBlock, 1.0f, DustID.Grass),
            // Джунглевая трава на грязи
            [TileID.JungleGrass] = new DebrisMaterial("Джунглевая трава", TileID.JungleGrass, ItemID.MudBlock, 1.0f, DustID.JungleGrass),
            // Грибная трава на грязи
            [TileID.MushroomGrass] = new DebrisMaterial("Грибная трава", TileID.MushroomGrass, ItemID.MudBlock, 1.0f, DustID.Dirt),
            // Трава на пепле (Преисподняя)
            [TileID.AshGrass] = new DebrisMaterial("Пепельная трава", TileID.AshGrass, ItemID.AshBlock, 1.1f, DustID.Ash),
            
            // Биомные травы ( Corruption, Crimson, Hallow)
            [TileID.CorruptGrass] = new DebrisMaterial("Порочная трава", TileID.CorruptGrass, ItemID.DirtBlock, 1.0f, DustID.CorruptPlants),
            [TileID.CrimsonGrass] = new DebrisMaterial("Кровавая трава", TileID.CrimsonGrass, ItemID.DirtBlock, 1.0f, DustID.CrimsonPlants),
            [TileID.HallowedGrass] = new DebrisMaterial("Священная трава", TileID.HallowedGrass, ItemID.DirtBlock, 1.0f, DustID.HallowedPlants),

            // Песок
            [TileID.Sand] = new DebrisMaterial("Песок", TileID.Sand, ItemID.SandBlock, 1.4f, DustID.Sand),
            [TileID.Crimsand] = new DebrisMaterial("Багровый песок", TileID.Crimsand, ItemID.CrimsandBlock, 1.4f, DustID.CrimsonPlants),
            [TileID.Ebonsand] = new DebrisMaterial("Порочный песок", TileID.Ebonsand, ItemID.EbonsandBlock, 1.4f, DustID.CorruptPlants),
            [TileID.Pearlsand] = new DebrisMaterial("Жемчужный песок", TileID.Pearlsand, ItemID.PearlsandBlock, 1.4f, DustID.HallowedPlants),

            // Песчаник
            [TileID.Sandstone] = new DebrisMaterial("Песчаник", TileID.Sandstone, ItemID.Sandstone, 1.6f, DustID.Sand),
            [TileID.CorruptSandstone] = new DebrisMaterial("Порочный песчаник", TileID.CorruptSandstone, ItemID.CorruptSandstone, 1.6f, DustID.CorruptPlants),
            [TileID.CrimsonSandstone] = new DebrisMaterial("Багровый песчаник", TileID.CrimsonSandstone, ItemID.CrimsonSandstone, 1.6f, DustID.CrimsonPlants),
            [TileID.HallowSandstone] = new DebrisMaterial("Священный песчаник", TileID.HallowSandstone, ItemID.HallowSandstone, 1.6f, DustID.HallowedPlants),

            // Затвердевший песок
            [TileID.HardenedSand] = new DebrisMaterial("Затвердевший песок", TileID.HardenedSand, ItemID.HardenedSand, 1.8f, DustID.Sand),
            [TileID.CorruptHardenedSand] = new DebrisMaterial("Порочный затвердевший песок", TileID.CorruptHardenedSand, ItemID.CorruptHardenedSand, 1.8f, DustID.CorruptPlants),
            [TileID.CrimsonHardenedSand] = new DebrisMaterial("Багровый затвердевший песок", TileID.CrimsonHardenedSand, ItemID.CrimsonHardenedSand, 1.8f, DustID.CrimsonPlants),
            [TileID.HallowHardenedSand] = new DebrisMaterial("Священный затвердевший песок", TileID.HallowHardenedSand, ItemID.HallowHardenedSand, 1.8f, DustID.HallowedPlants),

            // Лёд
            [TileID.IceBlock] = new DebrisMaterial("Лёд", TileID.IceBlock, ItemID.IceBlock, 2.0f, DustID.Ice),
            [TileID.CorruptIce] = new DebrisMaterial("Порочный лёд", TileID.CorruptIce, ItemID.IceBlock, 2.0f, DustID.Ice),
            [TileID.FleshIce] = new DebrisMaterial("Кровавый лёд", TileID.FleshIce, ItemID.IceBlock, 2.0f, DustID.Blood),
        };

        public static bool TryGet(int tileType, out DebrisMaterial material)
        {
            return SoftTiles.TryGetValue(tileType, out material);
        }
    }
}
