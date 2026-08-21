using Terraria.Audio;

namespace RealisticEarthquake.Common.Utils
{
    // Кастомные звуки, добавленные в RealisticEarthquake/Assets/Sounds/*.ogg
    // IsLooped не используется - EarthquakeSystem сам периодически переигрывает нужный сэмпл
    // с актуальной громкостью, это надёжнее, чем держать ссылку на "живой" зацикленный проигрыватель.
    public static class EarthquakeSounds
    {
        // Нарастающий гул перед началом (пункт 3).
        public static readonly SoundStyle Rumble = new SoundStyle("RealisticEarthquake/Assets/Sounds/Rumble")
        {
            MaxInstances = 1, // Не позволяет звуку перезапускаться поверх себя
            Volume = 1f
        };

        public static readonly SoundStyle MainQuake = new SoundStyle("RealisticEarthquake/Assets/Sounds/Earthquake")
        {
            MaxInstances = 1,
            Volume = 1f
        };

        public static readonly SoundStyle AftershockBurst = new SoundStyle("RealisticEarthquake/Assets/Sounds/Aftershock")
        {
            MaxInstances = 1,
            Volume = 1f
        };
    }
}
