# `ScrubJay.Universal`

Provides utility classes for working with _Any_ generic value, even if the type has the `allows ref struct` anti-constraint.

---

## `allows ref struct`

The `T : allows ref struct` anti-constraint and the ability for `ref struct`s to implement interfaces (both added in C#13)
are extremely useful tools for writing performant code.  

### Issues

As an _anti_-constraint, this causes `T` to no longer inherit from `object`.

- All the methods declared on `object` are unavailable.
```csharp
value.ToString();
value.Equals((object?)XXX);
value.GetHashCode();
value.GetType();
// fail to compile: `Cannot implicitly convert type 'T' to 'object'`
```

- Common utilities are unavailable.
```csharp
EqualityComparer<T>.Default.Equals(T?,T?);
Comparer<T>.Default.Compare(T?,T?);
// fail to compile with `The type 'T' may not be a ref struct or a type parameter allowing ref structs in order to use it as a type argument for type parameter 'T'`
```

### The Fix: `Any`

`Any` is a utility class that can access those and other methods on any generic value, regardless of constraints.

- `string Any.ToString<T>(in T? value)`
- `bool Any.Equals<T>(in T? value, object? other)`
- `int Any.GetHashCode<T>(in T? value)`
- `Type Any.GetType<T>(in T? value)`
- `bool Any.Equals<T>(in T? value, in T? other)` -- like `IEquatable<T>`
- `int Any.Compare<T>(in T? value, in T? other)` -- like `IComparable<T>`
- `string Any.Format<T>(in T? value, string? format, IFormatProvider? provider)` -- like `IFormattable`
- `bool Any.TryFormat<T>(in T? value, Span<char> destination, out int charsWritten, scoped ReadOnlySpan<char> format, IFormatProvider? provider)` -- like `ISpanFormattable`
- `bool Any.TryParse<T>(scoped ReadOnlySpan<char> text, IFormatProvider? provider, out T value)` -- like `ISpanParsable<T>`
- `bool Any.TryParse<T>(string? str, IFormatProvider? provider, out T value)` -- like `IParsable<T>`

### `UniversalComparer`

This is an `IEqualityComparer<T>` and `IComparer<T>` that works like `EqualityComparer<T>` and `Comparer<T>` but also with any `T`.

---

## Links
- `allows ref strct`
  - https://learn.microsoft.com/en-us/dotnet/csharp/whats-new/csharp-13#allows-ref-struct
  - https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/generics/constraints-on-type-parameters#allows-ref-struct
  - https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/keywords/where-generic-type-constraint