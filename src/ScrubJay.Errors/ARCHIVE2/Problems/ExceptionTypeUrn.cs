//namespace ScrubJay.Errors.Problems;
//
//[PublicAPI]
//public static class ExceptionTypeUrn
//{
//    extension(Exception? exception)
//    {
//        [return: NotNullIfNotNull(nameof(exception))]
//        public Uri? GetTypeURN()
//        {
//            if (exception is null)
//                return null;
//
//            var exceptionType = exception.GetType();
//            var fullname = (exceptionType.FullName ?? exceptionType.Name);
//            fullname = fullname.Replace('.', ':').ToLowerInvariant();
//
//            string urn = $"urn:dotnet:{fullname}";
//            return new Uri(urn, UriKind.Absolute);
//        }
//    }
//
//    public static bool TryParseExceptionType(
//        [AllowNull, NotNullWhen(true)] this Uri? uri,
//        [NotNullWhen(true)] out Type? exceptionType)
//    {
//        if (uri is not null && string.Equals(uri.Scheme, "urn", StringComparison.OrdinalIgnoreCase))
//        {
//            var path = uri.AbsolutePath;
//            if (path.StartsWith("dotnet:", StringComparison.OrdinalIgnoreCase))
//            {
//                string fullName = path.Substring(7).Replace(':', '.');
//                Type? type = Type.GetType(fullName, false);
//                if (type is not null && typeof(Exception).IsAssignableFrom(type))
//                {
//                    exceptionType = type;
//                    return true;
//                }
//            }
//        }
//
//        exceptionType = null;
//        return false;
//    }
//}