
## `ArgumentException`

When getting `.Message`, it overrides whatever is passed in as such:
```csharp
// code abbreviated for clarity
public override string Message
{
    get
    {
        string s = base.Message;
        if (!string.IsNullOrEmpty(_paramName))
        {
            s += " " + SR.Format(SR.Arg_ParamName_Name, _paramName);
        }

        return s;
    }
}
```

I want to provide a better message for arguments, hence the **Enhanced** Exceptions