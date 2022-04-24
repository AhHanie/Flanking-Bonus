using UnityEngine;
using Verse;

namespace SK.FlankingBonus
{
    public static class ModSettingsWindow
    {
        public static void Draw(Rect parent)
        {
            Listing_Standard list = new Listing_Standard(GameFont.Small);
            list.Begin(parent);
            list.CheckboxLabeled("SK_FlankingBonus_ModSettingsAllowSideFlanking".Translate(), ref ModSettings.allowSideFlankingDamage);
            if (ModSettings.allowSideFlankingDamage)
            {
                list.Label("SK_FlankingBonus_ModSettingsSideFlankingBonus".Translate());
                ModSettings.sideFlankingDamageBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.sideFlankingDamageBonus, 0, 1, false, ModSettings.sideFlankingDamageBonus.ToStringPercent(), null, null, 0.01f);
            }
            list.CheckboxLabeled("SK_FlankingBonus_ModSettingsAllowBackFlanking".Translate(), ref ModSettings.allowBackFlankingDamage);
            if (ModSettings.allowBackFlankingDamage)
            {
                list.Label("SK_FlankingBonus_ModSettingsSideFlankingBonus".Translate());
                ModSettings.backFlankingDamageBonus = Widgets.HorizontalSlider(list.GetRect(22f), ModSettings.backFlankingDamageBonus, 0, 1, false, ModSettings.backFlankingDamageBonus.ToStringPercent(), null, null, 0.01f);
            }
            list.End();
        }
    }
}
