using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;
using RimWorld;
using UnityEngine;

namespace SK.FlankingBonus
{
    public class HarmonyPatcher
    {
        public static Harmony instance;
        public static void PatchVanillaMethods()
        {
            if (instance == null)
            {
                Logger.WriteToHarmonyFile("Missing harmony instance");
                return;
            }

            // Patch ShotReport HitReportFor
            MethodInfo hitReportForMethod = AccessTools.Method(typeof(ShotReport), "HitReportFor");
            HarmonyMethod hitReportForPrefixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("HitReportForPrefixPatch"));
            instance.Patch(hitReportForMethod, hitReportForPrefixPatch);

            if (ModSettings.IsMeleeDamageBonusEnabled || ModSettings.IsRangedDamageBonusEnabled)
            {
                // Patch DamageWorker_AddInjury ApplyToPawn
                MethodInfo applyToPawnMethod = AccessTools.Method(typeof(DamageWorker_AddInjury), "ApplyToPawn");
                HarmonyMethod applyToPawnPrefixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("ApplyToPawnPrefixPatch"));
                instance.Patch(applyToPawnMethod, applyToPawnPrefixPatch);
            }

            if (ModSettings.IsAimingBonusEnabled)
            {
                // Patch ShotReport get_AimOnTargetChance_IgnoringPosture
                MethodInfo get_AimOnTargetChance_IgnoringPostureMethod = AccessTools.Method(typeof(ShotReport), "get_AimOnTargetChance_IgnoringPosture");
                HarmonyMethod get_AimOnTargetChance_IgnoringPostureMethodPostfixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("GetAimOnTargetIgnoringPostureChancePostfixPatch"));
                instance.Patch(get_AimOnTargetChance_IgnoringPostureMethod, null, get_AimOnTargetChance_IgnoringPostureMethodPostfixPatch);

                // Patch ShotReport get_PassCoverChanceMethod
                MethodInfo get_PassCoverChanceMethod = AccessTools.Method(typeof(ShotReport), "get_PassCoverChance");
                HarmonyMethod get_PassCoverChanceMethodPostfixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("GetPassCoverChancePostfixPatch"));
                instance.Patch(get_PassCoverChanceMethod, null, get_PassCoverChanceMethodPostfixPatch);
            }

            if (ModSettings.IsMeleeHitChanceBonusEnabled)
            {
                // Patch Verb_MeleeAttack GetNonMissChance
                MethodInfo getNonMissChanceMethod = AccessTools.Method(typeof(Verb_MeleeAttack), "GetNonMissChance");
                HarmonyMethod getNonMissChancePostfixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("GetNonMissChancePostfixPatch"));
                instance.Patch(getNonMissChanceMethod, null, getNonMissChancePostfixPatch);
            }

            if (ModSettings.IsAimingBonusEnabled || ModSettings.IsRangedDamageBonusEnabled)
            {
                // Patch ShotReport GetTextReadout
                MethodInfo getTextReadoutMethod = AccessTools.Method(typeof(ShotReport), "GetTextReadout");
                HarmonyMethod getTextReadoutPostfixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("GetTextReadoutPostfixPatch"));
                instance.Patch(getTextReadoutMethod, null, getTextReadoutPostfixPatch);
            }
        }

        // Before applying damage to a pawn
        public static bool ApplyToPawnPrefixPatch(DamageInfo dinfo, Pawn pawn)
        {
            
            if (!(dinfo.Instigator is Pawn instigator) || !Utils.whitelistedDamageTypes.Contains(dinfo.Def)) return true;
            bool isRangedWeapon = false;
            if (dinfo.Weapon != null) {
                isRangedWeapon = dinfo.Weapon.IsRangedWeapon;
            }
            else
            {
                return true;
            }

            Utils.Direction dir = Utils.DetermineDirectionInRelationTo(instigator, pawn);
            if (dir == Utils.Direction.Side)
            {
                dinfo.SetAmount(dinfo.Amount + dinfo.Amount * (isRangedWeapon ? ModSettings.sideFlankingDamageRangedBonus : ModSettings.sideFlankingDamageMeleeBonus));
            }
            else if (dir == Utils.Direction.Back)
            {
                dinfo.SetAmount(dinfo.Amount + dinfo.Amount * (isRangedWeapon ? ModSettings.backFlankingDamageRangedBonus : ModSettings.backFlankingDamageMeleeBonus));
            }
            return true;
        }

        /// <summary>
        /// Used to cache calculated direction of caster pawn in relation to target pawn of last HitReportFor
        /// </summary>
        public static bool HitReportForPrefixPatch(Thing caster, LocalTargetInfo target)
        {
            if (!target.HasThing || !(target.Thing is Pawn pawnTarget) || !(caster is Pawn casterPawn))
            {
                Utils.LastShotReportDirectionCalculation = Utils.Direction.None;
                return true;
            }
            Utils.LastShotReportDirectionCalculation = Utils.DetermineDirectionInRelationTo(casterPawn, pawnTarget);
            return true;
        }

        /// <summary>
        /// Add extra bonus chance to aim. This getter is used in Verb_LaunchProjectile
        /// to calculate whether a shot will miss or not
        /// </summary>
        /// <param name="__result"></param>
        public static void GetAimOnTargetIgnoringPostureChancePostfixPatch(ref float __result)
        {
            if (Utils.LastShotReportDirectionCalculation == Utils.Direction.Side)
                __result = Mathf.Min(__result + ModSettings.sideFlankingAimChanceBonus, 1f);
            else if (Utils.LastShotReportDirectionCalculation == Utils.Direction.Back)
                __result = Mathf.Min(__result + ModSettings.backFlankingAimChanceBonus, 1f);
        }

        /// <summary>
        /// Add extra bonus chance to pass cover chance. This getter is used in
        /// Verb_LaunchProjectile to calculate whether a shot will pass cover
        /// or not
        /// </summary>
        public static void GetPassCoverChancePostfixPatch(ref float __result)
        {
            if (Utils.LastShotReportDirectionCalculation == Utils.Direction.Side)
                __result = Mathf.Min(__result + ModSettings.sideFlankingPassCoverChanceBonus, 1f);
            else if (Utils.LastShotReportDirectionCalculation == Utils.Direction.Back)
                __result = Mathf.Min(__result + ModSettings.backFlankingPassCoverChanceBonus, 1f);
        }

        /// <summary>
        /// Add extra bonus chance to melee hit chance
        /// </summary>
        public static void GetNonMissChancePostfixPatch(Verb_MeleeAttack __instance, LocalTargetInfo target, ref float __result)
        {
            if (!target.HasThing || !(target.Thing is Pawn targetPawn) || !__instance.CasterIsPawn) return;
            Utils.Direction dir = Utils.DetermineDirectionInRelationTo(__instance.CasterPawn, targetPawn);
            if (dir == Utils.Direction.Side)
                __result = Mathf.Min(__result + ModSettings.sideFlankingMeleeHitChanceBonus, 1f);
            else if (dir == Utils.Direction.Back)
                __result = Mathf.Min(__result + ModSettings.backFlankingMeleeHitChanceBonus, 1f);
        }

        public static void GetTextReadoutPostfixPatch(ref string __result)
        {
            __result = Utils.AppendFlankDamage(__result);
        }
    }
}
