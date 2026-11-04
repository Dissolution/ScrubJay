Here's the full set with the trimming pieces removed and the property expression-bodied. I haven't built or packed this either.

**1. In your package, `buildTransitive\YourLibrary.targets`:**

```xml
<Project>
  <!-- Default to throwing in Debug unless the consumer set the property explicitly. -->
  <PropertyGroup>
    <YourLibraryRefStructHashThrows Condition="'$(YourLibraryRefStructHashThrows)' == '' and '$(Configuration)' == 'Debug'">true</YourLibraryRefStructHashThrows>
  </PropertyGroup>

  <ItemGroup>
    <RuntimeHostConfigurationOption Include="YourLibrary.RefStructHashThrows"
                                    Value="$(YourLibraryRefStructHashThrows)"
                                    Condition="'$(YourLibraryRefStructHashThrows)' != ''" />
  </ItemGroup>
</Project>
```

To ship it, add this to your library's `.csproj`:

```xml
<None Include="buildTransitive\YourLibrary.targets" Pack="true" PackagePath="buildTransitive\" />
```

**2. In your library, the switch:**

```csharp
internal static class LibrarySwitches
{
    internal static bool RefStructHashThrows =>
        AppContext.TryGetSwitch("YourLibrary.RefStructHashThrows", out bool enabled) && enabled;
}
```

Use it the same way at delegate creation:

```csharp
if (type.IsByRefLike)
{
    if (LibrarySwitches.RefStructHashThrows)
    {
        return static (in _) => throw new NotSupportedException("ref struct instances have no hash code.");
    }

    return static (in _) => 0;
}
```

**3. In the consumer's `.csproj`, only to override the default:**

```xml
<YourLibraryRefStructHashThrows>false</YourLibraryRefStructHashThrows>
```

When the switch is unset, `TryGetSwitch` returns false and the expression yields `false`, so a consumer who sets nothing gets the documented default: return `0`. The one exception is a Debug build, where your `.targets` file sets the switch to `true`.