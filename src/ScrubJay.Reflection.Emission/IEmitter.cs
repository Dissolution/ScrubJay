//namespace ScrubJay.Reflection.Emission;
//
//public class Emitter<S>
//    where S : Emitter<S>
//{
//    private readonly ILGenerator _generator;
//
//    public S Self => (S)this;
//
//    public Emitter(ILGenerator generator)
//    {
//        _generator = generator;
//    }
//
//    public S DefineLabel(out EmitterLabel label, [CallerArgumentExpression(nameof(label))] string? labelName = null)
//    {
//        Label lbl = _generator.DefineLabel();
//        
//    }
//}
//
//internal record class EmitterLabelInfo
//{
//    public EmitterLabel Label { get; }
//    public string? Name { get; }
//    public bool Defined { get; set; }
//    public bool Marked { get; set; }
//}
//
//[StructLayout(LayoutKind.Sequential, Size = sizeof(int), Pack = 0)]
//public readonly struct EmitterLabel
//{
//    public static bool operator ==(EmitterLabel left, EmitterLabel right) => left.Equals(right);
//    public static bool operator !=(EmitterLabel left, EmitterLabel right) => !left.Equals(right);
//}