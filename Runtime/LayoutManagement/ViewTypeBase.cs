namespace FusionGame.Core.LayoutManagement
{
    public abstract class ViewTypeBase
    {
        public abstract uint GetValue { get; }
        public abstract override int GetHashCode();
        public abstract override string ToString();

        public override bool Equals(object obj)
        {
            if (obj == null || GetType() != obj.GetType())
            {
                return false;
            }

            return true;
        }
    }

    public class LobbyViewType : ViewTypeBase
    {
        public override uint GetValue => 0;

        public override int GetHashCode()
        {
            return 0;
        }

        public override string ToString()
        {
            return "LobbyViewType";
        }
    }

    public class SingleGameViewType : ViewTypeBase
    {
        public override uint GetValue => 1;

        public override int GetHashCode()
        {
            return 1;
        }

        public override string ToString()
        {
            return "SingleGameView";
        }
    }
}