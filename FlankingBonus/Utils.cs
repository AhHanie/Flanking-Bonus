using System.Text;
using Verse;
using UnityEngine;

namespace SK.FlankingBonus
{
    public static class Utils
    {
        public enum Direction
        {
            Side,
            Back,
            Front
        }

        public static Direction LastShotReportDirectionCalculation = Direction.Front;

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
            if (ModSettings.allowSideFlankingDamage && LastShotReportDirectionCalculation == Direction.Side)
                builder.AppendLine("   " + "SK_FlankingBonus_TooltipFlankingSideBonusText".Translate(ModSettings.sideFlankingDamageBonus.ToStringPercent()));
            else if (ModSettings.allowBackFlankingDamage && LastShotReportDirectionCalculation == Direction.Back)
                builder.AppendLine("   " + "SK_FlankingBonus_TooltipFlankingBackBonusText".Translate(ModSettings.backFlankingDamageBonus.ToStringPercent()));
        }
    }
}
