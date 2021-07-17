using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SSFlare
{
    // Token: 0x0200000B RID: 11
    public class SSFoodSupplyFlare : Gas
    {
        // Token: 0x0600001D RID: 29 RVA: 0x00002C27 File Offset: 0x00000E27
        public override void SpawnSetup(Map map, bool respawningAfterLoad)
        {
            base.SpawnSetup(map, true);
            destroyTick = Find.TickManager.TicksGame + def.gas.expireSeconds.RandomInRange.SecondsToTicks();
        }

        // Token: 0x0600001E RID: 30 RVA: 0x00002C5C File Offset: 0x00000E5C
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref destroyTick, "destroyTick");
        }

        // Token: 0x0600001F RID: 31 RVA: 0x00002C78 File Offset: 0x00000E78
        public override void Tick()
        {
            if (destroyTick <= Find.TickManager.TicksGame)
            {
                Destroy();
            }

            graphicRotation += graphicRotationSpeed;
            if (this.DestroyedOrNull())
            {
                return;
            }

            var TargetMap = Map;
            var TargetCell = Position;
            if (Find.TickManager.TicksGame % 10 == 0)
            {
                FleckMaker.ThrowSmoke(this.TrueCenter() + new Vector3(0f, 0f, 0.1f), TargetMap, 1f);
            }

            if (Find.TickManager.TicksGame % 300 != 0)
            {
                return;
            }

            var DropSpot = GetDropSpot(TargetMap, TargetCell);
            var supplies = GetSupplies(SupplyList());
            DoMSMedSupplies(TargetMap, DropSpot, supplies);
            if (!this.DestroyedOrNull())
            {
                Destroy();
            }
        }

        // Token: 0x06000020 RID: 32 RVA: 0x00002D44 File Offset: 0x00000F44
        public void DoMSMedSupplies(Map map, IntVec3 DropSpot, List<Thing> things)
        {
            DropPodUtility.DropThingsNear(DropSpot, map, things, 110, false, true);
            Find.LetterStack.ReceiveLetter("SSFlare.FoodSupplyLabel".Translate(), "SSFlare.FoodSupplyEvent".Translate(),
                LetterDefOf.PositiveEvent, new TargetInfo(DropSpot, map));
        }

        // Token: 0x06000021 RID: 33 RVA: 0x00002D93 File Offset: 0x00000F93
        public IntVec3 GetDropSpot(Map map, IntVec3 root)
        {
            return root;
        }

        // Token: 0x06000022 RID: 34 RVA: 0x00002D98 File Offset: 0x00000F98
        public List<ThingDef> SupplyList()
        {
            var list = new List<ThingDef>();
            ThingDef named = null;
            for (var count = 0; count < 6; count++)
            {
                switch (count)
                {
                    case 0:
                        named = DefDatabase<ThingDef>.GetNamed("MSMultiVitamins", false);
                        break;
                    case 1:
                        named = DefDatabase<ThingDef>.GetNamed("SSMealTravelPackSpicy", false);
                        break;
                    case 2:
                        named = DefDatabase<ThingDef>.GetNamed("MealSurvivalPack", false);
                        break;
                    case 3:
                        named = DefDatabase<ThingDef>.GetNamed("SSMintChocolate", false);
                        break;
                    case 4:
                        named = DefDatabase<ThingDef>.GetNamed("SSPowerBar", false);
                        break;
                    case 5:
                        named = DefDatabase<ThingDef>.GetNamed("SSBlitz_Zest", false);
                        break;
                }

                if (named != null)
                {
                    list.Add(named);
                }
            }

            return list;
        }

        // Token: 0x06000023 RID: 35 RVA: 0x00002E3C File Offset: 0x0000103C
        public List<Thing> GetSupplies(List<ThingDef> list)
        {
            var supplies = new List<Thing>();
            foreach (var thingDef in list)
            {
                var thing = MakeSupplies(thingDef);
                if (thing != null)
                {
                    supplies.Add(thing);
                }
            }

            return supplies;
        }

        // Token: 0x06000024 RID: 36 RVA: 0x00002E9C File Offset: 0x0000109C
        public Thing MakeSupplies(ThingDef thingDef)
        {
            if (thingDef == null)
            {
                return null;
            }

            var supply = ThingMaker.MakeThing(thingDef);
            supply.stackCount = Math.Min(10, thingDef.stackLimit);

            return supply;
        }
    }
}