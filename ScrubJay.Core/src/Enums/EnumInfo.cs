using System.Globalization;
using System.Reflection;
using ScrubJay.Memory.Splitting;
using ScrubJay.Reflection;

namespace ScrubJay.Enums;


public abstract class EnumInfo
{
    public Type EnumType { get; }
    public Attributes Attributes { get; }
    public bool IsFlags { get; }
    public Type UnderlyingType { get; }
    public IReadOnlyList<EnumMember> Members { get; }

    protected EnumInfo(Type enumType, IReadOnlyList<EnumMember> members)
    {
        Debug.Assert(enumType.IsEnum);
        this.EnumType = enumType;
        this.Attributes = new(Attribute.GetCustomAttributes(enumType));
        this.IsFlags = Attributes.HasAttribute<FlagsAttribute>();
        this.UnderlyingType = Enum.GetUnderlyingType(enumType);
        this.Members = members;
    }

    public bool IsDefined(Enum? @enum)
    {
        if (@enum is null)
            return false;

        try
        {
            return Enum.IsDefined(EnumType, @enum);
        }
        catch (Exception ex)
        {
            return false;
        }
    }
}

    public sealed class EnumInfo<E> : EnumInfo
        where E : struct, Enum
    {
        public static readonly bool IsSigned = 
            Type.GetTypeCode(typeof(E).GetEnumUnderlyingType()) 
                is TypeCode.SByte or TypeCode.Int16 or TypeCode.Int32 or TypeCode.Int64;
        
        public static readonly bool IsUnsigned = 
            Type.GetTypeCode(typeof(E).GetEnumUnderlyingType()) 
                is TypeCode.Byte or TypeCode.UInt16 or TypeCode.UInt32 or TypeCode.UInt64;
        
        public new IReadOnlyList<EnumMember<E>> Members { get; }

        private static List<EnumMember<E>> GetEnumMembers()
        {
            var enumType = typeof(E);
            var memberFields = typeof(E).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            var members = new List<EnumMember<E>>();
            foreach (var memberField in memberFields)
            {
                // construct the EnumMember<E>
                var enumMember = typeof(EnumMember<>)
                    .MakeGenericType(enumType)
                    .GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic)
                    .FirstOrDefault()
                    .ThrowIfNull()
                    .Invoke(parameters: [memberField])
                    .ThrowIfNot<EnumMember<E>>();
                members.Add(enumMember);
            }
            return members;
        }

        public EnumInfo()
            : base(typeof(E), GetEnumMembers())
        {
            this.Members = (List<EnumMember<E>>)base.Members;
        }

        public bool IsDefined(E @enum)
        {
            try
            {
                return Enum.IsDefined<E>(@enum);
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public EnumMember<E>? GetMember(E @enum)
        {
            foreach (var enumMember in Members)
            {
                if (enumMember.Member.IsEqualTo(@enum))
                {
                    return enumMember;
                }
            }
            return null;
        }

        private Result<E> TryParseOne(scoped text text)
        {
            foreach (var enumMember in Members)
            {
#if NET9_0_OR_GREATER
                var lookup = enumMember._aliases.GetAlternateLookup<text>();
                if (lookup.Contains(text))
                {
                    return enumMember.Member;
                }
#else
                foreach (var alias in enumMember._aliases)
                {
                    if (text.Equate(alias))
                    {
                        return enumMember.Member;
                    }
                }
#endif
            }

            if (int.TryParse(text, NumberStyles.Any, null, out int i32))
            {
                foreach (var enumMember in Members)
                {
                    if (enumMember.Int32Value == i32)
                    {
                        return enumMember.Member;
                    }
                }
            }

            // could not
            return Ex.Parse<E>(text);
        }

        private Result<E> TryParseOne(string str)
        {
            foreach (var enumMember in Members)
            {
                if (enumMember._aliases.Contains(str))
                {
                    return enumMember.Member;
                }
            }

            if (int.TryParse(str, NumberStyles.Any, null, out int i32))
            {
                foreach (var enumMember in Members)
                {
                    if (enumMember.Int32Value == i32)
                    {
                        return enumMember.Member;
                    }
                }
            }

            // could not
            return Ex.Parse<E>(str);
        }

        public Result<E> TryParse(scoped text text)
        {
            if (IsFlags)
            {
                E @enum = default;
                var segments = text.SplitAny(",|", SplitOptions.IgnoreEmpty | SplitOptions.Trim);
                if (!segments.TryMoveNext(out var segment))
                {
                    return Ex.Parse<E>(text);
                }

                do
                {
                    if (TryParseOne(segment).IsError(out var ex, out var flag))
                        return ex;

                    @enum.AddFlag(flag);
                } while (segments.TryMoveNext(out segment));

                return @enum;
            }
            else
            {
                return TryParseOne(text);
            }
        }

        public Result<E> TryParse(string? str)
        {
            if (str is null)
                return Ex.ArgNull<string>(nameof(str));

            if (IsFlags)
            {
                E @enum = default;
                var segments = str.SplitAny(",|", SplitOptions.IgnoreEmpty | SplitOptions.Trim);
                if (!segments.TryMoveNext(out var segment))
                {
                    return Ex.Parse<E>(str);
                }

                do
                {
                    if (TryParseOne(segment).IsError(out var ex, out var flag))
                        return ex;

                    @enum.AddFlag(flag);
                } while (segments.TryMoveNext(out segment));

                return @enum;
            }
            else
            {
                return TryParseOne(str);
            }
        }
    }