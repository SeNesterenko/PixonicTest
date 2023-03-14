using Plugins.SimpleEventBus.Events;
using Plugins.SimpleEventBus.Interfaces;

namespace Plugins.SimpleEventBus.Extensions
{
    public static class SimpleBusExtensions
    {
        public static void Publish(this EventBase eventBase, IEventBus eventBus)
        {
            eventBus.Publish(eventBase);
        }
    }
}
