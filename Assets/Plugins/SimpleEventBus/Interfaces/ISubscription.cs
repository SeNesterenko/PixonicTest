using Plugins.SimpleEventBus.Events;

namespace Plugins.SimpleEventBus.Interfaces
{
    public interface ISubscription
    {
        /// <summary>
        /// Publish to the subscriber
        /// </summary>
        /// <param name="eventBase"></param>
        void Publish(EventBase eventBase);
    }
}
