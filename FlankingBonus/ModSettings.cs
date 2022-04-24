using Verse;

namespace SK.FlankingBonus
{
    public class ModSettings : Verse.ModSettings
    {
        public static bool allowSideFlankingDamage = true;
        public static bool allowBackFlankingDamage = true;
        public static float sideFlankingDamageBonus = 0.1f;
        public static float backFlankingDamageBonus = 0.25f;

        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref allowBackFlankingDamage, "allowSideFlankingDamage", true);
            Scribe_Values.Look(ref allowBackFlankingDamage, "allowBackFlankingDamage", true);
            Scribe_Values.Look(ref sideFlankingDamageBonus, "sideFlankingDamageBonus", 0.1f);
            Scribe_Values.Look(ref backFlankingDamageBonus, "backFlankingDamageBonus", 0.25f);
        }
    }
}
