using System.Globalization;
using ScrubJay.Text.Extensions;
using ScrubJay.Universal;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

internal static class ProblemDetails
{
    public const string TYPE_PROPERTY = "type";
    public const string STATUS_PROPERTY = "status";
    public const string TITLE_PROPERTY = "title";
    public const string DETAIL_PROPERTY = "detail";
    public const string INSTANCE_PROPERTY = "instance";
}

/// <summary>
/// 
/// </summary>
[PublicAPI]
public static class ProblemDetailsExtensions
{
    extension<E>(E exception)
        where E : Exception, ISJException
    {
        /// <summary>
        /// A URI-like reference that identifies the Problem type.
        /// </summary>
        /// <remarks>
        /// The Type URI is allowed to be a non-resolvable URI, so it is not typed as an <see cref="Uri"/>.
        /// </remarks>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-type"/>
        public string? Type
        {
            get
            {
                if (exception.Data.TryGetValue<string, object?>(ProblemDetails.TYPE_PROPERTY, out object? type))
                    return type?.ToString();
                return null; // about:blank
            }
            set
            {
                if (value is null)
                {
                    exception.Data.TryRemove<string>(ProblemDetails.TYPE_PROPERTY);
                }
                else
                {
                    exception.Data.Set<string, string>(ProblemDetails.TYPE_PROPERTY, value);
                }
            }
        }

        /// <summary>
        /// An <see cref="int"/> HTTP Status Code for this occurrence of a Problem.
        /// </summary>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-status"/>
        public int? Status
        {
            get
            {
                if (exception.Data.TryGetValue<string, object?>(ProblemDetails.STATUS_PROPERTY, out object? status))
                {
                    if (status is int statusCode)
                        return statusCode;
                    string? str = status?.ToString();
                    if (int.TryParse(str, NumberStyles.Integer, null, out statusCode))
                        return statusCode;
                    // could not interpret
                }
                return null; // about:blank
            }
            set
            {
                if (value.TryGetValue(out var httpStatusCode))
                {
                    exception.Data.Set<string, int>(ProblemDetails.STATUS_PROPERTY, httpStatusCode);
                }
                else
                {
                    exception.Data.TryRemove<string>(ProblemDetails.STATUS_PROPERTY);
                }
            }
        }
        
        /// <summary>
        /// A short, human-readable summary of the Problem.
        /// </summary>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-title"/>
        public string? Title
        {
            get
            {
                if (exception.Data.TryGetValue<string, string?>(ProblemDetails.TITLE_PROPERTY, out string? title))
                    return title;
                return null;
            }
            set
            {
                if (value is null)
                {
                    exception.Data.TryRemove<string>(ProblemDetails.TITLE_PROPERTY);
                }
                else
                {
                    exception.Data.Set<string, string>(ProblemDetails.TITLE_PROPERTY, value);
                }
            }
        }
        
        /// <summary>
        /// A human-readable explanation specific to this occurrence of the Problem.
        /// </summary>
        /// <remarks>
        /// If "details" is not present in <see cref="Exception.Data"/>, the <see cref="Exception.Message"/> will be returned.
        /// </remarks>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-detail"/>
        public string? Detail
        {
            get
            {
                if (exception.Data.TryGetValue<string, string?>(ProblemDetails.DETAIL_PROPERTY, out string? detail) && !string.IsNullOrEmpty(detail))
                    return detail;
                return exception.Message;
            }
            set
            {
                if (value is null)
                {
                    exception.Data.TryRemove<string>(ProblemDetails.DETAIL_PROPERTY);
                }
                else
                {
                    exception.Data.Set<string, string>(ProblemDetails.DETAIL_PROPERTY, value);
                }
            }
        }
        
        /// <summary>
        /// A URI-like reference that identifies the specific occurrence of the Problem.
        /// </summary>
        /// <remarks>
        /// If "instance' is not present in <see cref="Exception.Data"/>, the <see cref="Exception"/>'s <see cref="Type"/> will be returned.
        /// </remarks>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-instance"/>
        public string? Instance
        {
            get
            {
                if (exception.Data.TryGetValue<string, string?>(ProblemDetails.INSTANCE_PROPERTY, out string? instance) && !string.IsNullOrEmpty(instance))
                    return instance;
                return Type.Render<E>(in exception);
            }
            set
            {
                if (value is null)
                {
                    exception.Data.TryRemove<string>(ProblemDetails.INSTANCE_PROPERTY);
                }
                else
                {
                    exception.Data.Set<string, string>(ProblemDetails.INSTANCE_PROPERTY, value);
                }
            }
        }
    }
}