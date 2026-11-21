namespace ScrubJay.Errors;

public static partial class Ex
{
    [PublicAPI]
    [StackTraceHidden]
    public static class Enumeration
    {
        [StackTraceHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InvalidOperationException CollectionModified()
        {
            const string MESSAGE = "Collection was modified; enumeration operation may not execute.";
            return new InvalidOperationException(MESSAGE);
        }

        [StackTraceHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InvalidOperationException CannotHappen()
        {
            const string MESSAGE = "Enumeration has either not started or has already finished.";
            return new InvalidOperationException(MESSAGE);
        }

        [StackTraceHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InvalidOperationException NotStarted()
        {
            const string MESSAGE = "Enumeration has not yet started.";
            return new InvalidOperationException(MESSAGE);
        }

        [StackTraceHidden]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static InvalidOperationException Finished()
        {
            const string MESSAGE = "Enumeration has already finished.";
            return new InvalidOperationException(MESSAGE);
        }
    }
}