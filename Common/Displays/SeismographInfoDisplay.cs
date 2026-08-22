using Microsoft.Xna.Framework;
using Terraria;
using Terraria.ID;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Players;
using RealisticEarthquake.Common.Systems;

namespace RealisticEarthquake.Common.Displays
{
    public class SeismographInfoDisplay : InfoDisplay
    {
        public override bool Active()
        {
            // Показываем этот индикатор только если у игрока есть сейсмограф в инвентаре
            return Main.LocalPlayer.GetModPlayer<EarthquakePlayer>().HasSeismograph;
        }

        public override string DisplayValue(ref Color displayColor, ref Color displayShadowColor)
        {
            // Цвет текста: оранжево-красный во время землетрясения, иначе золотистый (как у часов)
            displayColor = EarthquakeSystem.CurrentState == EarthquakeState.Main ? Color.OrangeRed : Color.LightGoldenrodYellow;
            displayShadowColor = Color.Black;

            string text = EarthquakeSystem.CurrentState switch
            {
                EarthquakeState.Idle => $"Сейсмограф: {FormatTime(EarthquakeSystem.TicksUntilNextEarthquake)}",
                EarthquakeState.Warning => $"Сейсмограф: ГУЛ! ({FormatTime(EarthquakeSystem.TicksRemainingInState)})",
                EarthquakeState.Main => $"ЗЕМЛЕТРЯСЕНИЕ! (Магнитуда {EarthquakeSystem.CurrentMagnitude}/10)",
                EarthquakeState.Aftershock => "Сейсмограф: афтершоки",
                _ => "Сейсмограф: --"
            };

            return text;
        }

        private static string FormatTime(int ticks)
        {
            int totalSeconds = System.Math.Max(0, ticks / 60);
            int minutes = totalSeconds / 60;
            int seconds = totalSeconds % 60;
            return $"{minutes:00}:{seconds:00}";
        }
    }
}