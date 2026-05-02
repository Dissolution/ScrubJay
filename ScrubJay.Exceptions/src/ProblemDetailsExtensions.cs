using System.Globalization;
using ScrubJay.Text.Extensions;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

/// <summary>
/// 
/// </summary>
[PublicAPI]
public static class ProblemDetailsExtensions
{
    public const string TYPE_PROPERTY = "type";
    public const string STATUS_PROPERTY = "status";
    public const string TITLE_PROPERTY = "title";
    public const string DETAIL_PROPERTY = "detail";
    public const string INSTANCE_PROPERTY = "instance";
    
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
                if (exception.Data.TryGetValue(TYPE_PROPERTY, out object? type))
                    return type?.ToString();
                return null; // about:blank
            }
            set
            {
                if (value is not null)
                {
                    exception.Data[TYPE_PROPERTY] = value;
                }
                else
                {
                    exception.Data.Remove(TYPE_PROPERTY);
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
                if (exception.Data.TryGetValue(STATUS_PROPERTY, out object? status))
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
                    exception.Data[STATUS_PROPERTY] = httpStatusCode;
                }
                else
                {
                    exception.Data.Remove(STATUS_PROPERTY);
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
                if (exception.Data.TryGetValue(TITLE_PROPERTY, out var title))
                    return title?.ToString();
                return null;
            }
            set
            {
                if (value is not null)
                {
                    exception.Data[TITLE_PROPERTY] = value;
                }
                else
                {
                    exception.Data.Remove(TITLE_PROPERTY);
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
                if (exception.Data.TryGetValue(DETAIL_PROPERTY, out var detailObj))
                {
                    string? detail = detailObj?.ToString();
                    if (!string.IsNullOrEmpty(detail))
                        return detail;
                }
                return exception.Message;
            }
            set
            {
                if (value is not null)
                {
                    exception.Data[DETAIL_PROPERTY] = value;
                }
                else
                {
                    exception.Data.Remove(DETAIL_PROPERTY);
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
                if (exception.Data.TryGetValue(INSTANCE_PROPERTY, out var instanceObj))
                {
                    string? instance = instanceObj?.ToString();
                    if (!string.IsNullOrEmpty(instance))
                        return instance;
                }
                return Type.Render<E>(in exception);
            }
            set
            {
                if (value is not null)
                {
                    exception.Data[INSTANCE_PROPERTY] = value;
                }
                else
                {
                    exception.Data.Remove(INSTANCE_PROPERTY);
                }
            }
        }
    }
}