namespace FusionGame.Core.LayoutManagement
{
    public class LayoutSystem
    {
        public static ILayout ActiveLayout { get; private set; }

        public static void RegisterAsActiveLayout(ILayout layout)
        {
            ActiveLayout = layout;
        }
    }
}
