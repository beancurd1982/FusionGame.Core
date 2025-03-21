using System;

namespace FusionGame.Core.Events
{
    public interface IEvent { }

    public interface ITableEvent : IEvent
    {
        public uint TableId { get; }
    }

    public interface IEventBinding<T>
    {
        public Action<T> OnEvent { get; set; }
    }
}