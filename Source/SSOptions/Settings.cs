using UnityEngine;
using Verse;

namespace SSOptions
{
    // Token: 0x02000003 RID: 3
    public class Settings : ModSettings
    {
        // Token: 0x04000002 RID: 2
        public float ResPct = 100f;

        // Token: 0x06000004 RID: 4 RVA: 0x00002084 File Offset: 0x00000284
        public void DoWindowContents(Rect canvas)
        {
            var listing_Standard = new Listing_Standard {ColumnWidth = canvas.width};
            listing_Standard.Begin(canvas);
            listing_Standard.Gap();
            checked
            {
                listing_Standard.Label("SSOpt.ResPct".Translate() + "  " + (int) ResPct);
                ResPct = (int) listing_Standard.Slider((int) ResPct, 10f, 200f);
                listing_Standard.Gap();
                Text.Font = GameFont.Tiny;
                listing_Standard.Label("          " + "SSOpt.ResWarn".Translate());
                listing_Standard.Gap();
                listing_Standard.Label("          " + "SSOpt.ResTip".Translate());
                Text.Font = GameFont.Small;
                listing_Standard.Gap();
                listing_Standard.End();
            }
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002182 File Offset: 0x00000382
        public override void ExposeData()
        {
            base.ExposeData();
            Scribe_Values.Look(ref ResPct, "ResPct", 100f);
        }
    }
}