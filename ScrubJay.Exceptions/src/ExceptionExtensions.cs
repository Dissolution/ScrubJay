using System.Reflection;
using ScrubJay.Universal;
using ScrubJay.Universal.Extensions;

namespace ScrubJay.Exceptions;

[PublicAPI]
public static class ExceptionExtensions
{
    extension(Exception exception)
    {
        public Uri HResultInfo
        {
            get
            {
                return new Uri($"https://www.hresult.info/Search?q=0x{exception.HResult:X8}");
            }
        }

        internal void WriteOptionalPropertiesTo(ref InterpolatedText text)
        {
            var k = new
            {
                exception.Data,
                exception.HResult,
                exception.HelpLink,
                exception.InnerException,
            };
            
            Debugger.Break();
        }

        private static void WriteParameterTo(ParameterInfo parameter, ref InterpolatedText text)
        {
            text.RenderType(parameter.ParameterType);
            text.AppendLiteral(' ');
            text.AppendFormatted(parameter.Name);
            if (parameter.HasDefaultValue)
            {
                text.AppendLiteral(" = ");
                text.AppendFormatted(parameter.DefaultValue);
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
            text.AppendLiteral(' ');
            text.AppendLiteral(method.Name);
            var genericTypes = method.GetGenericArguments();
            if (genericTypes.Length > 0)
            {
                text.AppendLiteral('>');
                text.RenderType(genericTypes[0]);
                for (var i = 1; i < genericTypes.Length; i++)
                {
                    text.AppendLiteral(", ");
                    text.RenderType(genericTypes[i]);
                }
                text.AppendLiteral('>');
            }
            text.AppendLiteral('(');
            var parameters = method.GetParameters();
            if (parameters.Length > 0)
            {
                WriteParameterTo(parameters[0], ref text);
                for (var i = 1; i < parameters.Length; i++)
                {
                    text.AppendLiteral(", ");
                    WriteParameterTo(parameters[i], ref text);
                }
            }
            text.AppendLiteral(')');
        }

        public void WriteDebugInformationTo(ref InterpolatedText text)
        {
            string? source = exception.Source;
            MethodBase? targetSite = exception.TargetSite;
            string? stackTrace = exception.StackTrace;

            if (source is not null || targetSite is not null)
            {
                text.AppendLiteral(Environment.NewLine);
                text.AppendLiteral("  Target: ");
                if (source is not null)
                {
                    text.AppendLiteral(source);
                    if (targetSite is not null)
                        text.AppendLiteral('.');
                }
                if (targetSite is not null)
                {
                    WriteMethodTo(targetSite, ref text);
                }
            }

            if (stackTrace is not null)
            {
                text.AppendLiteral(Environment.NewLine);
                text.AppendLiteral("  StackTrace: ");
                text.AppendLiteral(stackTrace);
            }
        }
        
    }
    
   
}