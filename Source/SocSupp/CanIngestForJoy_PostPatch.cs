using HarmonyLib;
using RimWorld;
using Verse;

namespace SocSupp
{
    // Token: 0x0200000D RID: 13
    [HarmonyPatch(typeof(JoyGiver_Ingest), "CanIngestForJoy")]
    public class CanIngestForJoy_PostPatch
    {
        // Token: 0x06000027 RID: 39 RVA: 0x00002EE8 File Offset: 0x000010E8
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
}