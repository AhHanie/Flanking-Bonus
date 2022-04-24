using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using Verse;

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

            // Patch DamageWorker_AddInjury ApplyToPawn
            MethodInfo applyToPawnMethod = AccessTools.Method(typeof(DamageWorker_AddInjury), "ApplyToPawn");
            HarmonyMethod applyToPawnPrefixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("ApplyToPawnPrefixPatch"));
            instance.Patch(applyToPawnMethod, applyToPawnPrefixPatch);

            // Patch ShotReport HitReportFor
            MethodInfo hitReportForMethod = AccessTools.Method(typeof(ShotReport), "HitReportFor");
            HarmonyMethod hitReportForPostfixPatch = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("HitReportForPostfixPatch"));
            instance.Patch(hitReportForMethod, null, hitReportForPostfixPatch);

            // Patch ShotReport GetTextReadout
            MethodInfo getTextReadoutMethod = AccessTools.Method(typeof(ShotReport), "GetTextReadout");
            HarmonyMethod getTextReadoutTranspiler = new HarmonyMethod(typeof(HarmonyPatcher).GetMethod("GetTextReadoutTranspiler"));
            instance.Patch(getTextReadoutMethod, null, null, getTextReadoutTranspiler);
        }

        // Before applying damage to a pawn
        public static bool ApplyToPawnPrefixPatch(DamageInfo dinfo, Pawn pawn)
        {
            if (!(dinfo.Instigator is Pawn instigator)) return true;
            Utils.Direction dir = Utils.DetermineDirectionInRelationTo(instigator, pawn);
            if (dir == Utils.Direction.Front) return true;
            else if (dir == Utils.Direction.Side && ModSettings.allowSideFlankingDamage)
                dinfo.SetAmount(dinfo.Amount + dinfo.Amount * ModSettings.sideFlankingDamageBonus);
            else if (ModSettings.allowBackFlankingDamage)
                dinfo.SetAmount(dinfo.Amount + dinfo.Amount * ModSettings.backFlankingDamageBonus);
            return true;
        }

        /// <summary>
        /// Used to cache calculated direction of caster pawn in relation to target pawn of last HitReportFor
        /// </summary>
        public static void HitReportForPostfixPatch(Thing caster, LocalTargetInfo target)
        {
            if (!target.HasThing || !(target.Thing is Pawn pawnTarget) || !(caster is Pawn casterPawn)) return;
            Utils.LastShotReportDirectionCalculation = Utils.DetermineDirectionInRelationTo(casterPawn, pawnTarget);
        }

        /// <summary>
        /// Add Utils.AppendFlankDamage call at the following line in ShotReport.GetTextReadout:
        /// 
        /// stringBuilder.AppendLine(" " + TotalEstimatedHitChance.ToStringPercent());
        /// stringBuilder.AppendLine("   " + "ShootReportShooterAbility".Translate() + "  " + factorFromShooterAndDist.ToStringPercent());
        /// stringBuilder.AppendLine("   " + "ShootReportWeapon".Translate() + "        " + factorFromEquipment.ToStringPercent());
        ///                                                      <---- ADDED HERE
        /// if (target.HasThing && factorFromTargetSize != 1f)
        /// 
        /// Displays flank damage text in tooltip
        /// 
        /// </summary>
        public static IEnumerable<CodeInstruction> GetTextReadoutTranspiler(IEnumerable<CodeInstruction> instructions)
        {
            int index = 0;
            foreach (CodeInstruction instruction in instructions)
            {
                // Bad ... check for a unique signature instead so code could
                // survive Rimworld updates. I am too dumb for that though *sighs*
                // For such a simple transpiler, I am not going to bother
                if (index == 75)
                {
                    yield return new CodeInstruction(OpCodes.Ldloc_0); // stringBuilder
                    yield return new CodeInstruction(OpCodes.Call, AccessTools.Method(typeof(Utils), "AppendFlankDamage"));
                }

                yield return instruction;
                index++;
            }
        }
    }
}
