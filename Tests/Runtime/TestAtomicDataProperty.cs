using NUnit.Framework;

namespace FusionGame.Core.RuntimeTests
{
    public class TestAtomicDataProperty
    {
        [Test]
        public void IntProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Act
            myDataClass.IntProperty.SetValue(10);

            // Assert
            Assert.AreEqual(10, myDataClass.IntProperty.Value);
        }

        [Test]
        public void IntPropertyWithSpecificInitialValue_ShouldHaveInitialValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Assert
            Assert.AreEqual(5, myDataClass.IntPropertyWithSpecificInitialValue.Value);
        }

        [Test]
        public void StringProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Act
            myDataClass.StringProperty.SetValue("Test");

            // Assert
            Assert.AreEqual("Test", myDataClass.StringProperty.Value);
        }

        [Test]
        public void StringPropertyWithSpecificInitialValue_ShouldHaveInitialValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Assert
            Assert.AreEqual("Initial Value", myDataClass.StringPropertyWithSpecificInitialValue.Value);
        }

        [Test]
        public void BoolProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Act
            myDataClass.BoolProperty.SetValue(true);

            // Assert
            Assert.IsTrue(myDataClass.BoolProperty.Value);
        }

        [Test]
        public void BoolPropertyWithSpecificInitialValue_ShouldHaveInitialValue()
        {
            // Arrange
            var myDataClass = new SimpleData();

            // Assert
            Assert.IsTrue(myDataClass.BoolPropertyWithSpecificInitialValue.Value);
        }

        [Test]
        public void Properties_ShouldTriggerOnValueChanged()
        {
            // Arrange
            var myDataClass = new SimpleData();
            bool intEventTriggered = false;
            bool stringEventTriggered = false;
            bool boolEventTriggered = false;

            myDataClass.IntProperty.OnValueChanged += (newValue) => intEventTriggered = true;
            myDataClass.StringProperty.OnValueChanged += (newValue) => stringEventTriggered = true;
            myDataClass.BoolProperty.OnValueChanged += (newValue) => boolEventTriggered = true;

            // Act
            myDataClass.IntProperty.SetValue(10);
            myDataClass.StringProperty.SetValue("Test");
            myDataClass.BoolProperty.SetValue(true);

            // Assert
            Assert.IsTrue(intEventTriggered);
            Assert.IsTrue(stringEventTriggered);
            Assert.IsTrue(boolEventTriggered);
        }
    }
}