namespace ScrubJay.Enhancements.Exceptions;

[PublicAPI]
public interface IEnhancedArgumentException : IEnhancedException
{
    Argument Argument { get; }
}