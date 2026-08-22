using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Systems;

namespace RealisticEarthquake.Common.Players
{
    public class EarthquakePlayer : ModPlayer
    {
        public int DustyHazeTimeLeft;
        public bool HasSeismograph;

        public override void ResetEffects()
        {
            // Сбрасываем каждый кадр. UpdateEquips установит его в true, если предмет в инвентаре.
            HasSeismograph = false;
            
            if (DustyHazeTimeLeft > 0)
                DustyHazeTimeLeft--;
        }

        public override void UpdateEquips()
        {
            if (HasSeismograph)
            {
                // В 1.4.4 accWatch — это int. 
                // 4 = платиновые часы. Это говорит игре, что у игрока есть инфо-аксессуар, 
                // и нужно отрисовать иконку часов и зарезервировать место для текста.
                Player.accWatch = 4;
            }
        }

        public override void ModifyScreenPosition()
        {
            float intensity = EarthquakeSystem.CurrentShakeIntensity;
            if (intensity <= 0f)
                return;

            if (RealisticEarthquake.Common.Utils.CeilingScanner.IsUndergroundOrBelow(Player))
            {
                // Полная тряска под землёй
                Main.screenPosition += new Vector2(
                    Main.rand.NextFloat(-intensity, intensity),
                    Main.rand.NextFloat(-intensity, intensity)
                );
            }
            else
            {
                // === НОВОЕ: Лёгкая вибрация на поверхности ===
                // Берём 15% от подземной интенсивности, но не менее 0.3f и не более 1.5f пикселя.
                // Это создаёт едва заметное, но ощутимое "эхо" толчков.
                float surfaceShake = MathHelper.Clamp(intensity * 0.15f, 0.3f, 1.5f);
                
                Main.screenPosition += new Vector2(
                    Main.rand.NextFloat(-surfaceShake, surfaceShake),
                    Main.rand.NextFloat(-surfaceShake, surfaceShake)
                );
            }
        }
    }
}