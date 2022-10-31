using UnityEngine;
using Verse;

namespace SSVapes;

[StaticConstructorOnStartup]
internal class Gizmo_VapeStatus : Gizmo
{
    private const float ArrowScale = 0.5f;

    private static readonly Texture2D FullBarTex =
        SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

    private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

    private static readonly Texture2D TargetLevelArrow =
        ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated");

    public ThingWithComps Vaper;

    public Gizmo_VapeStatus()
    {
        base.Order = -304f;
    }

    public override float GetWidth(float maxWidth)
    {
        return 140f;
    }

    public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth, GizmoRenderParms parms)
    {
        var overRect = new Rect(topLeft.x, topLeft.y, GetWidth(maxWidth), 75f);
        Widgets.DrawWindowBackground(overRect);
        var rect = overRect.ContractedBy(6f);
        var rect2 = rect;
        rect2.height = overRect.height / 2f;
        Text.Font = GameFont.Tiny;
        var readType = "Empty";
        var defName = Vaper.def.defName;
        if (defName != "SSVaperRegular")
        {
            if (defName != "SSVaperFruity")
            {
                if (defName == "SSVaperMenthol")
                {
                    readType = "Menthol";
                }
            }
            else
            {
                readType = "Fruity";
            }
        }
        else
        {
            readType = "Regular";
        }

        Widgets.Label(rect2, readType + (readType == "Empty" ? "" : " vape uses left"));
        var rect3 = rect;
        rect3.yMin = rect2.yMin + (overRect.height / 2f) - 6f;
        rect3.height = (overRect.height / 2f) - 6f;
        var fillPercent = 0f;
        if (((SSVapeData)Vaper).VapeUses.Props.SSVapeUses > 0)
        {
            fillPercent = ((SSVapeData)Vaper).SSVapeUses /
                          (float)((SSVapeData)Vaper).VapeUses.Props.SSVapeUses;
        }

        Widgets.FillableBar(rect3, fillPercent, FullBarTex, EmptyBarTex, false);
        Text.Font = GameFont.Small;
        Text.Anchor = TextAnchor.MiddleCenter;
        Widgets.Label(rect3,
            $"{(Vaper as SSVapeData)?.SSVapeUses:F0} / {(Vaper as SSVapeData)?.VapeUses.Props.SSVapeUses:F0}");
        Text.Anchor = TextAnchor.UpperLeft;
        return new GizmoResult(GizmoState.Clear);
    }
}