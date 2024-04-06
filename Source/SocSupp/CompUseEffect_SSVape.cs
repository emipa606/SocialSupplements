using RimWorld;
using Verse;

namespace SSVapes;

public class CompUseEffect_SSVape : CompUseEffect
{
    public override void DoEffect(Pawn usedBy)
    {
        base.DoEffect(usedBy);
        if (parent.def == SSVapeDefOf.ThingDefOf.SSVaperRegular ||
            parent.def == SSVapeDefOf.ThingDefOf.SSVaperFruity ||
            parent.def == SSVapeDefOf.ThingDefOf.SSVaperMenthol)
        {
            SSVapeUtility.DoSSVape(usedBy, parent);
        }
    }

    public override AcceptanceReport CanBeUsedBy(Pawn p)
    {
        if (parent.def != SSVapeDefOf.ThingDefOf.SSVaperRegular &&
            parent.def != SSVapeDefOf.ThingDefOf.SSVaperMenthol &&
            parent.def != SSVapeDefOf.ThingDefOf.SSVaperFruity && parent.def != SSVapeDefOf.ThingDefOf.SSVaperEmpty)
        {
            return true;
        }

        SSVapeUtility.ChkSSVape(p, parent, out var Reason, out var Passed);
        if (Passed)
        {
            return true;
        }

        return Reason;
    }
}