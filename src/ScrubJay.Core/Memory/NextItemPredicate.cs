namespace ScrubJay.Memory;

[PublicAPI]
public delegate bool NextItemPredicate<in T>(T item);