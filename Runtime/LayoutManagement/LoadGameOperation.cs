using System;
using System.Collections;

namespace FusionGame.Core.LayoutManagement
{
    public abstract class LoadGameOperation
    {
        protected GameLoader GameLoader { get; private set; }

        protected LoadGameOperation(GameLoader gameLoader)
        {
            GameLoader = gameLoader;
        }

        public abstract IEnumerator LoadGame(Action<bool, ILoadableGame> onCompleted);

        public abstract override bool Equals(object obj);

        public abstract override int GetHashCode();

        public abstract override string ToString();
    }
}
