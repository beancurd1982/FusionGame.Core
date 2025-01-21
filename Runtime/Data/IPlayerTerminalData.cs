using System.Collections.Generic;

namespace FusionGame.Core.Data
{
    public interface IPlayerTerminalData
    {
        IDataProperty<HashSet<uint>> TablesToRecover { get; }
        IDataProperty<uint> ActiveViewType { get; }
        IDataProperty<uint> ActiveTableId { get; }
    }
}
