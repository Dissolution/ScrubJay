using System.Collections.Concurrent;

namespace ScrubJay.Polyfills.Collections;

public class TypeDictionary<TValue> : Dictionary<Type, TValue>
{
    
}

public class ConcurrentTypeDictionary<TValue> : ConcurrentDictionary<Type, TValue>
{
    
}