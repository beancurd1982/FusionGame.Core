namespace FusionGame.Core.LayoutManagement
{
    internal interface IUITransition
    {
        void OnStartTransition();
        void OnTransition(float progress);
        void OnEndTransition();
    }
}
