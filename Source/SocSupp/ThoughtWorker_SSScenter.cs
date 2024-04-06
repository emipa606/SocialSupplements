using RimWorld;
using Verse;

namespace SSScent;

public class ThoughtWorker_SSScenter : ThoughtWorker
{
    public readonly HediffDef SSHedCheckNeutral = HediffDefSSScents.HedSSScentNeutral;

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

        return SShedneutral is not { Visible: true } ? false : ThoughtState.ActiveAtStage(0);
    }

    [DefOf]
    public static class HediffDefSSScents
    {
        public static HediffDef HedSSScentNeutral;
    }
}