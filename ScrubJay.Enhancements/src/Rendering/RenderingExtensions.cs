using ScrubJay.Enhancements.Text.Building;

namespace ScrubJay.Enhancements.Rendering;

public static class RenderingExtensions
{
    extension<T>(ref readonly T value)
        where T : struct
    {
        public void RenderTo(ref TextBuilder builder)
        {
            throw new NotImplementedException();
        }

        public string Render()
        {
            var builder = new TextBuilder();
            value.RenderTo(ref builder);
            return builder.ToStringAndDispose();
        }
    }

   
}

public static class RenderingExtensions2
{
    extension<T>(T? value)
    {
        public void RenderTo(ref TextBuilder builder)
        {
            throw new NotImplementedException();
        }
        
        public string Render()
        {
            var builder = new TextBuilder();
            value.RenderTo(ref builder);
            return builder.ToStringAndDispose();
        }
    }
}