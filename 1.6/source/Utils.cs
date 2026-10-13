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
        public static List<DamageDef> whitelistedDamageTypes = new List<DamageDef>();

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
