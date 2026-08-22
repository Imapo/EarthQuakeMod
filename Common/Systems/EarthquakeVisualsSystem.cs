using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.ModLoader;

namespace RealisticEarthquake.Common.Systems
{
    public class EarthquakeVisualsSystem : ModSystem
    {
        public override void PostDrawInterface(SpriteBatch spriteBatch)
        {
            // Отрисовка теперь полностью обрабатывается ванильной системой через SeismographInfoDisplay.
            // Это гарантирует, что курсор мыши никогда не пропадет.
        }
    }
}