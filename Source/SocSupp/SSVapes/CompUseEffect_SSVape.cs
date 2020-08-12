using System;
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
			if (this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperRegular || this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperFruity || this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperMenthol)
			{
				SSVapeUtility.DoSSVape(usedBy, this.parent);
			}
		}

		// Token: 0x06000002 RID: 2 RVA: 0x000020A8 File Offset: 0x000002A8
		public override bool CanBeUsedBy(Pawn p, out string failReason)
		{
			failReason = null;
			if (this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperRegular || this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperMenthol || this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperFruity || this.parent.def == SSVapeDefOf.ThingDefOf.SSVaperEmpty)
			{
				string Reason;
				bool Passed;
				SSVapeUtility.ChkSSVape(p, this.parent, out Reason, out Passed);
				if (!Passed)
				{
					failReason = Reason;
					return false;
				}
			}
			return true;
		}
	}
}
