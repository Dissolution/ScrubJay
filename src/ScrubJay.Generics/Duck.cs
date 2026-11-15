//namespace ScrubJay.Generics;
//
//public class Scratch
//{
//    static Scratch()
//    {
//        Reflect<text>.Mimic<>()
//    }
//}
//
//[StructLayout(LayoutKind.Auto, Size = 0, Pack = 0)]
//public readonly ref struct Void
//{
//}
//
//public delegate R InstanceFunc<I, out R>(in I? instance)
//#if NET9_0_OR_GREATER
//    where I : allows ref struct
//    where R : allows ref struct
//#endif
//;
//
//public static class Reflect<T>
//    where T : allows ref struct
//{
//    public static InstanceFunc<T, R> Mimic<R>(string name)
//    {
//        throw new NotImplementedException();
//    }
//}