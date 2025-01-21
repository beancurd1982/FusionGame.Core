using System.Collections.Generic;
using System;

namespace FusionGame.Core.LayoutManagement
{
    public interface ILayout
    {
        /// <summary>
        /// The current view type
        /// </summary>
        ViewTypeBase CurrentViewType { get; }

        /// <summary>
        /// Is transition in progress
        /// </summary>
        bool InTransition { get; }

        // /// <summary>
        // /// All the games that are currently on screen
        // /// </summary>
        // HashSet<ILoadableGame> OnScreenGames { get; }

        /// <summary>
        /// All the games that are loaded and managed by this layout
        /// </summary>
        HashSet<ILoadableGame> Games { get; }

        /// <summary>
        /// The active table id
        /// In most of the cases this will be the table id of the game that player previously interacted with.
        /// If there is no game loaded at all, this will be 0.
        /// </summary>
        uint ActiveTableId { get; }

        /// <summary>
        /// Is there any table game with the specific tableId on screen
        /// </summary>
        /// <param name="tableId"></param>
        /// <param name="tableGames"></param>
        /// <returns></returns>
        bool IsTableOnScreen(uint tableId, out HashSet<ITableGame> tableGames);

        /// <summary>
        /// Is the game component on screen
        /// </summary>
        /// <param name="component"></param>
        /// <returns></returns>
        bool IsComponentOnScreen(IGameComponent component);

        /// <summary>
        /// Switch to a target view type with transition animation.
        /// </summary>
        /// <param name="targetViewType">the target view type</param>
        /// <param name="targetTableId">the target table id</param>
        /// <param name="duration">the duration of the transition in second</param>
        /// <param name="onCompleted">call back when the transition is completed</param>
        void SwitchToView(ViewTypeBase targetViewType, uint targetTableId, float duration, Action<bool> onCompleted);

        /// <summary>
        /// Switch to a target view type instantly without transition animation.
        /// </summary>
        /// <param name="targetViewType">the target view type</param>
        /// <param name="targetTableId">the target table id</param>
        /// <returns>true if switch to the target view type successfully, false otherwise</returns>
        bool SwitchToViewInstantly(ViewTypeBase targetViewType, uint targetTableId);

        /// <summary>
        /// Can we switch to the target view type with the target table id
        /// </summary>
        /// <param name="targetViewType"></param>
        /// <param name="targetTableId"></param>
        /// <returns></returns>
        bool CanSwitchToView(ViewTypeBase targetViewType, uint targetTableId);
    }
}
