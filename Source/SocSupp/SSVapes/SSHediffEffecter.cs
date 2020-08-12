using System;
using System.Collections.Generic;
using Verse;

namespace SSVapes
{
	// Token: 0x02000004 RID: 4
	internal class SSHediffEffecter
	{
		// Token: 0x06000008 RID: 8 RVA: 0x00002364 File Offset: 0x00000564
		internal static bool HediffEffect(HediffDef hediffdef, float SeverityToApply, Pawn pawn, BodyPartRecord part, out bool immune)
		{
			immune = false;
			if (!pawn.RaceProps.IsMechanoid && hediffdef != null)
			{
				if (!SSHediffEffecter.ImmuneTo(pawn, hediffdef))
				{
					if (!pawn.health.WouldDieAfterAddingHediff(hediffdef, part, SeverityToApply))
					{
						Pawn_HealthTracker health = pawn.health;
						Hediff hediff;
						if (health == null)
						{
							hediff = null;
						}
						else
						{
							HediffSet hediffSet = health.hediffSet;
							hediff = ((hediffSet != null) ? hediffSet.GetFirstHediffOfDef(hediffdef, false) : null);
						}
						Hediff hashediff = hediff;
						if (hashediff != null)
						{
							hashediff.Severity += SeverityToApply;
							return true;
						}
						Hediff addhediff = HediffMaker.MakeHediff(hediffdef, pawn, part);
						addhediff.Severity = SeverityToApply;
						pawn.health.AddHediff(addhediff, part, null, null);
						return true;
					}
				}
				else
				{
					immune = true;
				}
			}
			return false;
		}

		// Token: 0x06000009 RID: 9 RVA: 0x00002400 File Offset: 0x00000600
		internal static bool ImmuneTo(Pawn pawn, HediffDef def)
		{
			List<Hediff> hediffs = pawn.health.hediffSet.hediffs;
			for (int i = 0; i < hediffs.Count; i++)
			{
				HediffStage curStage = hediffs[i].CurStage;
				if (curStage != null && curStage.makeImmuneTo != null)
				{
					for (int j = 0; j < curStage.makeImmuneTo.Count; j++)
					{
						if (curStage.makeImmuneTo[j] == def)
						{
							return true;
						}
					}
				}
			}
			return false;
		}

		// Token: 0x0600000A RID: 10 RVA: 0x00002470 File Offset: 0x00000670
		internal static bool HasHediff(Pawn pawn, HediffDef def)
		{
			Pawn_HealthTracker health = pawn.health;
			HediffSet HS = (health != null) ? health.hediffSet : null;
			return HS != null && HS.GetFirstHediffOfDef(def, false) != null;
		}
	}
}
