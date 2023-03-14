using Plugins.SimpleEventBus;
using Plugins.SimpleEventBus.Interfaces;

namespace Events
{
    public static class EventStreams
    {
        public static IEventBus Game { get; } = new EventBus();
    }
}