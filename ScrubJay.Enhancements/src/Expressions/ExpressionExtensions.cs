using System.Linq.Expressions;
using ScrubJay.Enhancements.Text.Building;

namespace ScrubJay.Enhancements.Expressions;

public static class ExpressionExtensions
{
    extension(Expression? expression)
    {
        public void RenderTo(ref TextBuilder builder)
        {
            throw Ex.NotImplemented();
        }
    }
}