



---
## `EnumMemberInfo`

### `Attributes`

Certain `Attributes` applied to an enum member value will be used to assist with Parsing and Formatting:
  - `[System.ComponentModel.DataAnnotations.DisplayAttribute]`
    - `.Name` will be aliased as `Display.Name`
    - `.Description` will be aliased as `Display.Description`
    - `.ShortName` will be aliased as `Display.ShortName`
    - and an alias of `Display` will be the first non-`null` of `.Name`, `.Description`, and then `.ShortName`
  - `[System.ComponentModel.DescriptionAttribute]`
    - `.Description` will be aliased as `Description`
  - `[System.Runtime.Serialization.EnumMemberAttribute]`
    - `.Value` will be aliased as `EnumMember`
  - `[System.Runtime.Serialization.DataMemberAttribute]`
    - `.Name` will be aliased as `DataMember`

To return that specific value, use the alias as the `format` in `enum.Format(format)
To use any of the values for parsing, use the default `includeAttributes = true`