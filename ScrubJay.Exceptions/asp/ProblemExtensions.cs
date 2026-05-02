using Microsoft.AspNetCore.Mvc;

namespace ScrubJay.Exceptions.Asp;

[PublicAPI]
public static class ProblemExtensions
{
    private static IDictionary<string, object?> DataToExtensions(IDictionary data)
    {
        var dict = new Dictionary<string, object?>(capacity: data.Count, StringComparer.Ordinal);
        
        foreach (DictionaryEntry entry in data)
        {
            string key = entry.Key!.ToString()!;
            if (key is ProblemDetailsExtensions.TYPE_PROPERTY 
                or ProblemDetailsExtensions.TITLE_PROPERTY 
                or ProblemDetailsExtensions.STATUS_PROPERTY 
                or ProblemDetailsExtensions.DETAIL_PROPERTY 
                or ProblemDetailsExtensions.INSTANCE_PROPERTY)
            {
                continue;
            }
            dict[key] = entry.Value;
        }

        return dict;
    }
    
    
    extension<E>(E exception)
        where E : Exception, ISJException
    {
        public ProblemDetails ToProblemDetails()
        {
            return new ProblemDetails()
            {
                Type = exception.Type,
                Title = exception.Title,
                Status = exception.Status,
                Detail = exception.Detail,
                Instance = exception.Instance,
                Extensions = DataToExtensions(exception.Data),
            };
        }
    }
}