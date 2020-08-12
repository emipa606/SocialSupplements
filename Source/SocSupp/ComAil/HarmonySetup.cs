using System;
using System.Reflection;
using HarmonyLib;
using Verse;

namespace ComAil
{
	// Token: 0x0200000C RID: 12
	[StaticConstructorOnStartup]
	public static class HarmonySetup
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00002ED2 File Offset: 0x000010D2
		static HarmonySetup()
		{
			new Harmony("SocialSupplements.RW.Pelador").PatchAll(Assembly.GetExecutingAssembly());
		}
	}
}
