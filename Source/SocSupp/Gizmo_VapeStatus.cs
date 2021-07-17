using UnityEngine;
using Verse;

namespace SSVapes
{
    // Token: 0x02000003 RID: 3
    [StaticConstructorOnStartup]
    internal class Gizmo_VapeStatus : Gizmo
    {
        // Token: 0x04000005 RID: 5
        private const float ArrowScale = 0.5f;

        // Token: 0x04000002 RID: 2
        private static readonly Texture2D FullBarTex =
            SolidColorMaterials.NewSolidColorTexture(new Color(0.35f, 0.35f, 0.2f));

        // Token: 0x04000003 RID: 3
        private static readonly Texture2D EmptyBarTex = SolidColorMaterials.NewSolidColorTexture(Color.black);

        // Token: 0x04000004 RID: 4
        private static readonly Texture2D TargetLevelArrow =
            ContentFinder<Texture2D>.Get("UI/Misc/BarInstantMarkerRotated");

        // Token: 0x04000001 RID: 1
        public ThingWithComps Vaper;

        // Token: 0x06000004 RID: 4 RVA: 0x00002121 File Offset: 0x00000321
        public Gizmo_VapeStatus()
        {
            order = -304f;
        }

        // Token: 0x06000005 RID: 5 RVA: 0x00002134 File Offset: 0x00000334
        public override float GetWidth(float maxWidth)
        {
            return 140f;
        }

        // Token: 0x06000006 RID: 6 RVA: 0x0000213C File Offset: 0x0000033C
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
            if (((SSVapeData) Vaper).VapeUses.Props.SSVapeUses > 0)
            {
                fillPercent = ((SSVapeData) Vaper).SSVapeUses /
                              (float) ((SSVapeData) Vaper).VapeUses.Props.SSVapeUses;
            }

            Widgets.FillableBar(rect3, fillPercent, FullBarTex, EmptyBarTex, false);
            Text.Font = GameFont.Small;
            Text.Anchor = TextAnchor.MiddleCenter;
            Widgets.Label(rect3,
                (Vaper as SSVapeData)?.SSVapeUses.ToString("F0") + " / " +
                (Vaper as SSVapeData)?.VapeUses.Props.SSVapeUses.ToString("F0"));
            Text.Anchor = TextAnchor.UpperLeft;
            return new GizmoResult(GizmoState.Clear);
        }
    }
}