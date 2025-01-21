using System;
using FusionGame.Core.LayoutManagement;

namespace FusionGame.Core.Controllers
{
    /// <summary>
    /// Inherit from this class to create a table game component that can access the table data and be notified when it goes on or off screen.
    /// </summary>
    public abstract class TableOnScreenMonoBehaviour : TableMonoBehaviour, IOnScreen
    {
        public bool IsOnScreen { get; private set; }

        public void OffScreen()
        {
            IsOnScreen = false;

            throw new NotImplementedException();
        }

        public void OnScreen()
        {
            IsOnScreen = true;

            throw new NotImplementedException();
        }
    }
}
