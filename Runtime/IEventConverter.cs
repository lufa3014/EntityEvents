using Unity.Entities;

namespace EntityEvents
{
    public interface IEventConverter<out TEvent> where TEvent : unmanaged, IComponentData
    {
        TEvent ToEvent();
    }
}
