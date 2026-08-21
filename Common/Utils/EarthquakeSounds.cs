using Terraria.Audio;

namespace RealisticEarthquake.Common.Utils
{
    // Кастомные звуки, добавленные в RealisticEarthquake/Assets/Sounds/*.ogg
    // IsLooped не используется - EarthquakeSystem сам периодически переигрывает нужный сэмпл
    // с актуальной громкостью, это надёжнее, чем держать ссылку на "живой" зацикленный проигрыватель.
    public static class EarthquakeSounds
    {
        // Нарастающий гул перед началом (пункт 3).
        public static readonly SoundStyle Rumble = new SoundStyle("RealisticEarthquake/Assets/Sounds/Rumble");

        // Основной грохот во время активной фазы землетрясения.
        public static readonly SoundStyle MainQuake = new SoundStyle("RealisticEarthquake/Assets/Sounds/Earthquake");

        // Короткий звук одного афтершока - проигрывается один раз на каждый всплеск.
        public static readonly SoundStyle AftershockBurst = new SoundStyle("RealisticEarthquake/Assets/Sounds/Aftershock");
    }
}
