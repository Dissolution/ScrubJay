//namespace ScrubJay.Exceptions;
//
//public sealed class ExceptionData : IDictionary<string, object?>
//{
//    private readonly IDictionary _data;
//
//    public int Count => _data.Count;
//
//    bool ICollection<KeyValuePair<string, object?>>.IsReadOnly => _data.IsReadOnly;
//
//    public object? this[string key]
//    {
//        get
//        {
//            return _data[key];
//        }
//        set => _data[key] = value;
//    }
//
//    ICollection<string> IDictionary<string, object?>.Keys => _keys;
//
//    ICollection<object?> IDictionary<string, object?>.Values => _values;
//
//    public ExceptionData(IDictionary data)
//    {
//        _data = data;
//    }
//    
//    
//}