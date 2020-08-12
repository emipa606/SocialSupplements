using System;
using System.Collections.Generic;
using Verse;

namespace SSVapes
{
	// Token: 0x02000007 RID: 7
	public class SSVapeData : ThingWithComps
	{
		// Token: 0x17000002 RID: 2
		// (get) Token: 0x0600000F RID: 15 RVA: 0x000024E0 File Offset: 0x000006E0
		public CompSSVapeUses VapeUses
		{
			get
			{
				return base.GetComp<CompSSVapeUses>();
			}
		}

		// Token: 0x06000010 RID: 16 RVA: 0x000024E8 File Offset: 0x000006E8
		public override void PostMake()
		{
			base.PostMake();
			this.SSVapeUses = this.VapeUses.Props.SSVapeUses;
			this.SSVapeType = this.VapeUses.Props.SSVapeType;
		}

		// Token: 0x06000011 RID: 17 RVA: 0x0000251C File Offset: 0x0000071C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.SSVapeUses, "SSVapeUses", 0, false);
			Scribe_Values.Look<string>(ref this.SSVapeType, "SSVapeType", "empty", false);
			bool flag = Scribe.mode == LoadSaveMode.LoadingVars;
			bool flag2 = Scribe.mode == LoadSaveMode.PostLoadInit;
		}

		// Token: 0x06000012 RID: 18 RVA: 0x00002569 File Offset: 0x00000769
		public override IEnumerable<Gizmo> GetGizmos()
		{
			yield return new Gizmo_VapeStatus
			{
				Vaper = this
			};
			foreach (Gizmo item in base.GetGizmos())
			{
				yield return item;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x04000008 RID: 8
		public int SSVapeUses;

		// Token: 0x04000009 RID: 9
		public string SSVapeType = "empty";
	}
}
