using System.Reflection;

namespace ScrubJay.Enums;

public static class EnumHelper
{
    private static readonly ConcurrentTypeMap<EnumInfo> _cache = [];

    [Flags]
    public enum EnumParseOptions
    {
        None = 0,

        IgnoreCase = 1 << 0,

        IncludeDisplays = 1 << 1,
    }

    public sealed class Attributes : IReadOnlyList<Attribute>
    {
        private Attribute[] _attributes;

        public int Count => _attributes.Length;

        public Attribute this[int index]
        {
            get
            {
                return _attributes[Guard.Index(index, Count)];
            }
        }

        internal Attributes(Attribute[] attributes)
        {
            _attributes = attributes;
        }

        public bool HasAttribute<A>()
            where A : Attribute
        {
            return _attributes.OfType<A>().Any();
        }

        public bool HasAttribute<A>([NotNullWhen(true)] out A? attribute)
            where A : Attribute
        {
            attribute = _attributes.OfType<A>().FirstOrDefault();
            return attribute is not null;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        public IEnumerator<Attribute> GetEnumerator()
        {
            foreach (Attribute attribute in _attributes)
            {
                yield return attribute;
            }
        }
    }

    public class EnumMembers
    {
        
    }

    public class EnumMembers<E> : EnumMembers
        where E : struct, Enum
    {
        
    }
    
    internal abstract class EnumInfo
    {
        public Type EnumType { get; }
        public Attributes Attributes { get; }
        public bool IsFlags { get; }
        public Type UnderlyingType { get; }
        public EnumMembers Members { get; }

        protected EnumInfo(Type enumType, EnumMembers members)
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


        public Result<Enum> TryParse(Type enumType, string? str, EnumParseOptions options = EnumParseOptions.None)
        {
            throw Ex.NotImplemented();
        }

        public Result<Enum> TryParse(Type enumType, scoped text text, EnumParseOptions options = EnumParseOptions.None)
        {
            throw Ex.NotImplemented();
        }

        [return: NotNullIfNotNull(nameof(@enum))]
        public string? GetName(Enum? @enum)
        {
            throw Ex.NotImplemented();
        }

        public string? GetDisplay(Enum? @enum)
        {
            throw Ex.NotImplemented();
        }
    }

    internal sealed class EnumInfo<E> : EnumInfo
        where E : struct, Enum
    {
        private static EnumMembers<E> GetEnumMembers()
        {
            var memberFields = typeof(E).GetFields(BindingFlags.Public | BindingFlags.Static | BindingFlags.DeclaredOnly);
            
        }
        
        private EnumInfo() : base(typeof(E))
        {

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
    }
}