using FusionGame.Core.Data;

namespace FusionGame.Core.RuntimeTests
{
    public class SimpleData
    {
        public AtomicDataProperty<int> IntProperty { get; set; } = new AtomicDataProperty<int>();
        public AtomicDataProperty<int> IntPropertyWithSpecificInitialValue { get; set; } = new AtomicDataProperty<int>(5);
        public AtomicDataProperty<string> StringProperty { get; set; } = new AtomicDataProperty<string>();
        public AtomicDataProperty<string> StringPropertyWithSpecificInitialValue { get; set; } = new AtomicDataProperty<string>("Initial Value");
        public AtomicDataProperty<bool> BoolProperty { get; set; } = new AtomicDataProperty<bool>();
        public AtomicDataProperty<bool> BoolPropertyWithSpecificInitialValue { get; set; } = new AtomicDataProperty<bool>(true);
    }
}