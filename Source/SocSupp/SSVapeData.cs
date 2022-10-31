using System.Collections.Generic;
using Verse;

namespace SSVapes;

public class SSVapeData : ThingWithComps
{
    public string SSVapeType = "empty";

    public int SSVapeUses;

    public CompSSVapeUses VapeUses => GetComp<CompSSVapeUses>();

    public override void PostMake()
    {
        base.PostMake();
        SSVapeUses = VapeUses.Props.SSVapeUses;
        SSVapeType = VapeUses.Props.SSVapeType;
    }

    public override void ExposeData()
    {
        base.ExposeData();
        Scribe_Values.Look(ref SSVapeUses, "SSVapeUses");
        Scribe_Values.Look(ref SSVapeType, "SSVapeType", "empty");
    }

    public override IEnumerable<Gizmo> GetGizmos()
    {
        yield return new Gizmo_VapeStatus
        {
            Vaper = this
        };
        foreach (var item in base.GetGizmos())
        {
            yield return item;
        }
    }
}