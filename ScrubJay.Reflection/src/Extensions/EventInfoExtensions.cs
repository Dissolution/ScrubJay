namespace ScrubJay.Reflection.Extensions;

[PublicAPI]
public static class EventInfoExtensions
{
    extension(EventInfo)
    {
    }

    extension(EventInfo? @event)
    {
        public Visibility Visibility
        {
            get
            {
                Visibility visibility = default;
                if (@event is not null)
                {
                    visibility |= @event.AddMethod.Visibility;
                    visibility |= @event.RemoveMethod.Visibility;
                    visibility |= @event.RaiseMethod.Visibility;
                }

                return visibility;
            }
        }
    }
}