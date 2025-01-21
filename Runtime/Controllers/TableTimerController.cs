using System;
using UnityEngine;

namespace FusionGame.Core.Controllers
{
    internal class TableTimerController : TableMonoBehaviour
    {
        protected override void ResetValues()
        {
            Debug.Log($"On table timer controller {TableData.TableId} reset values");
        }

        protected override void OnTableDataBound()
        {
            Debug.Log($"On table timer controller bound to table {TableData.TableId} data: current timer value is {TableData.Timer.Value}");

            TableData.Timer.OnValueChanged += OnTimerValueChanged;
        }

        protected override void OnTableDataUnbound()
        {
            Debug.Log($"On table timer controller unbound from table {TableData.TableId} data");

            TableData.Timer.OnValueChanged -= OnTimerValueChanged;
        }

        private void OnTimerValueChanged(uint value)
        {
            Debug.Log($"On table timer controller {TableData.TableId} timer value is changed to {value}");
        }
    }
}
