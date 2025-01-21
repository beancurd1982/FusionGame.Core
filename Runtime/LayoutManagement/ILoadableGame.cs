using FusionGame.Core.Data;
using UnityEngine;

namespace FusionGame.Core.LayoutManagement
{
    /// <summary>
    /// Implement this interface if you want your game component to be notified when it goes on or off screen
    /// </summary>
    public interface IOnScreen
    {
        /// <summary>
        /// Is this object currently on screen
        /// </summary>
        bool IsOnScreen { get; }

        /// <summary>
        /// Called when the object is going off screen
        /// </summary>
        void OffScreen();

        /// <summary>
        /// Called when the object is going on screen
        /// </summary>
        void OnScreen();
    }

    /// <summary>
    /// Implement this interface if you need your game to be loaded and unloaded by the GameLoader
    /// </summary>
    public interface ILoadableGame : IOnScreen
    {
        /// <summary>
        /// Get the root game object of this game
        /// </summary>
        GameObject GetRootGameObject { get; }

        /// <summary>
        /// Get the name of this game
        /// </summary>
        string GetName { get; }

        /// <summary>
        /// Called when the game is loaded
        /// </summary>
        void OnLoaded();

        /// <summary>
        /// Called when the game is about to be unloaded
        /// </summary>
        void OnUnload();
    }

    /// <summary>
    /// Implement this interface for your game components
    /// </summary>
    public interface IGameComponent
    {
        /// <summary>
        /// Get the game object of this game component
        /// </summary>
        GameObject GetGameObject { get; }

        ILoadableGame ParentGame { get; }

        void RegisterParentGame(ILoadableGame parentGame);
    }

    /// <summary>
    /// Implement this interface for your table game components.
    /// The table component can access the table data.
    /// </summary>
    public interface ITableGameComponent : IGameComponent
    {
        /// <summary>
        /// The table data bound to this component
        /// </summary>
        ITableData TableData { get; }

        /// <summary>
        /// Bind table data to this component
        /// </summary>
        /// <param name="data"></param>
        void BindTableData(ITableData data);

        /// <summary>
        /// Unbind table data from this component
        /// </summary>
        void UnbindTableData();
    }

    public interface ITableGame : ILoadableGame, ITableGameComponent
    {
        void RegisterChildGameComponent(IGameComponent childGameComponent);
    }
}
