using System;
using Unity.Entities;

namespace EntityEvents
{
    [AttributeUsage(AttributeTargets.Struct, Inherited = false, AllowMultiple = false)]
    public sealed class EventAttribute : Attribute
    {
        public WorldSystemFilterFlags WorldFilter { get; }

        public EventAttribute(WorldSystemFilterFlags worldFilter)
        {
            WorldFilter = worldFilter;
        }
    }
}
