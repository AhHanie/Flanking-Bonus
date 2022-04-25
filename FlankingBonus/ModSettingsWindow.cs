using UnityEngine;
using Verse;

namespace SK.FlankingBonus
{
    public static class ModSettingsWindow
    {
        public static void Draw(Rect parent)
        {
            Listing_Standard list = new Listing_Standard(GameFont.Medium);
            Widgets.DrawBox(parent, 1, Texture2D.whiteTexture);
            Widgets.DrawMenuSection(parent);
            Rect inner = new Rect(parent.x + 5, parent.y + 20, parent.width - 20, parent.height - 20);
            list.Begin(inner);

            if (Widgets.ButtonText(new Rect(parent.x + 650, parent.y - 30, 175, 30), "SK_FlankingBonus_ModSettingsResetDefaultsButton".Translate()))
                ModSettings.ResetToDefaults();

            // Side Flanking
            list.Label("SK_FlankingBonus_ModSettingsAllowSideFlanking".Translate());
            Text.Font = GameFont.Small;
            list.Label("SK_FlankingBonus_ModSettingsSideFlankingDamageBonus".Translate());
            ModSettings.sideFlankingDamageBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingDamageBonus, 0, 1, false, ModSettings.sideFlankingDamageBonus.ToStringPercent(), null, null, 0.01f);
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
            list.Label("SK_FlankingBonus_ModSettingsBackFlankingDamageBonus".Translate());
            ModSettings.backFlankingDamageBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingDamageBonus, 0, 1, false, ModSettings.backFlankingDamageBonus.ToStringPercent(), null, null, 0.01f);
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
        }
    }
}
