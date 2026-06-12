namespace ScrubJay.Errors.Extensions;

/// <summary>
/// Extensions on <see cref="Exception"/> instances.
/// </summary>
[PublicAPI]
public static class ExceptionExtensions
{
    extension<E>(E exception)
        where E : Exception
    {
        /// <summary>
        /// Gets a <see cref="Uri"/> that points to the <see href="https://www.hresult.info"/> search for this Exception's <see cref="HResult"/>.
        /// </summary>
        public Uri HResultInfo => new($"https://www.hresult.info/Search?q=0x{exception.HResult:X8}");

    }

    extension<E>(E exception)
        where E : Exception, ISJException
    {
        public void Add(string key, object? value)
        {

        }
    }
}