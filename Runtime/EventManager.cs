using Unity.Burst;
using Unity.Collections;
using Unity.Entities;

namespace EntityEvents
{
    public partial struct EventManager<TEvent, TBufferElement> : ISystem where TEvent : unmanaged, IComponentData where TBufferElement : unmanaged, IBufferElementData, IEventConverter<TEvent>
    {
        [BurstDiscard]
        public void OnUpdate(ref SystemState state)
        {
            EndSimulationEntityCommandBufferSystem ecbSystem = state.World.GetExistingSystemManaged<EndSimulationEntityCommandBufferSystem>();
            if (ecbSystem == null) return;

            EntityCommandBuffer ecb = ecbSystem.CreateCommandBuffer();

            using (NativeArray<Entity> events = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<TEvent>()).ToEntityArray(Allocator.TempJob))
            {
                foreach (Entity @event in events)
                {
                    ecb.DestroyEntity(@event);
                }
            }

            using (NativeArray<Entity> eventRequests = state.EntityManager.CreateEntityQuery(ComponentType.ReadOnly<TBufferElement>()).ToEntityArray(Allocator.TempJob))
            {
                foreach (Entity eventRequest in eventRequests)
                {
                    DynamicBuffer<TBufferElement> eventBuffer = state.EntityManager.GetBuffer<TBufferElement>(eventRequest);
                    foreach (TBufferElement bufferElement in eventBuffer)
                    {
                        Entity eventEntity = ecb.CreateEntity();
                        ecb.AddComponent(eventEntity, bufferElement.ToEvent());
                    }
                    eventBuffer.Clear();
                }
            }
        }
    }
}
