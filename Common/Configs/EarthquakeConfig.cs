using System.ComponentModel;
using Terraria.ModLoader.Config;

namespace RealisticEarthquake.Common.Configs
{
    // ServerSide - чтобы в мультиплеере у всех была одна и та же настройка (её определяет хост/сервер).
    public class EarthquakeConfig : ModConfig
    {
        public override ConfigScope Mode => ConfigScope.ServerSide;

        // ВАЖНО: атрибут [Tooltip("текст")] с литеральной строкой больше не используется -
        // он не только устаревший (warning CS0618), но и НИКОГДА не переводится на другие языки,
        // так как полностью игнорирует систему локализации tModLoader.
        // Вместо этого просто убираем атрибут: tModLoader САМ подставит текст из файлов
        // Localization/<язык>_Mods.RealisticEarthquake.hjson по автоматическому ключу вида
        // Mods.RealisticEarthquake.Configs.EarthquakeConfig.<ИмяПоля>.Tooltip - именно такая
        // структура ключей уже используется в обоих hjson-файлах.

        [Header("Frequency")]

        [DefaultValue(20f)]
        [Range(5f, 60f)]
        [Increment(1f)]
        [Slider]
        public float AverageIntervalMinutes;

        [Header("Magnitude")]

        [DefaultValue(9)]
        [Range(3, 10)]
        public int MaxMagnitude;

        [Header("Aftershocks")]

        [DefaultValue(true)]
        public bool EnableAftershocks;

        [DefaultValue(150)]
        [Range(30, 300)]
        public int AftershockPeriodSeconds;

        [Header("InterfaceAlerts")]

        [DefaultValue(true)]
        public bool ShowChatMessages;

        [DefaultValue(true)]
        public bool ShowDebugButton;

        [Header("DamageDestruction")]

        [DefaultValue(1f)]
        [Range(0.1f, 3f)]
        [Slider]
        public float DebrisDamageMultiplier;

        [DefaultValue(true)]
        public bool ImmersiveAudioDucking;
    }
}
