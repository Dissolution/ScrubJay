//using System.Globalization;
//
//namespace ScrubJay.Errors.Problems;
//
///// <summary>
/////
///// </summary>
///// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457"/>
//[PublicAPI]
//public static class ProblemDetailsExtensions
//{
//    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457#name-type"/>
//    public const string TYPE_PROPERTY = "type";
//    
//    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457#name-status"/>
//    public const string STATUS_PROPERTY = "status";
//    
//    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457#name-title"/>
//    public const string TITLE_PROPERTY = "title";
//    
//    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457#name-detail"/>
//    public const string DETAIL_PROPERTY = "detail";
//    
//    /// <seealso href="https://datatracker.ietf.org/doc/html/rfc9457#name-instance"/>
//    public const string INSTANCE_PROPERTY = "instance";
//
//    extension(Exception? exception)
//    {
//        /// <summary>
//        /// A URI-like reference that identifies the Problem type.
//        /// </summary>
//        /// <remarks>
//        /// Maps to <c>exception.Data["type"]</c><br/>
//        /// Defaults to a <see href="https://en.wikipedia.org/wiki/Uniform_Resource_Name">URN</see> in the form:<br/>
//        /// <c>urn:dotnet:{FN}</c><br/>
//        /// Where <i>FN</i> is the <paramref name="exception"/>'s <see cref="Type"/>'s FullName with <c>.</c> replaced with <c>:</c>.<br/>
//        /// <i>e.g. </i><c>urn:dotnet:system:argumentnullexception</c>
//        /// </remarks>
//        /// <seealso cref="ExceptionTypeUrn"/>
//        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-type"/>
//        public Uri? Type
//        {
//            get
//            {
//                if (exception is null)
//                    return null;
//                
//                if (exception.Data.TryGetValue(TYPE_PROPERTY, out object? objType))
//                {
//                    if (objType is Uri uri)
//                        return uri;
//                    
//                    if (objType is string urn && Uri.TryCreate(urn, UriKind.RelativeOrAbsolute, out uri!))
//                        return uri;
//                    
//                    // cannot turn this into a uri
//                    return null;
//                }
//              
//                // Use the Exception's Type as a URN
//                return exception.GetTypeURN();
//            }
//            set
//            {
//                if (exception is null)
//                    return;
//                exception.Data.SetOrRemove(TYPE_PROPERTY, value);
//            }
//        }
//        
//        /// <summary>
//        /// An <see cref="int"/> HTTP Status Code for this occurrence of a Problem.
//        /// </summary>
//        /// <remarks>
//        /// Maps to <c>exception.Data["status"]</c>
//        /// </remarks>
//        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-status"/>
//        public int? Status
//        {
//            get
//            {
//                if (exception is null)
//                    return null;
//
//                if (!exception.Data.TryGetValue(STATUS_PROPERTY, out object? objStatus))
//                    return null;
//                
//                int code;
//                if (objStatus is int)
//                {
//                    code = (int)objStatus;
//                }
//                else if (objStatus is IConvertible convertible)
//                {
//                    code = convertible.ToInt32(null);
//                }
//                else
//                {
//                    string? strStatus = objStatus?.ToString();
//                    if (!int.TryParse(strStatus, NumberStyles.Integer, null, out code))
//                    {
//                        return null;
//                    }
//                }
//
//                // We will only return a valid status code
//                if (code >= 100 && code <= 599)
//                    return code;
//                
//                return null;
//            }
//            set
//            {
//                if (exception is null)
//                    return;
//
//                if (value.HasValue)
//                {
//                    int code = value.GetValueOrDefault();
//                    if (code >= 100 && code <= 599)
//                    {
//                        exception.Data.Set(STATUS_PROPERTY, code);
//                        return;
//                    }
//                }
//              
//                exception.Data.Remove(STATUS_PROPERTY);
//            }
//        }
//
//        /// <summary>
//        /// A short, human-readable summary of the Problem.
//        /// </summary>
//        /// <remarks>
//        /// Maps to <c>exception.Data["title"]</c><br/>
//        /// Defaults to the <paramref name="exception"/>'s <see cref="Type"/>'s Name.
//        /// </remarks>
//        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-title"/>
//        public string? Title
//        {
//            get
//            {
//                if (exception is null)
//                    return null;
//                
//                if (exception.Data.TryGetValue(TITLE_PROPERTY, out var objTitle))
//                    return objTitle?.ToString();
//                
//                return exception.GetType().Name;
//            }
//            set
//            {
//                if (exception is null)
//                    return;
//                
//                exception.Data.SetOrRemove(TITLE_PROPERTY, value);
//            }
//        }
//
//        
//        /// <summary>
//        /// A human-readable explanation specific to this occurrence of the Problem.
//        /// </summary>
//        /// <remarks>
//        /// Maps to <c>exception.Data["detail"]</c><br/>
//        /// Defaults to the <paramref name="exception"/>'s Message.
//        /// </remarks>
//        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-detail"/>
//        public string? Detail
//        {
//            get
//            {
//                if (exception is null)
//                    return null;
//                
//                if (exception.Data.TryGetValue(DETAIL_PROPERTY, out var objDetail))
//                    return objDetail?.ToString();
//                
//                return exception.Message;
//            }
//            set
//            {
//                if (exception is null)
//                    return;
//                
//                exception.Data.SetOrRemove(DETAIL_PROPERTY, value);
//            }
//        }
//
//        /// <summary>
//        /// A URI-like reference that identifies the specific occurrence of the Problem.
//        /// </summary>
//        /// <remarks>
//        /// Maps to <c>exception.Data["instance"]</c>
//        /// </remarks>
//        /// <seealso href="https://www.rfc-editor.org/rfc/rfc9457.html#name-instance"/>
//        public string? Instance
//        {
//            get
//            {
//                if (exception is null)
//                    return null;
//                
//                if (exception.Data.TryGetValue(INSTANCE_PROPERTY, out var objInstance))
//                    return objInstance?.ToString();
//                
//                return null;
//            }
//            set
//            {
//                if (exception is null)
//                    return;
//                
//                exception.Data.SetOrRemove(INSTANCE_PROPERTY, value);
//            }
//        }
//    }
//}