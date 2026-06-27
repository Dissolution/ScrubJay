using System.Reflection;

namespace ScrubJay.Testing.Collections;

partial class TestingData
{
    public static IReadOnlyList<object?> Objects { get; } =
    [
        // null
        (object?)null,
        // valid flags enum
        BindingFlags.Public | BindingFlags.NonPublic,
        // invalid non-flags enum
        (StringComparison)147,
        // primitive
        (byte)147,
        -0.0d,
        // struct
        IntPtr.Zero,
        // Nullable
        (Nullable<int>)null,
        (Nullable<int>)147,
        // char
        '❤',
        "\uD800",
        // string
        (string?)null,
        string.Empty,
        "Sphinx of black quartz, judge my vow!",
        "🏃🏻‍➡️ 🟫 🦊 🦘 🆙 🦥 🐕",
        "pass\0word",
        // delegate
        new Action(static () => { }),
        // object itself
        new object(),
        // type
        typeof(TestingData),
        typeof(IList<>),
        // exception
        new Exception(nameof(TestingData)),
        // old net stuff
        DBNull.Value,
        // anonymous object
        new { Id = 147, Name = "TJ", IsAdmin = true, },
        // array
        new byte[4] { 0, 147, 13, 101 },
        Array.Empty<object?>(),
        // simple dictionary
        new Dictionary<int, string>
        {
            { 1, "one" },
            { 2, "two" },
            { 3, "three" },
        },
        // complex classes
        AppDomain.CurrentDomain,
        Enumerable.Range(0, 13),
        Task.FromException(new InvalidOperationException()),
    ];
}