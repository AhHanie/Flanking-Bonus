using UnityEngine;
using Verse;

namespace SK.FlankingBonus
{
    public static class ModSettingsWindow
    {
        private static Vector2 scrollPosition = Vector2.zero;
        public static void Draw(Rect parent)
        {
            Listing_Standard list = new Listing_Standard(GameFont.Medium);
            Rect outerRect = new Rect(parent.x, parent.y + 20, parent.width, parent.height - 20);
            Rect innerRect = new Rect(outerRect);
            innerRect.x += 5;
            innerRect.width -= 35;
            innerRect.y += 10;
            innerRect.height -= 20;
            Rect scrollRect = new Rect(0f, innerRect.y, innerRect.width - 20f, parent.height * 1.1f);
            Widgets.DrawMenuSection(outerRect);
            Widgets.BeginScrollView(innerRect, ref scrollPosition, scrollRect, true);
            list.Begin(scrollRect);


            if (Widgets.ButtonText(new Rect(parent.x + 600, parent.y - 30, 175, 30), "SK_FlankingBonus_ModSettingsResetDefaultsButton".Translate()))
                ModSettings.ResetToDefaults();

            // Side Flanking
            list.Label("SK_FlankingBonus_ModSettingsAllowSideFlanking".Translate());
            Text.Font = GameFont.Small;
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingRangedDamageBonus".Translate());
            ModSettings.sideFlankingDamageRangedBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingDamageRangedBonus, 0, 1, false, ModSettings.sideFlankingDamageRangedBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingMeleeDamageBonus".Translate());
            ModSettings.sideFlankingDamageMeleeBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingDamageMeleeBonus, 0, 1, false, ModSettings.sideFlankingDamageMeleeBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingAimChanceBonus".Translate());
            ModSettings.sideFlankingAimChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingAimChanceBonus, 0, 1, false, ModSettings.sideFlankingAimChanceBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingPassCoverChanceBonus".Translate());
            ModSettings.sideFlankingPassCoverChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingPassCoverChanceBonus, 0, 1, false, ModSettings.sideFlankingPassCoverChanceBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingMeleeHitChanceBonus".Translate());
            ModSettings.sideFlankingMeleeHitChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingMeleeHitChanceBonus, 0, 1, false, ModSettings.sideFlankingMeleeHitChanceBonus.ToStringPercent(), null, null, 0.01f);
            Text.Font = GameFont.Medium;

            // Back Flanking
            list.Label("SK_FlankingBonus_ModSettingsAllowBackFlanking".Translate());
            Text.Font = GameFont.Small;
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingRangedDamageBonus".Translate());
            ModSettings.backFlankingDamageRangedBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingDamageRangedBonus, 0, 1, false, ModSettings.backFlankingDamageRangedBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingMeleeDamageBonus".Translate());
            ModSettings.backFlankingDamageMeleeBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingDamageMeleeBonus, 0, 1, false, ModSettings.backFlankingDamageMeleeBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingAimChanceBonus".Translate());
            ModSettings.backFlankingAimChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingAimChanceBonus, 0, 1, false, ModSettings.backFlankingAimChanceBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingPassCoverChanceBonus".Translate());
            ModSettings.backFlankingPassCoverChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingPassCoverChanceBonus, 0, 1, false, ModSettings.backFlankingPassCoverChanceBonus.ToStringPercent(), null, null, 0.01f);
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingMeleeHitChanceBonus".Translate());
            ModSettings.backFlankingMeleeHitChanceBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingMeleeHitChanceBonus, 0, 1, false, ModSettings.backFlankingMeleeHitChanceBonus.ToStringPercent(), null, null, 0.01f);
            Text.Font = GameFont.Medium;

            // Notes
            list.Label("SK_FlankingBonus_ModSettingsNotesSectionTitle".Translate());
            Text.Font = GameFont.Small;
            list.Label("SK_FlankingBonus_ModSettingsDamageDisableInfoLabel".Translate());
            list.Label("SK_FlankingBonus_ModSettingsAimChanceDisableInfoLabel".Translate());
            list.Label("SK_FlankingBonus_ModSettingsMeleeHitChanceDisableInfoLabel".Translate());
            list.Label("SK_FlankingBonus_ModSettingsRestartWarningLabel".Translate());
            list.End();
            Widgets.EndScrollView();
        }
    }
}
