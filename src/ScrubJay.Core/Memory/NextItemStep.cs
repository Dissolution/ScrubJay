namespace ScrubJay.Memory;

[PublicAPI]
public delegate Option<int> NextItemStep<in T>(T item);