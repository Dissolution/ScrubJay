using System.Reflection;
using ScrubJay.Universal;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static class ExceptionExtensions
{
      private static void WriteParameterTo(ParameterInfo parameter, ref InterpolatedText text)
        {
            text.RenderType(parameter.ParameterType);
            text.Append(' ');
            text.Append(parameter.Name);
            if (parameter.HasDefaultValue)
            {
                text.Append(" = ");
                text.Append(parameter.DefaultValue);
            }
        }

        private static void WriteMethodTo(MethodBase method, ref InterpolatedText text)
        {
            Type returnType;
            if (method is MethodInfo methodInfo)
            {
                returnType = methodInfo.ReturnType;
            }
            else if (method is ConstructorInfo constructorInfo)
            {
                if (constructorInfo.IsStatic)
                {
                    returnType = typeof(void);
                }
                else
                {
                    returnType = constructorInfo.DeclaringType!;
                }
            }
            else
            {
                throw new InvalidOperationException();
            }

            text.RenderType(returnType);
            text.Append(' ');
            text.Append(method.Name);
            var genericTypes = method.GetGenericArguments();
            if (genericTypes.Length > 0)
            {
                text.Append('>');
                text.RenderType(genericTypes[0]);
                for (var i = 1; i < genericTypes.Length; i++)
                {
                    text.Append(", ");
                    text.RenderType(genericTypes[i]);
                }
                text.Append('>');
            }
            text.Append('(');
            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                WriteParameterTo(parameters[0], ref text);
                for (var i = 1; i < parameters.Length; i++)
                {
                    text.Append(", ");
                    WriteParameterTo(parameters[i], ref text);
                }
            }
            text.Append(')');
        }
    
    extension<E>(E exception)
        where E : Exception
    {
        /// <summary>
        /// Gets a <see cref="Uri"/> that points to the <see href="https://www.hresult.info"/> search for this Exception's <see cref="HResult"/>.
        /// </summary>
        public Uri HResultInfo => new($"https://www.hresult.info/Search?q=0x{exception.HResult:X8}");

        public void WriteDebugInformationTo(ref InterpolatedText text, int offset = 0)
        {
            string? source = exception.Source;
            MethodBase? targetSite = exception.TargetSite;
            string? stackTrace = exception.StackTrace;

            if (source is not null || targetSite is not null)
            {
                text.Append(Environment.NewLine);
                text.AppendRepeat(offset, "  ");
                text.Append("Target: ");
                if (source is not null)
                {
                    text.Append(source);
                    if (targetSite is not null)
                        text.Append('.');
                }
                if (targetSite is not null)
                {
                    WriteMethodTo(targetSite, ref text);
                }
            }

            if (stackTrace is not null)
            {
                text.Append(Environment.NewLine);
                text.AppendRepeat(offset, "  ");
                text.Append($"StackTrace: {stackTrace}");
            }
        }

        public void WriteOptionalPropertiesTo(ref InterpolatedText text, int offset = 0)
        {
            // HResult
            var hResult = (HResult)exception.HResult;
            text.Append(Environment.NewLine);
            text.AppendRepeat(offset, "  ");
            text.Append($"HResult: {hResult:X} ({(hResult.IsSuccess ? "Success" : "Failure")})");

            // HelpLink
            if (!string.IsNullOrEmpty(exception.HelpLink))
            {
                text.Append(Environment.NewLine);
                text.AppendRepeat(offset, "  ");
                text.Append($"HelpLink: {exception.HelpLink}");
            }

            // Data
            if (exception.Data.Count > 0)
            {
                text.Append(Environment.NewLine);
                text.AppendRepeat(offset, "  ");
                text.Append("Data:");
                offset++;
                foreach (DictionaryEntry entry in exception.Data)
                {
                    text.Append(Environment.NewLine);
                    text.AppendRepeat(offset, "  ");
                    text.Append(entry.Key);
                    text.Append(": ");
                    text.Append(entry.Value);
                }
                offset--;
            }

            // Inner Exception(s)
            if (exception.InnerException is not null)
            {
                text.Append(Environment.NewLine);
                text.AppendRepeat(offset, "  ");
                text.Append("Inner:");
                offset++;
                exception.InnerException.WriteTo(ref text, offset);
                offset--;
            }
        }


        public void WriteTo(ref InterpolatedText text, int offset = 0)
        {
            text.RenderType(typeof(E));
            text.Append(':');
            offset++;

            var message = exception.Message;
            if (!string.IsNullOrEmpty(message))
            {
                text.AppendLine();
                text.AppendRepeat(offset, "  ");
                text.Append($"Message: {message}");
            }
            
            exception.WriteDebugInformationTo(ref text, offset);
            exception.WriteOptionalPropertiesTo(ref text, offset);
            offset--;
        }
    }
}