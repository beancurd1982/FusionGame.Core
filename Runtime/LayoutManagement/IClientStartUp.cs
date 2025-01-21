using FusionGame.Core.Data;

namespace FusionGame.Core.LayoutManagement
{
    public interface IClientStartUp
    {
        /// <summary>
        /// Reset the game layout to the default status when the game starts up.
        /// This method is only called once when the game starts up.
        /// </summary>
        void ResetGameLayoutOnStartUp(IPlayerTerminalData playerTerminalData);

        /// <summary>
        /// Reset the game layout to the default status when the user cashes out.
        /// </summary>
        void ResetGameLayoutOnUserCashOut(IPlayerTerminalData playerTerminalData);

        /// <summary>
        /// Reset the game layout to the default status when the user buys in.
        /// </summary>
        void ResetGameLayoutOnUserBuyIn(IPlayerTerminalData playerTerminalData);

        /// <summary>
        /// Reset the game layout when the table list has changed.
        /// Tables might have been added or removed, the layout should be updated accordingly.
        /// </summary>
        void ResetGameLayoutOnTableListChanged(IPlayerTerminalData playerTerminalData);

        /// <summary>
        /// Reset the game layout when the user times out.
        /// The player might have played with the layout while there is not credit.
        /// After the player left the PT, the layout will be reset to the default status after certain amount of time (30 seconds for example).
        /// </summary>
        void ResetGameLayoutOnUserTimeOut(IPlayerTerminalData playerTerminalData);
    }
}
