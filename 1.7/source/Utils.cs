using System;
using System.Text;
using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace SK.FlankingBonus
{
    public static class Utils
    {
        public enum Direction
        {
            None,
            Side,
            Back,
            Front
        }

        /// <summary>
        /// Direction of the shooter in relation to the target of the most recent HitReportFor call on this thread.
        /// Thread static so a report built on another thread can never leak into this one. None is the default
        /// value of the enum, which is what threads that never built a report see
        /// </summary>
        [ThreadStatic]
        public static Direction LastShotReportDirectionCalculation;
        public static HashSet<DamageDef> whitelistedDamageTypes = new HashSet<DamageDef>();

        /// <summary>
        /// Determine the direction of the shooter pawn in relation to the target pawn
        /// Check 1.6/source/Resources folder for explanation image
        /// </summary>
        public static Direction DetermineDirectionInRelationTo(Pawn shooter, Pawn target)
        {
            // Vector3.Angle is always within 0 - 180 degrees, front is the 45 degrees to either side of the facing
            float diff = Vector3.Angle(target.Rotation.FacingCell.ToVector3(), shooter.DrawPos - target.DrawPos);
            if (diff < 45)
                return Direction.Front;
            else if (diff <= 135)
                return Direction.Side;
            return Direction.Back;
        }

        public static string AppendFlankDamage(string input)
        {
            StringBuilder builder = new StringBuilder(input);

            if (LastShotReportDirectionCalculation == Direction.Side)
            {
                builder.AppendLine("   " + "SK_FlankingBonus_TooltipFlankingBonusSideTitle".Translate());
                if (ModSettings.IsRangedDamageBonusEnabled)
                    builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusDamageText".Translate(ModSettings.sideFlankingDamageRangedBonus.ToStringPercent()));
                if (ModSettings.IsAimingBonusEnabled)
                {
                    if (ModSettings.sideFlankingAimChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusAimChanceText".Translate(ModSettings.sideFlankingAimChanceBonus.ToStringPercent()));
                    if (ModSettings.sideFlankingPassCoverChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusPassCoverChanceText".Translate(ModSettings.sideFlankingPassCoverChanceBonus.ToStringPercent()));
                }
            }
            else if (LastShotReportDirectionCalculation == Direction.Back)
            {
                builder.AppendLine("   " + "SK_FlankingBonus_TooltipFlankingBonusBackTitle".Translate());
                if (ModSettings.IsRangedDamageBonusEnabled)
                    builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusDamageText".Translate(ModSettings.backFlankingDamageRangedBonus.ToStringPercent()));
                if (ModSettings.IsAimingBonusEnabled)
                {
                    if (ModSettings.backFlankingAimChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusAimChanceText".Translate(ModSettings.backFlankingAimChanceBonus.ToStringPercent()));
                    if (ModSettings.backFlankingPassCoverChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusPassCoverChanceText".Translate(ModSettings.backFlankingPassCoverChanceBonus.ToStringPercent()));
                }
            }

            return builder.ToString();
        }


        public static void Init()
        {
            whitelistedDamageTypes.Add(DamageDefOf.Bullet);
            whitelistedDamageTypes.Add(DamageDefOf.Cut);
            whitelistedDamageTypes.Add(DamageDefOf.Blunt);
            whitelistedDamageTypes.Add(DamageDefOf.Crush);
            whitelistedDamageTypes.Add(DamageDefOf.Stab);
            whitelistedDamageTypes.Add(DamageDefOf.Bite);
            whitelistedDamageTypes.Add(DamageDefOf.Scratch);
        }
    }
}
