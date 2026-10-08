//namespace ScrubJay.Errors;
//
//partial class Ex
//{
//    [PublicAPI]
//    [StackTraceHidden]
//    public static class Enumeration
//    {
//        public static InvalidOperationException CollectionModified()
//        {
//            const string InvalidOperation_EnumFailedVersion = "Collection was modified; enumeration operation may not execute.";
//            return new InvalidOperationException(InvalidOperation_EnumFailedVersion);
//        }
//
//        public static InvalidOperationException CannotHappen()
//        {
//            const string InvalidOperation_EnumOpCantHappen = "Enumeration has either not started or has already finished.";
//            return new InvalidOperationException(InvalidOperation_EnumOpCantHappen);
//        }
//        
//        public static InvalidOperationException NotStarted()
//        {
//            return new InvalidOperationException("Enumeration has not started.");
//        }
//        
//        public static InvalidOperationException Finished()
//        {
//            return new InvalidOperationException("Enumeration has already finished.");
//        }
//    }
//}