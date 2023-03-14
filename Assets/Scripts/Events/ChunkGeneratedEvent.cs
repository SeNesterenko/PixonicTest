using Models;
using Plugins.SimpleEventBus.Events;

namespace Events
{
    public class ChunkGeneratedEvent : EventBase
    {
        public ChunkModel ChunkModel { get; }
        
        public ChunkGeneratedEvent(ChunkModel chunkModel)
        {
            ChunkModel = chunkModel;
        }
    }
}