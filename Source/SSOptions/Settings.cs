using UnityEngine;
using Verse;

namespace SSOptions;

public class Settings : ModSettings
{
    public float ResPct = 100f;

    public void DoWindowContents(Rect canvas)
    {
        var listing_Standard = new Listing_Standard { ColumnWidth = canvas.width };
        listing_Standard.Begin(canvas);
        listing_Standard.Gap();
        checked
        {
            listing_Standard.Label("SSOpt.ResPct".Translate() + "  " + (int)ResPct);
            ResPct = (int)listing_Standard.Slider((int)ResPct, 10f, 200f);
            listing_Standard.Gap();
            Text.Font = GameFont.Tiny;
            listing_Standard.Label("          " + "SSOpt.ResWarn".Translate());
            listing_Standard.Gap();
            listing_Standard.Label("          " + "SSOpt.ResTip".Translate());
            Text.Font = GameFont.Small;
            if (Controller.currentVersion != null)
            {
                listing_Standard.Gap();
                GUI.contentColor = Color.gray;
                listing_Standard.Label("SSOpt.CurrentModVersion".Translate(Controller.currentVersion));
                GUI.contentColor = Color.white;
            }

            listing_Standard.End();
        }
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref ResPct, "ResPct", 100f);
    }
}