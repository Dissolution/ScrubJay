namespace ScrubJay.Exceptions;

[PublicAPI]
public interface IEnhancedArgumentException : IEnhancedException
{
    Argument Argument { get; }
}