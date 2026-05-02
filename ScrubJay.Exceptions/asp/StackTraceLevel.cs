namespace ScrubJay.Exceptions.Asp;

[PublicAPI]
public enum StackTraceLevel
{
    /// <summary>
    /// Do not show a stack trace
    /// </summary>
    None,
    
    /// <summary>
    /// Show a sanitized version of the stack trace
    /// </summary>
    FilesAndMethods,
    
    /// <summary>
    /// Show the full stack trace
    /// </summary>
    Full,
}