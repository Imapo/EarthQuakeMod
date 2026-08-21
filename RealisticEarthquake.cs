using System.IO;
using Terraria.ModLoader;
using RealisticEarthquake.Common.Systems;

namespace RealisticEarthquake
{
    // Главный класс мода. Логика вынесена в Common/Systems/EarthquakeSystem.cs
    public class RealisticEarthquakeMod : Mod
    {
        public override void HandlePacket(BinaryReader reader, int whoAmI)
        {
            EarthquakeNetHandler.HandlePacket(reader, whoAmI);
        }
    }
}
