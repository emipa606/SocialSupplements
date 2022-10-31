using HarmonyLib;
using RimWorld;
using Verse;

namespace SocSupp;

[HarmonyPatch(typeof(JoyGiver_Ingest), "CanIngestForJoy")]
public class CanIngestForJoy_PostPatch
{
    [HarmonyPostfix]
    [HarmonyPriority(0)]
    public static void PostFix(ref bool __result, Pawn pawn, Thing t)
    {
        if (!__result)
        {
            return;
        }

        if (!t.def.IsIngestible)
        {
            var ingestible = t.def.ingestible;
            if (ingestible?.joyKind == null)
            {
                var ingestible2 = t.def.ingestible;
                if (ingestible2 == null || ingestible2.joy <= 0f)
                {
                    return;
                }
            }
        }

        if (t.def.defName.StartsWith("SS") && t.def.defName.EndsWith("Spicy"))
        {
            __result = false;
        }
    }
}