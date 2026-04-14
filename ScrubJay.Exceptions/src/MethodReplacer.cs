using System.Reflection;

namespace ScrubJay.Exceptions;

// patches all instances of a type
//public static unsafe void PatchVTable(Type type, MethodInfo original, MethodInfo replacement)
//{
//    RuntimeHelpers.PrepareMethod(original.MethodHandle);
//    RuntimeHelpers.PrepareMethod(replacement.MethodHandle);
//
//    // The vtable slot is embedded in the MethodDesc
//    // TypeHandle -> MethodTable -> vtable slots
//    IntPtr* vtable = *(IntPtr**)type.TypeHandle.Value;
//
//    // Find the slot index (requires walking the MethodTable)
//    int slot = GetVTableSlot(original); // non-trivial — see below
//
//    IntPtr newPtr = replacement.GetFunctionPointer();
//    VirtualProtect((IntPtr)(vtable + slot), IntPtr.Size, 0x40, out _);
//    vtable[slot] = newPtr;
//}

internal static class MethodReplacer
{
    // We need different handling for 32 and 64-bit
    private static readonly bool _is64bit = IntPtr.Size == sizeof(long);

    private static readonly Dictionary<string, string> _replacements = [];

    private static string GetKey(MethodInfo method)
    {
        return $"{method.DeclaringType!.FullName}.{method.Name} @ 0x{method.MethodHandle.GetFunctionPointer().ToString("X" + (IntPtr.Size * 2))}";
    }

    public static unsafe bool TryReroute(MethodInfo? source, MethodInfo? destination)
    {
        // validate if the two methods are compatible
        if (source is null || destination is null)
            return false;
        if (destination.ReturnType != source.ReturnType)
            return false;
        if (destination.IsStatic != source.IsStatic)
            return false;
        var sourceParams = source.GetParameters();
        var destParams = destination.GetParameters();
        if (destParams.Length != sourceParams.Length)
            return false;
        for (var i = 0; i < sourceParams.Length; i++)
        {
            var sourceParam = sourceParams[i];
            var destParam = destParams[i];
            if (destParam.ParameterType != sourceParam.ParameterType)
                return false;
            if (destParam.Attributes != sourceParam.Attributes)
                return false;
        }

        var sourceKey = GetKey(source);
        var destKey = GetKey(destination);

        if (_replacements.TryGetValue(sourceKey, out var existingReplacement))
        {
            Log.Write(LogLevel.Debug, $"Attempted to replace {sourceKey} (already replaced with {existingReplacement}) with {destKey}");
            return false;
        }
        _replacements[sourceKey] = destKey;

        RuntimeHelpers.PrepareMethod(source.MethodHandle);
        RuntimeHelpers.PrepareMethod(destination.MethodHandle);
        
        try
        {


            if (_is64bit)
            {
                // 64-bit systems use 64-bit absolute address and jumps
                // 12 byte destructive

                // Get function pointers
                long sourceBase = source.MethodHandle.GetFunctionPointer().ToInt64();
                long destinationBase = destination.MethodHandle.GetFunctionPointer().ToInt64();

                // Native source address
                byte* pointerRawSource = (byte*)sourceBase;

                // Pointer to insert jump address into native code
                long* pointerRawAddress = (long*)(pointerRawSource + 0x02);

                // Insert 64-bit absolute jump into native code (address in rax)
                // mov rax, immediate64
                // jmp [rax]
                *(pointerRawSource + 0x00) = 0x48;
                *(pointerRawSource + 0x01) = 0xB8;
                *pointerRawAddress = destinationBase; // ( Pointer_Raw_Source + 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09 )
                *(pointerRawSource + 0x0A) = 0xFF;
                *(pointerRawSource + 0x0B) = 0xE0;
            }
            else
            {
                // 32-bit systems use 32-bit relative offset and jump
                // 5 byte destructive

                // Get function pointers
                int sourceBase = source.MethodHandle.GetFunctionPointer().ToInt32();
                int destinationBase = destination.MethodHandle.GetFunctionPointer().ToInt32();

                // Native source address
                byte* pointerRawSource = (byte*)sourceBase;

                // Pointer to insert jump address into native code
                int* pointerRawAddress = (int*)(pointerRawSource + 1);

                // Jump offset (less instruction size)
                int offset = (destinationBase - sourceBase) - 5;

                // Insert 32-bit relative jump into native code
                *pointerRawSource = 0xE9;
                *pointerRawAddress = offset;
            }

            // done!
            return true;
        }
        catch (Exception ex)
        {
            Log.Write(LogLevel.Error, ex);
            return false;
        }
    }
}