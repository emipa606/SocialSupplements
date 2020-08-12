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
			this.destroyTick = Find.TickManager.TicksGame + this.def.gas.expireSeconds.RandomInRange.SecondsToTicks();
		}

		// Token: 0x0600001E RID: 30 RVA: 0x00002C5C File Offset: 0x00000E5C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.destroyTick, "destroyTick", 0, false);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x00002C78 File Offset: 0x00000E78
		public override void Tick()
		{
			if (this.destroyTick <= Find.TickManager.TicksGame)
			{
				this.Destroy(DestroyMode.Vanish);
			}
			this.graphicRotation += this.graphicRotationSpeed;
			if (!this.DestroyedOrNull())
			{
				Map TargetMap = base.Map;
				IntVec3 TargetCell = base.Position;
				if (Find.TickManager.TicksGame % 10 == 0)
				{
					MoteMaker.ThrowSmoke(this.TrueCenter() + new Vector3(0f, 0f, 0.1f), TargetMap, 1f);
				}
				if (Find.TickManager.TicksGame % 300 == 0)
				{
					IntVec3 DropSpot = this.GetDropSpot(TargetMap, TargetCell);
					List<Thing> supplies = this.GetSupplies(this.SupplyList());
					this.DoMSMedSupplies(TargetMap, DropSpot, supplies);
					if (!this.DestroyedOrNull())
					{
						this.Destroy(DestroyMode.Vanish);
					}
				}
			}
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002D44 File Offset: 0x00000F44
		public void DoMSMedSupplies(Map map, IntVec3 DropSpot, List<Thing> things)
		{
			DropPodUtility.DropThingsNear(DropSpot, map, things, 110, false, true, true, true);
			Find.LetterStack.ReceiveLetter("SSFlare.FoodSupplyLabel".Translate(), "SSFlare.FoodSupplyEvent".Translate(), LetterDefOf.PositiveEvent, new TargetInfo(DropSpot, map, false), null, null, null, null);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x00002D93 File Offset: 0x00000F93
		public IntVec3 GetDropSpot(Map map, IntVec3 root)
		{
			return root;
		}

		// Token: 0x06000022 RID: 34 RVA: 0x00002D98 File Offset: 0x00000F98
		public List<ThingDef> SupplyList()
		{
			List<ThingDef> list = new List<ThingDef>();
			ThingDef def = null;
			for (int count = 0; count < 6; count++)
			{
				switch (count)
				{
				case 0:
					def = DefDatabase<ThingDef>.GetNamed("MSMultiVitamins", false);
					break;
				case 1:
					def = DefDatabase<ThingDef>.GetNamed("SSMealTravelPackSpicy", false);
					break;
				case 2:
					def = DefDatabase<ThingDef>.GetNamed("MealSurvivalPack", false);
					break;
				case 3:
					def = DefDatabase<ThingDef>.GetNamed("SSMintChocolate", false);
					break;
				case 4:
					def = DefDatabase<ThingDef>.GetNamed("SSPowerBar", false);
					break;
				case 5:
					def = DefDatabase<ThingDef>.GetNamed("SSBlitz_Zest", false);
					break;
				}
				if (def != null)
				{
					list.Add(def);
				}
			}
			return list;
		}

		// Token: 0x06000023 RID: 35 RVA: 0x00002E3C File Offset: 0x0000103C
		public List<Thing> GetSupplies(List<ThingDef> list)
		{
			List<Thing> supplies = new List<Thing>();
			foreach (ThingDef def in list)
			{
				Thing thing = this.MakeSupplies(def);
				if (thing != null)
				{
					supplies.Add(thing);
				}
			}
			return supplies;
		}

		// Token: 0x06000024 RID: 36 RVA: 0x00002E9C File Offset: 0x0000109C
		public Thing MakeSupplies(ThingDef def)
		{
			Thing supply = null;
			if (def != null)
			{
				supply = ThingMaker.MakeThing(def, null);
				supply.stackCount = Math.Min(10, def.stackLimit);
			}
			return supply;
		}
	}
}
