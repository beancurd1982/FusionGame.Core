using System;

namespace FusionGame.Core.Data
{
    public interface ITableData
    {
        uint TableId { get; }

        IDataProperty<uint> Timer { get; }

        event Action OnDestroy;

        void Destroy();

    }
}

