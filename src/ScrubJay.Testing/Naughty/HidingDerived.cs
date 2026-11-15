namespace ScrubJay.Testing.Naughty;

public sealed class HidingDerived : HidingBase
{
    new public string Value { get; set; } = "hidden";
}