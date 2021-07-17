using RimWorld;
using Verse;

namespace SSVapes
{
    // Token: 0x02000002 RID: 2
    public class CompUseEffect_SSVape : CompUseEffect
    {
        // Token: 0x06000001 RID: 1 RVA: 0x00002050 File Offset: 0x00000250
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

        // Token: 0x06000002 RID: 2 RVA: 0x000020A8 File Offset: 0x000002A8
        public override bool CanBeUsedBy(Pawn p, out string failReason)
        {
            failReason = null;
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

            failReason = Reason;
            return false;
        }
    }
}