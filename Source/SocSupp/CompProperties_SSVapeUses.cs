using Verse;

namespace SSVapes;

public class CompProperties_SSVapeUses : CompProperties
{
    public readonly string SSVapeType = "empty";

    public int SSVapeUses;

    public CompProperties_SSVapeUses()
    {
        compClass = typeof(CompSSVapeUses);
    }
}