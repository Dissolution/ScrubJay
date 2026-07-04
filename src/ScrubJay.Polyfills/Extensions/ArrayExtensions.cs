using System.Runtime.Serialization;
using ScrubJay.Errors.Validation;

namespace ScrubJay.Polyfills;

[PublicAPI]
public static class ArrayExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static ref object? RefItemImpl(Array array, int index)
    {
        Emit.Ldarg(nameof(array));
        Emit.Ldarg(nameof(index));
        Emit.Ldelema<object?>();
        return ref ReturnRef<object?>();
    }
    
    extension(Array? array)
    {
        public ref object? RefItemAt(int index)
        {
            Throw.IfNull(array);
            if (array.Rank != 1)
                throw Ex.Arg(array, "rank must be 1");
            var lb = array.GetLowerBound(0);
            var ub = array.GetUpperBound(0);
            if (index < lb || index > ub)
                throw Ex.ArgRange(index, $"was not in [{lb}..{ub}]");
            return ref RefItemImpl(array, index);
        }
    }
    
    extension<T>(T[]? array)
    {
        public void SetAll(T item)
        {
            if (array is null) return;
            for (var i = 0; i < array.Length; i++)
            {
                array[i] = item;
            }
        }
        
        public void RefEach(RefAction<T>? refItem)
        {
            if (array is null || refItem is null)
                return;

            for (var i = 0; i < array.Length; i++)
            {
                refItem(ref array[i]);
            }
        }
    }
}