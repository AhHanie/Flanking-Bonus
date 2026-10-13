using Verse;

namespace SK.FlankingBonus
{
    public class ModSettings : Verse.ModSettings
    {
        public static float sideFlankingDamageRangedBonus = 0.1f;
        public static float backFlankingDamageRangedBonus = 0.25f;
        public static float sideFlankingDamageMeleeBonus = 0.1f;
        public static float backFlankingDamageMeleeBonus = 0.25f;
        public static float sideFlankingAimChanceBonus = 0.02f;
        public static float sideFlankingPassCoverChanceBonus = 0.01f;
        public static float sideFlankingMeleeHitChanceBonus = 0.01f;
        public static float backFlankingAimChanceBonus = 0.04f;
        public static float backFlankingPassCoverChanceBonus = 0.02f;
        public static float backFlankingMeleeHitChanceBonus = 0.02f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref sideFlankingDamageRangedBonus, "sideFlankingDamageRangedBonus", 0.1f);
            Scribe_Values.Look(ref backFlankingDamageRangedBonus, "backFlankingDamageRangedBonus", 0.25f);
            Scribe_Values.Look(ref sideFlankingDamageMeleeBonus, "sideFlankingDamageMeleeBonus", 0.1f);
            Scribe_Values.Look(ref backFlankingDamageMeleeBonus, "backFlankingDamageMeleeBonus", 0.25f);
            Scribe_Values.Look(ref sideFlankingAimChanceBonus, "sideFlankingAimChanceBonus", 0.02f);
            Scribe_Values.Look(ref sideFlankingPassCoverChanceBonus, "sideFlankingPassCoverChanceBonus", 0.01f);
            Scribe_Values.Look(ref sideFlankingMeleeHitChanceBonus, "sideFlankingMeleeHitChanceBonus", 0.01f);
            Scribe_Values.Look(ref backFlankingAimChanceBonus, "backFlankingAimChanceBonus", 0.04f);
            Scribe_Values.Look(ref backFlankingPassCoverChanceBonus, "backFlankingPassCoverChanceBonus", 0.02f);
            Scribe_Values.Look(ref backFlankingMeleeHitChanceBonus, "backFlankingMeleeHitChanceBonus", 0.02f);
        }

        public static bool IsAimingBonusEnabled
        {
            get
            {
                return sideFlankingAimChanceBonus > 0 || sideFlankingPassCoverChanceBonus > 0 || backFlankingAimChanceBonus > 0 || backFlankingPassCoverChanceBonus > 0;
            }
        }

        public static bool IsRangedDamageBonusEnabled
        {
            get
            {
                return sideFlankingDamageRangedBonus > 0 || backFlankingDamageRangedBonus > 0;
            }
        }

        public static bool IsMeleeDamageBonusEnabled
        {
            get
            {
                return sideFlankingDamageMeleeBonus > 0 || backFlankingDamageMeleeBonus > 0;
            }
        }

        public static bool IsMeleeHitChanceBonusEnabled
        {
            get
            {
                return sideFlankingMeleeHitChanceBonus > 0 || backFlankingMeleeHitChanceBonus > 0;
            }
        }

        public static void ResetToDefaults()
        {
            sideFlankingDamageRangedBonus = 0.1f;
            backFlankingDamageRangedBonus = 0.25f;
            sideFlankingDamageMeleeBonus = 0.1f;
            backFlankingDamageMeleeBonus = 0.25f;
            sideFlankingAimChanceBonus = 0.02f;
            sideFlankingPassCoverChanceBonus = 0.01f;
            sideFlankingMeleeHitChanceBonus = 0.01f;
            backFlankingAimChanceBonus = 0.04f;
            backFlankingPassCoverChanceBonus = 0.02f;
            backFlankingMeleeHitChanceBonus = 0.02f;
        }
    }
}
