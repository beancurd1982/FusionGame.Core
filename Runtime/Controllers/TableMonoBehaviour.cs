using FusionGame.Core.Data;
using FusionGame.Core.LayoutManagement;
using UnityEngine;

namespace FusionGame.Core.Controllers
{
    /// <summary>
    /// Inherit from this class to create a table game component that can access the table data.
    /// </summary>
    public abstract class TableMonoBehaviour : MonoBehaviour, ITableGameComponent
    {
        public ITableData TableData { get; private set; }
        public GameObject GetGameObject => gameObject;
        public ILoadableGame ParentGame { get; private set; }

        public virtual void RegisterParentGame(ILoadableGame parentGame)
        {
            if (ParentGame != null)
            {
                throw new System.InvalidOperationException($"Parent game is already set for {gameObject.name}, can't register again.");
            }
            else if (parentGame == null)
            {
                throw new System.ArgumentNullException(nameof(parentGame));
            }
            else
            {
                ParentGame = parentGame;
            }
        }

        public virtual void BindTableData(ITableData data)
        {
            if (data == null)
            {
                throw new System.ArgumentNullException("data");
            }
            else if (TableData != null)
            {
                throw new System.InvalidOperationException("Data is already bound to this object");
            }
            else
            {
                TableData = data;
                TableData.OnDestroy += UnbindTableData;

                OnTableDataBound();
            }
        }

        public void UnbindTableData()
        {
            if (TableData == null) return;

            TableData.OnDestroy -= UnbindTableData;

            //TODO: avoid call reset values if the table game is about to be destroyed
            ResetValues();

            // trigger the OnTableDataUnbound method call before the TableData is set to null
            // this is to allow the user to unsubscribe from the table data events.
            OnTableDataUnbound();

            // set the table data to null in the end
            TableData = null;
        }

        protected virtual void Awake()
        {
            ResetValues();
        }

        /// <summary>
        /// This method can be triggered on start up before the table data is bound, or after the table data is unbound.
        /// Implement this method to reset the component values to their default state.
        /// </summary>
        protected abstract void ResetValues();

        /// <summary>
        /// This method is called after the table data is properly bound.
        /// It's guaranteed that the TableData property is not null.
        /// Implement this method to
        /// 1. set up the component values based on the table data.
        /// 2. subscribe to the table data events.
        /// </summary>
        protected abstract void OnTableDataBound();

        /// <summary>
        /// This method is called when the table data is about to be unbound.
        /// Implement this method to unsubscribe from the table data events.
        /// You can still access to the data in this method.
        /// However, you should not rely on the data property values anymore as the TableData property will be set to null after this method is called,
        /// </summary>
        protected abstract void OnTableDataUnbound();
    }
}
