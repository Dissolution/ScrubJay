#pragma warning disable SYSLIB1054

using BenchmarkDotNet.Engines;
using ScrubJay.Text.Utilities;

namespace ScrubJay.Text.Benchmarks.CopyTo;

public partial class StringCopyToCharArrayBenchmarks
{
    // ReSharper disable once FieldCanBeMadeReadOnly.Local
    private Consumer _consumer = new();

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void CopyToCharArray(string source, char[] destination)
    {
        source.CopyTo(0, destination, 0, source.Length);
        _consumer.Consume(destination);
    }

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void AsSpanCopyToAsSpan(string source, char[] destination)
    {
        source.AsSpan().CopyTo(destination);
        _consumer.Consume(destination);
    }

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void Emit_Cpblk(string source, char[] destination)
    {
        TextHelper.Notsafe.CopyText(source, destination, source.Length);
        _consumer.Consume(destination);
    }

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void Buffer_MemoryCopy(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                Buffer.MemoryCopy(sourcePtr, destPtr, destination.Length * sizeof(char), source.Length * sizeof(char));
            }
        }
        _consumer.Consume(destination);
    }

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void Unsafe_CopyBlock(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                Unsafe.CopyBlock(destPtr, sourcePtr, (uint)(source.Length * sizeof(char)));
            }
        }
        _consumer.Consume(destination);
    }

    /*
    [DllImport("msvcrt.dll", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    private static unsafe extern void* dllimport_memcpy(void* dest, void* src, nuint count);


    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void DllImport_Memcpy(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                dllimport_memcpy(destPtr, sourcePtr, (nuint)(source.Length * sizeof(char)));
            }
        }
        _consumer.Consume(destination);
    }


    [DllImport("msvcrt.dll", EntryPoint = "memmove", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    private static unsafe extern void* dllimport_memmove(void* dest, void* src, nuint count);

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void DllImport_Memmove(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                dllimport_memmove(destPtr, sourcePtr, (nuint)(source.Length * sizeof(char)));
            }
        }
        _consumer.Consume(destination);
    }

#if NET7_0_OR_GREATER

    [LibraryImport("msvcrt.dll", EntryPoint = "memcpy", SetLastError = false)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    private static unsafe partial void* libraryimport_memcpy(void* dest, void* src, nuint count);

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void LibraryImport_Memcpy(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                libraryimport_memcpy(destPtr, sourcePtr, (nuint)(source.Length * sizeof(char)));
            }
        }
        _consumer.Consume(destination);
    }

    [LibraryImport("msvcrt.dll", EntryPoint = "memmove", SetLastError = false)]
    [UnmanagedCallConv(CallConvs = [typeof(CallConvCdecl)])]
    [DefaultDllImportSearchPaths(DllImportSearchPath.SafeDirectories)]
    private static unsafe partial void* libraryimport_memmove(void* dest, void* src, nuint count);

    [Benchmark]
    [ArgumentsSource(typeof(TestValues), nameof(TestValues.StringsWithArrays))]
    public void LibraryImport_Memmove(string source, char[] destination)
    {
        unsafe
        {
            fixed (char* sourcePtr = source)
            fixed (char* destPtr = destination)
            {
                libraryimport_memmove(destPtr, sourcePtr, (nuint)(source.Length * sizeof(char)));
            }
        }
        _consumer.Consume(destination);
    }
#endif
*/
}