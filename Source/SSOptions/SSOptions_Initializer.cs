using System;
using System.Collections.Generic;
using Verse;

namespace SSOptions
{
    // Token: 0x02000004 RID: 4
    [StaticConstructorOnStartup]
    internal static class SSOptions_Initializer
    {
        // Token: 0x06000007 RID: 7 RVA: 0x000021B3 File Offset: 0x000003B3
        static SSOptions_Initializer()
        {
            LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
        }

        // Token: 0x06000008 RID: 8 RVA: 0x000021D0 File Offset: 0x000003D0
        public static void Setup()
        {
            var allDefs = DefDatabase<ResearchProjectDef>.AllDefsListForReading;
            if (allDefs.Count <= 0)
            {
                return;
            }

            var SSList = SSResearchList();
            foreach (var ResDef in allDefs)
            {
                if (!SSList.Contains(ResDef.defName))
                {
                    continue;
                }

                var Resbase = ResDef.baseCost;
                Resbase = checked((int) Math.Round(Resbase * (Controller.Settings.ResPct / 100f)));
                ResDef.baseCost = Resbase;
            }
        }

        // Token: 0x06000009 RID: 9 RVA: 0x00002268 File Offset: 0x00000468
        public static List<string> SSResearchList()
        {
            var list = new List<string>();
            list.AddDistinct("SSBlitz");
            list.AddDistinct("SSMouthWash");
            list.AddDistinct("SSVape");
            list.AddDistinct("SSScenters");
            list.AddDistinct("SSPolyflower");
            return list;
        }
    }
}