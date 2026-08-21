using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Systems;

namespace RealisticEarthquake.Common.Players
{
    public class EarthquakePlayer : ModPlayer
    {
        // Сколько тиков ещё должна держаться "пыльная дымка" перед глазами (для отрисовки в EarthquakeVisualsSystem)
        public int DustyHazeTimeLeft;

        // Бонус: если у игрока есть сейсмограф - показываем предупреждения заранее и точнее.
        public bool HasSeismograph;

        public override void ResetEffects()
        {
            if (DustyHazeTimeLeft > 0)
                DustyHazeTimeLeft--;
        }

        // Официальный хук tModLoader/vanilla именно для тряски экрана камеры.
        public override void ModifyScreenPosition()
        {
            float intensity = EarthquakeSystem.CurrentShakeIntensity;
            if (intensity <= 0f)
                return;

            // Землетрясение ощущается только в подземелье и ниже - на поверхности только гул (без тряски).
            if (!RealisticEarthquake.Common.Utils.CeilingScanner.IsUndergroundOrBelow(Player))
                return;

            Main.screenPosition += new Vector2(
                Main.rand.NextFloat(-intensity, intensity),
                Main.rand.NextFloat(-intensity, intensity)
            );
        }
    }
}
