using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace SSFlare;

public class SSFoodSupplyFlare : Gas
{
    public override void SpawnSetup(Map map, bool respawningAfterLoad)
    {
        base.SpawnSetup(map, true);
        destroyTick = Find.TickManager.TicksGame + def.gas.expireSeconds.RandomInRange.SecondsToTicks();
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref destroyTick, "destroyTick");
    }

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

    public void DoMSMedSupplies(Map map, IntVec3 DropSpot, List<Thing> things)
    {
        DropPodUtility.DropThingsNear(DropSpot, map, things, 110, false, true);
        Find.LetterStack.ReceiveLetter("SSFlare.FoodSupplyLabel".Translate(), "SSFlare.FoodSupplyEvent".Translate(),
            LetterDefOf.PositiveEvent, new TargetInfo(DropSpot, map));
    }

    public IntVec3 GetDropSpot(Map map, IntVec3 root)
    {
        return root;
    }

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