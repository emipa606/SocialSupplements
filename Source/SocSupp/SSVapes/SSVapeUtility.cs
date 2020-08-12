using System;
using RimWorld;
using UnityEngine;
using Verse;

namespace SSVapes
{
	// Token: 0x02000009 RID: 9
	public class SSVapeUtility
	{
		// Token: 0x06000016 RID: 22 RVA: 0x0000259C File Offset: 0x0000079C
		public static void DoSSVape(Pawn p, Thing t)
		{
			string HediffName = "";
			string defName = t.def.defName;
			if (!(defName == "SSVaperRegular"))
			{
				if (!(defName == "SSVaperFruity"))
				{
					if (defName == "SSVaperMenthol")
					{
						HediffName = "SSVape_Menthol_High";
					}
				}
				else
				{
					HediffName = "SSVape_Fruity_High";
				}
			}
			else
			{
				HediffName = "SSVape_Regular_High";
			}
			if (HediffName.StartsWith("SSVape"))
			{
				HediffDef hediffdef = DefDatabase<HediffDef>.GetNamed(HediffName, false);
				ChemicalDef SLChemDef = DefDatabase<ChemicalDef>.GetNamed("Smokeleaf", true);
				if (hediffdef != null)
				{
					float SeverityToApply = 0.5f;
					bool immune;
					if (SSHediffEffecter.HediffEffect(hediffdef, SeverityToApply, p, null, out immune))
					{
						HediffDef SLTol = DefDatabase<HediffDef>.GetNamed("SmokeleafTolerance", true);
						if (SLTol != null)
						{
							float Sev = 0.02f / p.BodySize;
							bool immune2;
							SSHediffEffecter.HediffEffect(SLTol, Sev, p, null, out immune2);
						}
						if (p.RaceProps.IsFlesh)
						{
							HediffDef addictionHediffDef = DefDatabase<HediffDef>.GetNamed("SmokeleafAddiction", true);
							Hediff_Addiction hediff_Addiction = AddictionUtility.FindAddictionHediff(p, SLChemDef);
							Hediff hediff = AddictionUtility.FindToleranceHediff(p, SLChemDef);
							float num = (hediff != null) ? hediff.Severity : 0f;
							if (hediff_Addiction != null)
							{
								hediff_Addiction.Severity += 0.06f;
							}
							else if (Rand.Value < 0.01f && num >= 0.15f)
							{
								p.health.AddHediff(addictionHediffDef, null, null, null);
								if (PawnUtility.ShouldSendNotificationAbout(p))
								{
									Find.LetterStack.ReceiveLetter("LetterLabelNewlyAddicted".Translate(SLChemDef.label).CapitalizeFirst(), "LetterNewlyAddicted".Translate(p.LabelShort, SLChemDef.label, p.Named("PAWN")).AdjustedFor(p, "PAWN", true).CapitalizeFirst(), LetterDefOf.NegativeEvent, p, null, null, null, null);
								}
							}
							if (addictionHediffDef.causesNeed != null)
							{
								Need need = p.needs.AllNeeds.Find((Need x) => x.def == addictionHediffDef.causesNeed);
								if (need != null)
								{
									float effect = 1f;
									AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(p, SLChemDef, ref effect);
									need.CurLevel += effect;
								}
							}
						}
					}
					if (((p != null) ? p.needs : null) != null)
					{
						Need need2 = p.needs.TryGetNeed(NeedDefOf.Rest);
						if (need2 != null)
						{
							float effect2 = -0.1f;
							AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(p, SLChemDef, ref effect2);
							need2.CurLevel += effect2;
						}
					}
					bool flag;
					if (p == null)
					{
						flag = (null != null);
					}
					else
					{
						Pawn_NeedsTracker needs = p.needs;
						flag = (((needs != null) ? needs.joy : null) != null);
					}
					if (flag)
					{
						JoyKindDef Chem = DefDatabase<JoyKindDef>.GetNamed("Chemical", false);
						Pawn_NeedsTracker needs2 = p.needs;
						if (needs2 != null)
						{
							needs2.joy.GainJoy(0.85f, Chem);
						}
					}
					if (p.Map != null)
					{
						Vector3 BreathOffset = new Vector3(0f, 0f, -0.04f);
						Vector3 loc = p.Drawer.DrawPos + p.Drawer.renderer.BaseHeadOffsetAt(p.Rotation) + p.Rotation.FacingCell.ToVector3() * 0.21f + BreathOffset;
						for (int i = 0; i < 4; i++)
						{
							SSVapeUtility.ThrowVapeBreathPuff(loc, p.Map, p.Rotation.AsAngle, p.Position.ToVector3());
							MoteMaker.ThrowSmoke(p.Position.ToVector3(), p.Map, 0.5f);
						}
					}
					int usesLeft = (t as SSVapeData).SSVapeUses;
					usesLeft--;
					(t as SSVapeData).SSVapeUses = usesLeft;
					if (usesLeft <= 0)
					{
						t.Destroy(DestroyMode.Vanish);
						Thing newVapeThing;
						GenDrop.TryDropSpawn(ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("SSVaperEmpty", false), null), p.Position, p.Map, ThingPlaceMode.Near, out newVapeThing, null, null);
						(newVapeThing as SSVapeData).SSVapeType = "empty";
						(newVapeThing as SSVapeData).SSVapeUses = 0;
					}
				}
			}
		}

		// Token: 0x06000017 RID: 23 RVA: 0x000029AC File Offset: 0x00000BAC
		public static void ThrowVapeBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (loc.ToIntVec3().ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
			{
				MoteThrown moteThrown = SSVapeUtility.NewBaseVapePuff();
				moteThrown.exactPosition = loc;
				moteThrown.exactPosition += new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f));
				moteThrown.SetVelocity(throwAngle + (float)Rand.Range(-1, 1), Rand.Range(0.1f, 0.4f));
				moteThrown.Velocity += inheritVelocity * 0.0025f;
				moteThrown.Scale = Rand.Range(1.1f, 1.2f);
				GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
			}
		}

		// Token: 0x06000018 RID: 24 RVA: 0x00002A7D File Offset: 0x00000C7D
		public static MoteThrown NewBaseVapePuff()
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_AirPuff, null);
			moteThrown.Scale = 1.5f;
			moteThrown.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			return moteThrown;
		}

		// Token: 0x06000019 RID: 25 RVA: 0x00002AB0 File Offset: 0x00000CB0
		public static void ChkSSVape(Pawn p, Thing vape, out string Reason, out bool Passed)
		{
			Reason = null;
			if (!p.RaceProps.Humanlike)
			{
				Passed = false;
				Reason = "SSVapes.NotHumanLike".Translate((p != null) ? p.LabelShort.CapitalizeFirst() : null);
				return;
			}
			if (!(vape is SSVapeData))
			{
				Passed = false;
				Reason = "SSVapes.NotAVaper".Translate(vape.Label.CapitalizeFirst());
				return;
			}
			if ((vape as SSVapeData).SSVapeUses <= 0)
			{
				Passed = false;
				Reason = "SSVapes.NoUsesLeft".Translate(vape.Label.CapitalizeFirst());
				return;
			}
			Passed = true;
		}
	}
}
