using System;
using UnityEngine;

namespace FusionGame.Core.Data
{
    public class TableData : ITableData
    {
        public uint TableId { get; set; }

        public IDataProperty<uint> Timer => timer;

        public event Action OnDestroy;

        private readonly DataProperty<uint> timer = new DataProperty<uint>(0);

        public void Destroy()
        {
            OnDestroy?.Invoke();
        }

        public TableData(uint tableId)
        {
            if (tableId == 0)
            {
                throw new ArgumentException("TableId cannot be 0");
            }

            TableId = tableId;
        }

        public void UpdateTimer(uint value)
        {
            timer.Value = value;
        }
    }
}
