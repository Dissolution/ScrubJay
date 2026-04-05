using System.ComponentModel;

namespace ScrubJay.Functional;

/// <summary>
/// An <see cref="Exception"/> that contains <see href="https://www.rfc-editor.org/rfc/rfc9457">Problem Details</see>-like information.
/// </summary>
/// <remarks>
/// <see cref="ProblemException"/> supports
/// <see href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#collection-initializers">collection initialization syntax</see>
/// as a passthrough to its <see cref="Data"/> property.
/// </remarks>
/// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457"/><br/>
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/classes-and-structs/object-and-collection-initializers#collection-initializers"/><br/>
[PublicAPI]
public sealed class ProblemException : Exception, IEnumerable<KeyValuePair<string, object?>>
{
    /// <summary>
    /// A short, human-readable summary of the problem type.
    /// </summary>
    public string? Title { get; init; } = null;
    
    /// <summary>
    /// A human-readable explanation specific to this occurrence of the problem.
    /// </summary>
    public string? Details { get; init; } = null;
    
    /// <summary>
    /// Gets information about the Caller's File Path, Line Number, and Member Name.
    /// </summary>
    public new string? Source
    {
        get => base.Source;
    }

    /// <summary>
    /// Gets a collection of <see cref="DictionaryEntry">entries</see> that provide additional information about the <see cref="ProblemException"/>.
    /// </summary>
    /// <remarks>
    /// This is intended to be used as an <see cref="IDictionary{string,object}"/>.
    /// </remarks>
    public override IDictionary Data
    {
        get
        {
            return base.Data;
        }
    }

    [UnsafeAccessor(UnsafeAccessorKind.Field, Name = "_innerException")]
    private static extern ref Exception? GetInnerException(Exception exception);
    
    public new Exception? InnerException
    {
        get => base.InnerException;
        init => GetInnerException(this) = value;
    }
      
    public override string Message
    {
        get
        {
            if (Title is not null)
            {
                if (Details is not null)
                {
                    return $"{Title}: {Details}";
                }
                else
                {
                    return Title;
                }
            }
            else
            {
                if (Details is not null)
                {
                    return Details;
                }
                else
                {
                    return nameof(ProblemException);
                }
            }
        }
    }

    

    private static string GetSource(string? callerFilePath, int? callerLineNumber, string? callerMemberName)
    {
        callerFilePath ??= "????.??";
        string lineNum = callerLineNumber.TryGetValue(out int n) ? n.ToString() : "???";
        callerMemberName ??= "??";
        return $"{callerFilePath}:{lineNum}  {callerMemberName}";
    }

    public ProblemException(
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
        : base()
    {
        base.Source = GetSource(filePath, lineNumber, memberName);
    }
    
    public ProblemException(
        Exception? innerException,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
        : base(null, innerException)
    {
        base.Source = GetSource(filePath, lineNumber, memberName);
    }

    public ProblemException(
        string? details, 
        string? title = null, 
        Exception? innerException = null,
        [CallerFilePath] string? filePath = null,
        [CallerLineNumber] int? lineNumber = null,
        [CallerMemberName] string? memberName = null)
        : base(details, innerException)
    {
        base.Source = GetSource(filePath, lineNumber, memberName);
        this.Details = details ?? innerException?.Message;
        this.Title = title ?? innerException?.GetType().Name;
        if (innerException is not null && innerException.Data.Count > 0)
        {
            foreach (DictionaryEntry entry in innerException.Data)
            {
                this.Data[entry.Key.ToString()!] = entry.Value;
            }
        }
    }
    
    public void Add(KeyValuePair<string, object?> pair)
    {
        this.Data[pair.Key] = pair.Value;
    }
    
    public void Add((string Key, object? Value) tuple)
    {
        this.Data[tuple.Key] = tuple.Value;
    }
    
    public void Add(string key, object? value)
    {
        this.Data[key] = value;
    }

    IEnumerator IEnumerable.GetEnumerator() => Data.GetEnumerator();

    public IEnumerator<KeyValuePair<string, object?>> GetEnumerator()
    {
        foreach (DictionaryEntry entry in this.Data)
        {
            string key = entry.Key.ToString()!;
            object? value = entry.Value;
            yield return new(key, value);
        }
    }

    public override string ToString()
    {
        StringBuilder builder = new StringBuilder();

        builder.Append("ProblemException:");
        
        if (!string.IsNullOrEmpty(Title))
        {
            builder.AppendLine()
                .Append($"  Title: {Title}");
        }
        
        if (!string.IsNullOrEmpty(Details))
        {
            builder.AppendLine()
                .Append($"  Details: {Details}");
        }

        if (Data.Count > 0)
        {
            builder.AppendLine()
                .Append("  Data:");
            
            foreach (DictionaryEntry entry in Data)
            {
                builder.AppendLine()
                    .Append($"    {entry.Key}: {entry.Value}");
            }
        }

        if (this.InnerException is not null)
        {
            builder
                .AppendLine()
                .Append($"  Inner {this.InnerException.GetType().Name}: {this.InnerException.Message}");
        }

        return builder.ToString();
    }
}