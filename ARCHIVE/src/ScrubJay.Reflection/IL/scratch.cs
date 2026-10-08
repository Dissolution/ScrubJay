namespace ScrubJay.Reflection.IL;

public partial class Instructions
{
    public static BareInstruction Nop() => new BareInstruction(OpCodes.Nop);
    
}