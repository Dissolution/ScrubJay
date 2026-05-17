#pragma warning disable CA1822

namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class FieldInfoExtensions
{
    extension(FieldInfo)
    {
    }

    extension(FieldInfo? fieldInfo)
    {
        public Visibility Visibility
        {
            get
            {
                Visibility visibility = default;
                if (fieldInfo is not null)
                {
                    visibility |= fieldInfo.IsStatic ? Visibility.Static : Visibility.Instance;
                    if (fieldInfo.IsPublic)
                        visibility |= Visibility.Public;
                    if (fieldInfo.IsAssembly || fieldInfo.IsFamilyAndAssembly || fieldInfo.IsFamilyOrAssembly)
                        visibility |= Visibility.Internal;
                    if (fieldInfo.IsFamily || fieldInfo.IsFamilyAndAssembly || fieldInfo.IsFamilyOrAssembly)
                        visibility |= Visibility.Protected;
                    if (fieldInfo.IsPrivate)
                        visibility |= Visibility.Private;
                }

                return visibility;
            }
        }
    }
}