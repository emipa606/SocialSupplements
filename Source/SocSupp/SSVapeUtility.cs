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
            var HediffName = "";
            var defName = t.def.defName;
            switch (defName)
            {
                case "SSVaperRegular":
                    HediffName = "SSVape_Regular_High";
                    break;
                case "SSVaperFruity":
                    HediffName = "SSVape_Fruity_High";
                    break;
                case "SSVaperMenthol":
                    HediffName = "SSVape_Menthol_High";
                    break;
            }

            if (!HediffName.StartsWith("SSVape"))
            {
                return;
            }

            var hediffdef = DefDatabase<HediffDef>.GetNamed(HediffName, false);
            var SLChemDef = DefDatabase<ChemicalDef>.GetNamed("Smokeleaf");
            if (hediffdef == null)
            {
                return;
            }

            var SeverityToApply = 0.5f;
            if (SSHediffEffecter.HediffEffect(hediffdef, SeverityToApply, p, null, out _))
            {
                var SLTol = DefDatabase<HediffDef>.GetNamed("SmokeleafTolerance");
                if (SLTol != null)
                {
                    var Sev = 0.02f / p.BodySize;
                    SSHediffEffecter.HediffEffect(SLTol, Sev, p, null, out _);
                }

                if (p.RaceProps.IsFlesh)
                {
                    var addictionHediffDef = DefDatabase<HediffDef>.GetNamed("SmokeleafAddiction");
                    var hediff_Addiction = AddictionUtility.FindAddictionHediff(p, SLChemDef);
                    var hediff = AddictionUtility.FindToleranceHediff(p, SLChemDef);
                    var num = hediff?.Severity ?? 0f;
                    if (hediff_Addiction != null)
                    {
                        hediff_Addiction.Severity += 0.06f;
                    }
                    else if (Rand.Value < 0.01f && num >= 0.15f)
                    {
                        p.health.AddHediff(addictionHediffDef);
                        if (PawnUtility.ShouldSendNotificationAbout(p))
                        {
                            Find.LetterStack.ReceiveLetter(
                                "LetterLabelNewlyAddicted".Translate(SLChemDef.label).CapitalizeFirst(),
                                "LetterNewlyAddicted".Translate(p.LabelShort, SLChemDef.label, p.Named("PAWN"))
                                    .AdjustedFor(p).CapitalizeFirst(), LetterDefOf.NegativeEvent, p);
                        }
                    }

                    if (addictionHediffDef.causesNeed != null)
                    {
                        var need = p.needs.AllNeeds.Find(x => x.def == addictionHediffDef.causesNeed);
                        if (need != null)
                        {
                            var effect = 1f;
                            AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(p, SLChemDef,
                                ref effect);
                            need.CurLevel += effect;
                        }
                    }
                }
            }

            if (p?.needs != null)
            {
                var need2 = p.needs.TryGetNeed(NeedDefOf.Rest);
                if (need2 != null)
                {
                    var effect2 = -0.1f;
                    AddictionUtility.ModifyChemicalEffectForToleranceAndBodySize(p, SLChemDef, ref effect2);
                    need2.CurLevel += effect2;
                }
            }

            bool joyNeed;
            if (p == null)
            {
                joyNeed = false;
            }
            else
            {
                var needs = p.needs;
                joyNeed = needs?.joy != null;
            }

            if (joyNeed)
            {
                var Chem = DefDatabase<JoyKindDef>.GetNamed("Chemical", false);
                var needs2 = p.needs;
                if (needs2 != null)
                {
                    needs2.joy.GainJoy(0.85f, Chem);
                }
            }

            if (p?.Map != null)
            {
                var BreathOffset = new Vector3(0f, 0f, -0.04f);
                var loc = p.Drawer.DrawPos + p.Drawer.renderer.BaseHeadOffsetAt(p.Rotation) +
                          (p.Rotation.FacingCell.ToVector3() * 0.21f) + BreathOffset;
                for (var i = 0; i < 4; i++)
                {
                    ThrowVapeBreathPuff(loc, p.Map, p.Rotation.AsAngle, p.Position.ToVector3());
                    FleckMaker.ThrowSmoke(p.Position.ToVector3(), p.Map, 0.5f);
                }
            }

            var usesLeft = ((SSVapeData) t).SSVapeUses;
            usesLeft--;
            ((SSVapeData) t).SSVapeUses = usesLeft;
            if (usesLeft > 0)
            {
                return;
            }

            t.Destroy();
            if (p == null)
            {
                return;
            }

            GenDrop.TryDropSpawn(
                ThingMaker.MakeThing(DefDatabase<ThingDef>.GetNamed("SSVaperEmpty", false)), p.Position,
                p.Map, ThingPlaceMode.Near, out var newVapeThing);
            ((SSVapeData) newVapeThing).SSVapeType = "empty";
            ((SSVapeData) newVapeThing).SSVapeUses = 0;
        }

        // Token: 0x06000017 RID: 23 RVA: 0x000029AC File Offset: 0x00000BAC
        public static void ThrowVapeBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
        {
            if (loc.ToIntVec3().ShouldSpawnMotesAt(map) && !map.moteCounter.SaturatedLowPriority)
            {
                FleckMaker.ThrowAirPuffUp(
                    loc + new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f)), map);
                //fleckThrown.baseData.exactPosition = loc;
                //fleckThrown.baseData.exactPosition += ;
                //fleckThrown.SetVelocity(throwAngle + (float)Rand.Range(-1, 1), Rand.Range(0.1f, 0.4f));
                //fleckThrown.Velocity += inheritVelocity * 0.0025f;
                //fleckThrown.baseData.Scale = Rand.Range(1.1f, 1.2f);
                //GenSpawn.Spawn(fleckThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
            }
        }

        // Token: 0x06000018 RID: 24 RVA: 0x00002A7D File Offset: 0x00000C7D
        //public static FleckThrown NewBaseVapePuff()
        //{
        //	var moteThrown = FleckMaker.ThrowAirPuffUp() (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_AirPuff, null);
        //	moteThrown.Scale = 1.5f;
        //	moteThrown.rotationRate = (float)Rand.RangeInclusive(-240, 240);
        //	return moteThrown;
        //}

        // Token: 0x06000019 RID: 25 RVA: 0x00002AB0 File Offset: 0x00000CB0
        public static void ChkSSVape(Pawn p, Thing vape, out string Reason, out bool Passed)
        {
            Reason = null;
            if (!p.RaceProps.Humanlike)
            {
                Passed = false;
                Reason = "SSVapes.NotHumanLike".Translate(p.LabelShort.CapitalizeFirst());
                return;
            }

            if (!(vape is SSVapeData data))
            {
                Passed = false;
                Reason = "SSVapes.NotAVaper".Translate(vape.Label.CapitalizeFirst());
                return;
            }

            if (data.SSVapeUses <= 0)
            {
                Passed = false;
                Reason = "SSVapes.NoUsesLeft".Translate(data.Label.CapitalizeFirst());
                return;
            }

            Passed = true;
        }
    }
}