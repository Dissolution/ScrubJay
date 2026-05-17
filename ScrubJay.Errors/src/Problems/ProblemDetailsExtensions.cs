using System.Globalization;
using ScrubJay.Universal.Extensions;
using Any = ScrubJay.Universal.Any;

namespace ScrubJay.Errors.Problems;

/// <summary>
///
/// </summary>
[PublicAPI]
public static class ProblemDetailsExtensions
{
    public const string TYPE_PROPERTY = "type";
    public const string TITLE_PROPERTY = "title";
    public const string STATUS_PROPERTY = "status";
    public const string DETAIL_PROPERTY = "detail";
    public const string INSTANCE_PROPERTY = "instance";

    extension<E>(E exception)
        where E : Exception
    {
        /// <summary>
        /// A URI-like reference that identifies the Problem type.
        /// </summary>
        /// <remarks>
        /// The Type URI is allowed to be a non-resolvable URI, so it is not typed as an <see cref="Uri"/>.<br/>
        /// If "type" is not present in the <paramref name="exception"/>'s <see cref="Exception.Data"/>,
        /// a urn for the <see cref="Exception"/>'s <see cref="Type"/> will be returned.
        /// </remarks>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-type"/>
        public string? Type
        {
            get
            {
                if (exception.Data.TryGetValue(TYPE_PROPERTY, out object? type))
                    return type?.ToString();
                // Use the Exception's Type
                var exType = Any.GetType<E>(in exception);
                return $"urn:{exType.Namespace}:{Type.Render(exType)}";
            }
            set => exception.Data.SetOrRemove(TYPE_PROPERTY, value);
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
            set => exception.Data.SetOrRemove(TITLE_PROPERTY, value);
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
                    int code;
                    if (status is int)
                    {
                        code = (int)status;
                    }
                    else
                    {
                        string? str = status?.ToString();
                        if (!int.TryParse(str, NumberStyles.Integer, null, out code))
                        {
                            return null;
                        }
                    }

                    // We will only return a valid status code
                    if (code >= 100 && code <= 599)
                        return code;
                }
                return null;
            }
            set
            {
                if (value.TryGetValue(out var code) && code >= 100 && code <= 599)
                {
                    exception.Data[STATUS_PROPERTY] = code;
                }
                else
                {
                    exception.Data.Remove(STATUS_PROPERTY);
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
            set => exception.Data.SetOrRemove(DETAIL_PROPERTY, value);
        }

        /// <summary>
        /// A URI-like reference that identifies the specific occurrence of the Problem.
        /// </summary>
        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-instance"/>
        public string? Instance
        {
            get
            {
                if (exception.Data.TryGetValue(INSTANCE_PROPERTY, out var instanceObj))
                {
                    return instanceObj?.ToString();
                }
                return null;
            }
            set => exception.Data.SetOrRemove(INSTANCE_PROPERTY, value);
        }
    }
}