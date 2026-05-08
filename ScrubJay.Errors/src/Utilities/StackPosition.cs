namespace ScrubJay.Errors.Utilities;

public class StackPosition
{
    public static StackPosition Capture()
    {
        var stacktrace = new StackTrace(1, true); // skip the Capture() method itself

        throw new NotImplementedException();
    }
}