using System.Collections.Generic;
using FusionGame.Core.Data;
using FusionGame.Core.LayoutManagement;
using UnityEngine;

namespace FusionGame.Core.Controllers
{
    internal class CanvasTableGame : MonoBehaviour, ITableGame
    {
        /// <summary>
        /// The root canvas
        /// </summary>
        [SerializeField] private Canvas rootCanvas;

        /// <summary>
        /// The name of the game. For example it could be "QuartzSingleViewBaccarat"
        /// </summary>
        [SerializeField] private string gameName;

        public GameObject GetGameObject => gameObject;

        public ILoadableGame ParentGame => this;

        public bool IsOnScreen { get; private set; }

        public ITableData TableData { get; private set; }

        public string GetName
        {
            get
            {
                if (TableData != null)
                {
                    return $"{gameName} - {TableData.TableId}";
                }
                else
                {
                    return gameName;
                }
            }
        }

        public GameObject GetRootGameObject => gameObject;

        protected HashSet<IGameComponent> GameComponents = new HashSet<IGameComponent>();

        public void OnLoaded()
        {
            Debug.Log($"The canvas table game {gameName} has just been loaded.");

            // register all game components
            var gameComponents = GetComponentsInChildren<IGameComponent>();
            foreach (var gameComponent in gameComponents)
            {
                if (ReferenceEquals(gameComponent, this)) continue;

                GameComponents.Add(gameComponent);
                gameComponent.RegisterParentGame(this);

                // bind table data for the component if available
                if (TableData != null && gameComponent is ITableGameComponent tableGameComponent)
                {
                    tableGameComponent.BindTableData(TableData);
                }
            }
        }

        public void OnUnload()
        {
            Debug.Log($"The canvas table game {gameName} is about to be unloaded.");

            if (TableData != null)
            {
                UnbindTableData();
            }
        }

        public void OffScreen()
        {
            IsOnScreen = false;

            // disable the root canvas first
            // this will avoid recalculates the layout and trigger of re-batching of child UI elements
            rootCanvas.enabled = false;

            foreach (var gameComponent in GameComponents)
            {
                if (gameComponent is IOnScreen onScreenComponent)
                {
                    onScreenComponent.OffScreen();
                }
            }
        }

        public void OnScreen()
        {
            IsOnScreen = true;

            foreach (var gameComponent in GameComponents)
            {
                if (gameComponent is IOnScreen onScreenComponent)
                {
                    onScreenComponent.OnScreen();
                }
            }

            // enabling the root canvas last will avoid multiple re-layout operations.
            // When the root canvas is enabled, it activates all its children in one step, batching the UI calculations for the entire hierarchy.
            rootCanvas.enabled = true;
        }

        public void BindTableData(ITableData data)
        {
            if (data == null)
            {
                throw new System.ArgumentNullException(nameof(data));
            }
            else if (TableData != null)
            {
                // TODO: support multiple table data binding
                throw new System.InvalidOperationException("Data is already bound to this table game");
            }
            else
            {
                TableData = data;
                TableData.OnDestroy += OnTableDataDestroyed;

                // bind table data for all children components
                foreach (var gameComponent in GameComponents)
                {
                    if (gameComponent is ITableGameComponent tableGameComponent)
                    {
                        tableGameComponent.BindTableData(data);
                    }
                }
            }
        }

        public void UnbindTableData()
        {
            if (TableData == null) return;

            TableData.OnDestroy -= OnTableDataDestroyed;

            // unbind table data for all children components
            foreach (var gameComponent in GameComponents)
            {
                if (gameComponent is ITableGameComponent tableGameComponent)
                {
                    tableGameComponent.UnbindTableData();
                }
            }

            // set the table data to null in the end
            TableData = null;
        }

        public void RegisterChildGameComponent(IGameComponent childGameComponent)
        {
            if (childGameComponent == null)
            {
                throw new System.ArgumentNullException(nameof(childGameComponent));
            }

            // check if the child game component is already registered
            if (GameComponents.Contains(childGameComponent)) return;

            GameComponents.Add(childGameComponent);
            childGameComponent.RegisterParentGame(this);

            // bind table data if available
            if (TableData != null && childGameComponent is ITableGameComponent tableGameComponent)
            {
                tableGameComponent.BindTableData(TableData);
            }
        }

        public void RegisterParentGame(ILoadableGame parentGame)
        {
            // do nothing as this component is the game itself
        }

        private void OnTableDataDestroyed()
        {
            TableData.OnDestroy -= OnTableDataDestroyed;

            // no need to unbind data for all children components, as they listen to TableData.OnDestroy event and will unbind themselves

            // set the table data to null in the end
            TableData = null;
        }
    }
}
