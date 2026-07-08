namespace ScrubJay.Text.Tests;

public class InterpolatedTextTests
{
    [Fact]
    public void DefaultInstanceWorks()
    {
        InterpolatedText text = default;
        Assert.Equal(0, text.Length);
        Assert.Equal("", text.ToString());
        
        text.AppendLiteral("TRJ");
        Assert.Equal(3, text.Length);
        Assert.Equal("TRJ", text.ToString());
        
        text.Dispose();
        Assert.Equal(0, text.Length);
        Assert.Equal("", text.ToString());
    }
    
    [Fact]
    public void NewInstanceWorks()
    {
        InterpolatedText text = new();
        Assert.Equal(0, text.Length);
        Assert.Equal("", text.ToString());
        
        text.AppendLiteral("TRJ");
        Assert.Equal(3, text.Length);
        Assert.Equal("TRJ", text.ToString());
        
        text.Dispose();
        Assert.Equal(0, text.Length);
        Assert.Equal("", text.ToString());
    }
}