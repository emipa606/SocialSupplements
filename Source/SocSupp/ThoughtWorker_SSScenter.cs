using RimWorld;
using Verse;

namespace SSScent
{
    // Token: 0x0200000A RID: 10
    public class ThoughtWorker_SSScenter : ThoughtWorker
    {
        // Token: 0x0400000A RID: 10
        public HediffDef SSHedCheckNeutral = HediffDefSSScents.HedSSScentNeutral;

        // Token: 0x0600001B RID: 27 RVA: 0x00002B64 File Offset: 0x00000D64
        protected override ThoughtState CurrentSocialStateInternal(Pawn pawn, Pawn other)
        {
            if (!other.RaceProps.Humanlike || !RelationsUtility.PawnsKnowEachOther(pawn, other))
            {
                return false;
            }

            if (!pawn.health.capacities.CapableOf(PawnCapacityDefOf.Breathing))
            {
                return false;
            }

            Hediff SShedneutral;
            if (other.health == null)
            {
                SShedneutral = null;
            }
            else
            {
                var hedSet = other.health.hediffSet;
                SShedneutral = hedSet?.GetFirstHediffOfDef(SSHedCheckNeutral);
            }

            if (SShedneutral == null || !SShedneutral.Visible)
            {
                return false;
            }

            return ThoughtState.ActiveAtStage(0);
        }

        // Token: 0x02000011 RID: 17
        [DefOf]
        public static class HediffDefSSScents
        {
            // Token: 0x04000015 RID: 21
            public static HediffDef HedSSScentNeutral;
        }
    }
}