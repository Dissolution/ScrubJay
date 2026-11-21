//namespace ScrubJay.Errors.Collections;
//
//[PublicAPI]
//public readonly record struct KeyValue
//{
//    public readonly string Key;
//    public readonly object? Value;
//
//    public KeyValue(string key, object? value)
//    {
//        Key = key;
//        Value = value;
//    }
//
//    public void Deconstruct(out string key, out object? value)
//    {
//        key = Key;
//        value = Value;
//    }
//
//    public override string ToString() => $"\"{Key}\": {Value}";
//}
//
//[PublicAPI]
//public sealed class KeyValues : IEnumerable<KeyValue>
//{
//
//    public void Add(string key, object? value)
//    {
//        
//    }
//
//    public void Add((string Key, object? Value) tuple)
//    {
//        
//    }
//
//    public void Add(DictionaryEntry entry)
//    {
//        
//    }
//
//    public void Add(KeyValuePair<string, object?> pair)
//    {
//        
//    }
//
//    public KeyValues()
//    {
//        
//    }
//
//    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
//
//    public IEnumerator<KeyValue> GetEnumerator() => throw new NotImplementedException();
//}