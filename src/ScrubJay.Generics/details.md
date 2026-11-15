## Boxing, Unboxing, and ref-structs.

There is a large issue when it comes to trying to support boxing operations upon `T : allows ref struct` instances.
The C# compiler treats those instances as radioactive and it is difficult to escape that constraint.

Ideally, we want to support this method:
```csharp
public static bool TryBox<T>(T? instance, out object? boxed)
    where T : allows ref struct
{
    if (typeof(T).IsByRefLike)
    {
        boxed = null;
        return false;
    }
    
    // boxing code here
}
```

Attempting to do so has many failures:

---
#### Fail #1

Trying to use the same exact code as the non-`allows ref struct` `TryBox` method won't even compile:

```csharp
boxed = (object?)value;
return true;
```

> Cannot cast expression of type 'T?' to type 'object?'.

---
#### Fail #2

Trying to use `InlineIL.Emit` to emit that code directly throws _runtime_ errors only when a `ref struct` is used as a `T`:

```csharp
Emit.Ldarg(nameof(boxed));
Emit.Ldarg(nameof(value));
Emit.Box(typeof(T));
Emit.Stind_Ref();
Emit.Ldc_I4_1();
Emit.Ret();
throw Unreachable();
```

> System.InvalidProgramException: Common Language Runtime detected an invalid program.

---
#### Fail #3

Trying to `Emit` the code to call the other `TryBox` method directly has an even worse runtime error:
     
```csharp
 Emit.Ldarg(nameof(value));
 Emit.Ldarg(nameof(boxed));
 Emit.Call(MethodRef.Method(typeof(Any), nameof(TryBox), [typeof(T), typeof(object).MakeByRefType()]).MakeGenericMethod(typeof(T)));
 Emit.Ret();
 throw Unreachable();
```

> System.Security.VerificationException: type argument 'XXX' violates the constraint of type parameter 'T'.

---

### Solution

After a bit of tinkering and reading up on how the compiler works, I found a hack that seems to work.
`Emit` can still be used, but we need another shim method in between that 'strips' off the `allows ref struct` constraint never having the compiler attempt to compile its Emit with a ref struct T.
  
