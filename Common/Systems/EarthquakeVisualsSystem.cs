using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.GameContent;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Players;

namespace RealisticEarthquake.Common.Systems
{
    public class EarthquakeVisualsSystem : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            Player player = Main.LocalPlayer;
            if (player == null || !player.active)
                return;

            EarthquakePlayer eqPlayer = player.GetModPlayer<EarthquakePlayer>();

            // Метод DrawDustyHaze полностью удалён, чтобы курсор не пропадал
            DrawSeismographHud(spriteBatch, eqPlayer);
        }

        private void DrawSeismographHud(SpriteBatch spriteBatch, EarthquakePlayer eqPlayer)
        {
            if (!eqPlayer.HasSeismograph)
                return;

            string text = EarthquakeSystem.CurrentState switch
            {
                EarthquakeState.Idle => $"Сейсмограф: спокойно ({FormatTime(EarthquakeSystem.TicksUntilNextEarthquake)} до толчков)",
                EarthquakeState.Warning => $"Сейсмограф: НАРАСТАЮЩИЙ ГУЛ! Толчки через {FormatTime(EarthquakeSystem.TicksRemainingInState)}",
                EarthquakeState.Main => $"Сейсмограф: ЗЕМЛЕТРЯСЕНИЕ! Магнитуда {EarthquakeSystem.CurrentMagnitude}/10",
                EarthquakeState.Aftershock => "Сейсмограф: возможны афтершоки",
                _ => string.Empty
            };

            if (string.IsNullOrEmpty(text))
                return;

            Vector2 pos = new Vector2(16, 100);
            Color color = EarthquakeSystem.CurrentState == EarthquakeState.Main ? Color.OrangeRed : Color.LightGoldenrodYellow;

            spriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.None, RasterizerState.CullNone);
            Terraria.Utils.DrawBorderString(spriteBatch, text, pos, color, 0.9f);
            spriteBatch.End();
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