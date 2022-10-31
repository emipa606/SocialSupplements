using System;
using System.Collections.Generic;
using Verse;

namespace SSOptions;

[StaticConstructorOnStartup]
internal static class SSOptions_Initializer
{
    static SSOptions_Initializer()
    {
        LongEventHandler.QueueLongEvent(Setup, "LibraryStartup", false, null);
    }

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
            Resbase = checked((int)Math.Round(Resbase * (Controller.Settings.ResPct / 100f)));
            ResDef.baseCost = Resbase;
        }
    }

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