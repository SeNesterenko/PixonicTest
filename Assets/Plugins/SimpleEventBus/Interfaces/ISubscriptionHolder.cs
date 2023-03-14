using Plugins.SimpleEventBus.Events;

namespace Plugins.SimpleEventBus.Interfaces
{
    public interface ISubscriptionHolder
    {
        void Invoke(EventBase eventBase);
    }
}