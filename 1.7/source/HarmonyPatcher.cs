using System.Reflection;
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
            // Patch ShotReport HitReportFor
            Patch(AccessTools.Method(typeof(ShotReport), nameof(ShotReport.HitReportFor), new[] { typeof(Thing), typeof(Verb), typeof(LocalTargetInfo) }),
                "ShotReport.HitReportFor", nameof(HitReportForPrefixPatch), null);

            if (ModSettings.IsMeleeDamageBonusEnabled || ModSettings.IsRangedDamageBonusEnabled)
            {
                // Patch DamageWorker_AddInjury ApplyToPawn
                Patch(AccessTools.Method(typeof(DamageWorker_AddInjury), "ApplyToPawn", new[] { typeof(DamageInfo), typeof(Pawn) }),
                    "DamageWorker_AddInjury.ApplyToPawn", nameof(ApplyToPawnPrefixPatch), null);
            }

            if (ModSettings.IsAimingBonusEnabled)
            {
                // Patch ShotReport get_AimOnTargetChance_IgnoringPosture
                Patch(AccessTools.PropertyGetter(typeof(ShotReport), nameof(ShotReport.AimOnTargetChance_IgnoringPosture)),
                    "ShotReport.AimOnTargetChance_IgnoringPosture", null, nameof(GetAimOnTargetIgnoringPostureChancePostfixPatch));

                // Patch ShotReport get_PassCoverChance
                Patch(AccessTools.PropertyGetter(typeof(ShotReport), nameof(ShotReport.PassCoverChance)),
                    "ShotReport.PassCoverChance", null, nameof(GetPassCoverChancePostfixPatch));
            }

            if (ModSettings.IsMeleeHitChanceBonusEnabled)
            {
                // Patch Verb_MeleeAttack GetNonMissChance
                Patch(AccessTools.Method(typeof(Verb_MeleeAttack), "GetNonMissChance", new[] { typeof(LocalTargetInfo) }),
                    "Verb_MeleeAttack.GetNonMissChance", null, nameof(GetNonMissChancePostfixPatch));
            }

            if (ModSettings.IsAimingBonusEnabled || ModSettings.IsRangedDamageBonusEnabled)
            {
                // Patch ShotReport GetTextReadout
                Patch(AccessTools.Method(typeof(ShotReport), nameof(ShotReport.GetTextReadout), System.Type.EmptyTypes),
                    "ShotReport.GetTextReadout", null, nameof(GetTextReadoutPostfixPatch));
            }
        }

        /// <summary>
        /// Patch the target with the named patch methods of this class. A target that no longer exists is
        /// reported in the log instead of failing the remaining patches
        /// </summary>
        private static void Patch(MethodBase target, string targetName, string prefixName, string postfixName)
        {
            if (target == null)
            {
                Log.Error("[Flanking Bonus] Could not find " + targetName + " to patch, the flanking bonus that depends on it will not work.");
                return;
            }
            HarmonyMethod prefix = prefixName == null ? null : new HarmonyMethod(typeof(HarmonyPatcher), prefixName);
            HarmonyMethod postfix = postfixName == null ? null : new HarmonyMethod(typeof(HarmonyPatcher), postfixName);
            instance.Patch(target, prefix, postfix);
        }

        // Before applying damage to a pawn. DamageInfo is a struct so it has to be passed by ref for the new amount to
        // reach the original method
        public static bool ApplyToPawnPrefixPatch(ref DamageInfo dinfo, Pawn pawn)
        {
            if (!(dinfo.Instigator is Pawn instigator) || dinfo.Weapon == null || !Utils.whitelistedDamageTypes.Contains(dinfo.Def)) return true;
            bool isRangedWeapon = dinfo.Weapon.IsRangedWeapon;

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
