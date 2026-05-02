namespace ScrubJay.Exceptions.Asp;

[PublicAPI]
public enum StackTraceLevel
{
    /// <summary>
    /// Do not show a stack trace.
    /// </summary>
    None,
    
    Sanitized,
    
    /// <summary>
    /// Show the files and methods of the stack trace.
    /// </summary>
    FilesAndMethods,
    
    /// <summary>
    /// Show the full captured stack trace.
    /// </summary>
    Full,
}