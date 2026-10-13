using Verse;
using HarmonyLib;
using UnityEngine;

namespace SK.FlankingBonus
{
    public class Mod : Verse.Mod
    {
        public Mod(ModContentPack content) : base(content)
        {
            Harmony instance = new Harmony("rimworld.sk.flankingbonus");
            HarmonyPatcher.instance = instance;

            LongEventHandler.QueueLongEvent(Init, "SK.FlankingBonus.Init", true, null);
        }

        public override string SettingsCategory()
        {
            return "SK_FlankingBonus_SettingsListItemName".Translate();
        }

        public override void DoSettingsWindowContents(Rect rect)
        {
            ModSettingsWindow.Draw(rect);
            base.DoSettingsWindowContents(rect);
        }

        public void Init()
        {
            GetSettings<ModSettings>();
            HarmonyPatcher.PatchVanillaMethods();
            Utils.Init();
        }
    }
}
