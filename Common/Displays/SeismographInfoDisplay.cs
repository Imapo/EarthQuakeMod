using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Localization;
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

            // Весь текст берётся из файлов локализации (Localization/*.hjson), чтобы одинаково
            // хорошо работать и на русском, и на английском клиенте - без хардкода строк в коде.
            const string prefix = "Mods.RealisticEarthquake.InfoDisplays.SeismographInfoDisplay.";

            return EarthquakeSystem.CurrentState switch
            {
                EarthquakeState.Idle => Language.GetTextValue(prefix + "Idle", FormatTime(EarthquakeSystem.TicksUntilNextEarthquake)),
                EarthquakeState.Warning => Language.GetTextValue(prefix + "Warning", FormatTime(EarthquakeSystem.TicksRemainingInState)),
                EarthquakeState.Main => Language.GetTextValue(prefix + "Main", EarthquakeSystem.CurrentMagnitude),
                EarthquakeState.Aftershock => Language.GetTextValue(prefix + "Aftershock"),
                _ => Language.GetTextValue(prefix + "Unknown")
            };
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
