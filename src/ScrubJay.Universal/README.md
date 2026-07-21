# ScrubJay.Universal





## `object` Methods

Per [System.Object](https://learn.microsoft.com/en-us/dotnet/api/system.object):

> The [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-10.0) class is the ultimate base class of all .NET classes; it is the root of the type hierarchy.
>
> Because all classes in .NET are derived from [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-10.0), every method defined in the [Object](https://learn.microsoft.com/en-us/dotnet/api/system.object?view=net-10.0) class is available in all objects in the system.

That is true for all `struct` and `class` instances, but **false** for [ref struct](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/builtin-types/ref-struct) instances. (Net9.0+ / C#13+ feature).

---

When using a generic type with the `allows ref struct` anti-constraint, you are no longer able to access the root [object methods](https://learn.microsoft.com/en-us/dotnet/api/system.object#methods) on instances of that type:

```csharp
public static void ARSTest<T>(T? value)
	where T : allows ref struct
{
	bool equals = value.Equals((object?)"TRJ");
    int hashCode = value.GetHashCode();
    Type type = ((object?)value).GetType();
	string str = ((object?)value).ToString();
}
```

> [!CAUTION]
>
> The first two methods will fail to compile:
>
> > Error [CS0029](https://learn.microsoft.com/en-us/dotnet/csharp/language-reference/compiler-messages/cs0029) : Cannot implicitly convert type 'T' to 'object'`
>
> The second two methods will also fail to compile:
>
> > Error [CS0030](https://learn.microsoft.com/en-us/dotnet/csharp/misc/cs0030) : Cannot convert type 'T' to 'object'

---

### `ScrubJay.Universal.Any`

This utility class lets you call `.Equals(object?) -> bool`, `.GetHashCode() -> int`, `.GetType() -> Type`, and `.ToString() -> string` on _any_ value, including generic values with the `allows ref struct` anti-constraint.