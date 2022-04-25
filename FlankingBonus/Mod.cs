using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Verse;
using HarmonyLib;
using UnityEngine;

namespace SK.FlankingBonus
{
    public class Mod : Verse.Mod
    {
        public static bool SHOULD_PRINT_LOG = false;
        public Mod(ModContentPack content) : base(content)
        {
            Harmony instance = new Harmony("rimworld.sk.flankingbonus");
            HarmonyPatcher.instance = instance;

            LongEventHandler.ExecuteWhenFinished(Init);
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
