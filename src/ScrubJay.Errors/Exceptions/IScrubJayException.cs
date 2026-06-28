#pragma warning disable CA1711

namespace ScrubJay.Errors.Exceptions;

[PublicAPI]
public interface IScrubJayException<TSelf>
    where TSelf : Exception, IScrubJayException<TSelf>;