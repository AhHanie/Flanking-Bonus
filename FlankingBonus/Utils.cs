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
            Side,
            Back,
            Front,
            None
        }

        public static Direction LastShotReportDirectionCalculation = Direction.None;
        public static List<DamageDef> blacklistedDamageTypes = new List<DamageDef>();

        /// <summary>
        /// Determine the direction of the shooter pawn in relation to the target pawn
        /// Check resources folder for explanation image
        /// </summary>
        public static Direction DetermineDirectionInRelationTo(Pawn shooter, Pawn target)
        {
            float diff = Vector3.Angle(target.Rotation.FacingCell.ToVector3(), shooter.DrawPos - target.DrawPos);
            if ((diff >= 315.01 && diff <= 360) || (diff >= 0 && diff < 45))
                return Direction.Front;
            else if ((diff >= 45 && diff <= 135) || (diff >= 225.01 && diff <= 315))
                return Direction.Side;
            return Direction.Back;
        }

        // Called by transpiler
        public static void AppendFlankDamage(StringBuilder builder)
        {
            if (LastShotReportDirectionCalculation == Direction.Side)
            {
                builder.AppendLine("   " + "SK_FlankingBonus_TooltipFlankingBonusSideTitle".Translate());
                if (ModSettings.IsDamageBonusEnabled)
                    builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusDamageText".Translate(ModSettings.sideFlankingDamageBonus.ToStringPercent()));
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
                if (ModSettings.IsDamageBonusEnabled)
                    builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusDamageText".Translate(ModSettings.backFlankingDamageBonus.ToStringPercent()));
                if (ModSettings.IsAimingBonusEnabled)
                {
                    if (ModSettings.backFlankingAimChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusAimChanceText".Translate(ModSettings.backFlankingAimChanceBonus.ToStringPercent()));
                    if (ModSettings.backFlankingPassCoverChanceBonus > 0)
                        builder.AppendLine("      " + "SK_FlankingBonus_TooltipFlankingBonusPassCoverChanceText".Translate(ModSettings.backFlankingPassCoverChanceBonus.ToStringPercent()));
                }
            }
        }

        public static void Init()
        {
            blacklistedDamageTypes.Add(DamageDefOf.Flame);
            blacklistedDamageTypes.Add(DamageDefOf.Bomb);
        }
    }
}
