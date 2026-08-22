using Microsoft.Xna.Framework;
using Terraria;
using Terraria.GameContent.UI.Elements;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;
using Terraria.UI;
using RealisticEarthquake.Common.Systems;

namespace RealisticEarthquake.Content.UI
{
    // Небольшая кнопка сбоку экрана, при нажатии разворачивается в панель выбора магнитуды (пункт 11).
    public class EarthquakeUIState : UIState
    {
        private UIPanel togglePanel;
        private UIPanel expandedPanel;

        // Храним ссылки на текстовые элементы, чтобы обновлять их текст каждый кадр в Update() -
        // это надёжнее, чем один раз выставить текст в OnInitialize(). OnInitialize() вызывается
        // очень рано (во время Mod.Load()), когда собственные .hjson-файлы локализации мода
        // ещё могут быть не до конца прогружены движком - из-за этого Language.GetTextValue(...)
        // мог не найти перевод именно в этот момент и закешировать неверный текст навсегда.
        // Обновление в Update() также даёт бонус: кнопка сама переключит язык, если игрок
        // поменяет язык игры прямо во время сессии, без перезахода.
        private UIText iconText;
        private UIText titleText;
        private UIText subtitleText;
        private UIText triggerLabelText;
        private UIText magnitudeValueText;

        private int selectedMagnitude = 5;
        private bool expanded;

        private const float ToggleWidth = 46f;
        private const float ToggleHeight = 46f;
        private const float PanelWidth = 210f;
        private const float PanelHeight = 150f;
        private const float TopOffset = 180f;
        private const float RightMargin = 14f;

        public override void OnInitialize()
        {
            // --- Кнопка-переключатель, всегда видна в углу экрана ---
            // ВАЖНО: используем ТОЛЬКО Left.Set(pixels, percent), без HAlign - иначе смещения складываются
            // и элемент улетает за пределы экрана.
            togglePanel = new UIPanel();
            togglePanel.Width.Set(ToggleWidth, 0f);
            togglePanel.Height.Set(ToggleHeight, 0f);
            togglePanel.Top.Set(TopOffset, 0f);
            togglePanel.Left.Set(-(ToggleWidth + RightMargin), 1f); // от правого края экрана
            togglePanel.BackgroundColor = new Color(60, 40, 30);
            togglePanel.OnLeftClick += (evt, el) => ToggleExpanded();
            togglePanel.OnMouseOver += (evt, el) => togglePanel.BackgroundColor = new Color(90, 60, 45);
            togglePanel.OnMouseOut += (evt, el) => togglePanel.BackgroundColor = new Color(60, 40, 30);

            // Текст ("...") здесь временный placeholder - реальный перевод подставится уже в первом Update().
            iconText = new UIText("...", 0.9f) { HAlign = 0.5f, VAlign = 0.5f };
            togglePanel.Append(iconText);
            Append(togglePanel);

            // --- Разворачиваемая панель настроек теста ---
            expandedPanel = new UIPanel();
            expandedPanel.Width.Set(PanelWidth, 0f);
            expandedPanel.Height.Set(PanelHeight, 0f);
            expandedPanel.Top.Set(TopOffset, 0f);
            expandedPanel.Left.Set(-(ToggleWidth + RightMargin + PanelWidth + 8f), 1f); // левее кнопки
            expandedPanel.BackgroundColor = new Color(40, 30, 25);

            titleText = new UIText("...", 0.8f) { HAlign = 0.5f };
            titleText.Top.Set(6, 0f);
            expandedPanel.Append(titleText);

            subtitleText = new UIText("...", 0.75f);
            subtitleText.Top.Set(38, 0f);
            subtitleText.Left.Set(15, 0f);
            expandedPanel.Append(subtitleText);

            UIText minusBtn = new UIText("[ - ]", 0.9f);
            minusBtn.Top.Set(65, 0f);
            minusBtn.Left.Set(15, 0f);
            minusBtn.OnLeftClick += (evt, el) => { selectedMagnitude = System.Math.Max(1, selectedMagnitude - 1); UpdateMagnitudeText(); };
            minusBtn.OnMouseOver += (evt, el) => minusBtn.TextColor = Color.Yellow;
            minusBtn.OnMouseOut += (evt, el) => minusBtn.TextColor = Color.White;
            expandedPanel.Append(minusBtn);

            magnitudeValueText = new UIText(selectedMagnitude.ToString(), 1.1f);
            magnitudeValueText.Top.Set(62, 0f);
            magnitudeValueText.Left.Set(95, 0f);
            expandedPanel.Append(magnitudeValueText);

            UIText plusBtn = new UIText("[ + ]", 0.9f);
            plusBtn.Top.Set(65, 0f);
            plusBtn.Left.Set(150, 0f);
            plusBtn.OnLeftClick += (evt, el) => { selectedMagnitude = System.Math.Min(10, selectedMagnitude + 1); UpdateMagnitudeText(); };
            plusBtn.OnMouseOver += (evt, el) => plusBtn.TextColor = Color.Yellow;
            plusBtn.OnMouseOut += (evt, el) => plusBtn.TextColor = Color.White;
            expandedPanel.Append(plusBtn);

            UIPanel triggerBtn = new UIPanel();
            triggerBtn.Width.Set(180, 0f);
            triggerBtn.Height.Set(32, 0f);
            triggerBtn.Top.Set(100, 0f);
            triggerBtn.Left.Set(15, 0f);
            triggerBtn.BackgroundColor = new Color(120, 60, 40);
            triggerBtn.OnLeftClick += (evt, el) => TriggerEarthquake();
            triggerBtn.OnMouseOver += (evt, el) => triggerBtn.BackgroundColor = new Color(160, 80, 50);
            triggerBtn.OnMouseOut += (evt, el) => triggerBtn.BackgroundColor = new Color(120, 60, 40);

            triggerLabelText = new UIText("...", 0.85f) { HAlign = 0.5f, VAlign = 0.5f };
            triggerBtn.Append(triggerLabelText);
            expandedPanel.Append(triggerBtn);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            // Подставляем актуальный перевод каждый кадр - дёшево (пара коротких строк),
            // зато гарантированно корректно независимо от порядка загрузки локализации мода.
            iconText.SetText(Language.GetTextValue("Mods.RealisticEarthquake.UI.ToggleButtonLabel"));
            titleText.SetText(Language.GetTextValue("Mods.RealisticEarthquake.UI.PanelTitle"));
            subtitleText.SetText(Language.GetTextValue("Mods.RealisticEarthquake.UI.MagnitudeLabel"));
            triggerLabelText.SetText(Language.GetTextValue("Mods.RealisticEarthquake.UI.TriggerButton"));
        }

        private void UpdateMagnitudeText() => magnitudeValueText.SetText(selectedMagnitude.ToString());

        private void ToggleExpanded()
        {
            expanded = !expanded;
            if (expanded)
                Append(expandedPanel);
            else
                RemoveChild(expandedPanel);
        }

        private void TriggerEarthquake()
        {
            if (Main.netMode == NetmodeID.MultiplayerClient)
                EarthquakeNetHandler.SendManualTriggerRequest(selectedMagnitude);
            else
                ModContent.GetInstance<EarthquakeSystem>().ManualTrigger(selectedMagnitude);

            Main.NewText(Language.GetTextValue("Mods.RealisticEarthquake.UI.TriggerRequestedMessage", selectedMagnitude), Color.Yellow);
        }
    }
}
