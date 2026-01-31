# `ScrubJay.Universal`

Provides several utility classes for working with any generic type, even with the `ref struct` anti-constraint.

## `Any`

Normally when you have a `T` instance, where `T : allows ref struct`, you cannot perform common object-related operations:
- `.ToString()`
- `.Equals(object?)`
- `.GetHashCode()`
- `.GetType()`

Nor can you use common comparision utilities:
- `EqualityComparer<T>.Default.Equals(T?,T?)`
- `Comparer<T>.Default.Compare(T?,T?)`

`Any` provides all of those methods (and more) that can take any `T` value, even `ref struct`s.

#### `AnyComparer`

A universal `IEqualityComparer<T>` and `IComparer<T>` that also works with any `T` value.

## `TypeName`

An additional utility class that can convert `Type`s into a much more readable (C#) form.

Example for `typeof(IList<int>)`:

| Method | Result |
| --- | --- |
| `.ToString()` | ``System.Collections.Generic.IList`1[System.Int32]`` |
| `.Name` | ``IList`1`` |
| `.FullName` | ``System.Collections.Generic.IList`1[[System.Int32, System.Private.CoreLib, Version=10.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e]]`` |
| `TypeName.For()` | `IList<int>` |
