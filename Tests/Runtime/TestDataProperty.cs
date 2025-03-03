using NUnit.Framework;
using System.Collections.Generic;

namespace FusionGame.Core.RuntimeTests
{
    public class TestDataProperty
    {
        [Test]
        public void IntProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new TableData();

            // Act
            myDataClass.PropertyTableId.SetValue(10);

            // Assert
            Assert.AreEqual(10, myDataClass.TableId.Value);
        }

        [Test]
        public void IntPropertyWithSpecificInitialValue_ShouldHaveInitialValue()
        {
            // Arrange
            var myDataClass = new TableData();

            // Assert
            Assert.AreEqual(5, myDataClass.BettingTimeInSecond.Value);
        }

        [Test]
        public void StringProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new TableData();

            // Act
            myDataClass.PropertyTableName.SetValue("Test");

            // Assert
            Assert.AreEqual("Test", myDataClass.TableName.Value);
        }

        [Test]
        public void StringPropertyWithSpecificInitialValue_ShouldHaveInitialValue()
        {
            // Arrange
            var myDataClass = new TableData();

            // Assert
            Assert.AreEqual("Initial Value", myDataClass.TableTitle.Value);
        }

        [Test]
        public void BoolProperty_ShouldSetAndGetCorrectValue()
        {
            // Arrange
            var myDataClass = new TableData();

            // Act
            myDataClass.PropertyIsLiveDealer.SetValue(true);

            // Assert
            Assert.IsTrue(myDataClass.IsLiveDealer.Value);
        }

        [Test]
        public void Properties_ShouldTriggerOnValueChanged()
        {
            // Arrange
            var myDataClass = new TableData();
            bool intEventTriggered = false;
            bool stringEventTriggered = false;
            bool boolEventTriggered = false;

            myDataClass.TableId.OnValueChanged += (newValue) => intEventTriggered = true;
            myDataClass.TableName.OnValueChanged += (newValue) => stringEventTriggered = true;
            myDataClass.IsLiveDealer.OnValueChanged += (newValue) => boolEventTriggered = true;

            // Act
            myDataClass.PropertyTableId.SetValue(10);
            myDataClass.PropertyTableName.SetValue("Test");
            myDataClass.PropertyIsLiveDealer.SetValue(true);

            // Assert
            Assert.IsTrue(intEventTriggered);
            Assert.IsTrue(stringEventTriggered);
            Assert.IsTrue(boolEventTriggered);
        }

        [Test]
        public void UserDataProperty_ShouldHaveDefaultValues()
        {
            // Arrange
            var simpleData = new TableData();

            // Act
            var userData = simpleData.User.Value;

            // Assert
            Assert.AreEqual("Default User Name", userData.Name);
            Assert.AreEqual(0, userData.Age);
            Assert.IsNotNull(userData.FavouriteFoods);
            Assert.IsEmpty(userData.FavouriteFoods);
        }

        [Test]
        public void CreateChangeList_ShouldCreateChangeListWithName()
        {
            // Arrange
            var simpleData = new TableData();
            var changeListName = "TestChangeList";

            // Act
            var changeList = simpleData.PropertyUser.CreateChangeList(changeListName);

            // Assert
            Assert.IsNotNull(changeList);
            Assert.AreEqual(changeListName, changeList.Name);
        }

        [Test]
        public void ApplyChangeList_ShouldApplyChangesCorrectly()
        {
            // Arrange
            var simpleData = new TableData();
            var changeList = simpleData.PropertyUser.CreateChangeList("TestChangeList");
            changeList.AppendChange(nameof(UserData.Name), "Updated User");
            changeList.AppendChange(nameof(UserData.Age), 30);
            changeList.AppendChange(nameof(UserData.FavouriteFoods), new List<string> { "Sushi", "Tacos" });

            var iSimpleData = (ITableData)simpleData;
            iSimpleData.User.OnPropertyChanged += (propertyName) =>
            {
                switch (propertyName)
                {
                    case nameof(UserData.Name):
                        Assert.AreEqual("Updated User", iSimpleData.User.Value.Name);
                        break;
                    case nameof(UserData.Age):
                        Assert.AreEqual(30, iSimpleData.User.Value.Age);
                        break;
                    case nameof(UserData.FavouriteFoods):
                        Assert.IsNotNull(iSimpleData.User.Value.FavouriteFoods);
                        Assert.AreEqual(2, iSimpleData.User.Value.FavouriteFoods.Count);
                        Assert.Contains("Sushi", iSimpleData.User.Value.FavouriteFoods);
                        Assert.Contains("Tacos", iSimpleData.User.Value.FavouriteFoods);
                        break;
                }
            };

            // Act
            var result = changeList.Submit(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(error);
            var userData = simpleData.User.Value;
            Assert.AreEqual("Updated User", userData.Name);
            Assert.AreEqual(30, userData.Age);
            Assert.IsNotNull(userData.FavouriteFoods);
            Assert.AreEqual(2, userData.FavouriteFoods.Count);
            Assert.Contains("Sushi", userData.FavouriteFoods);
            Assert.Contains("Tacos", userData.FavouriteFoods);
        }

        [Test]
        public void ApplyChangeList_ShouldTriggerOnPropertyChanged()
        {
            // Arrange
            var simpleData = new TableData();
            var changeList = simpleData.PropertyUser.CreateChangeList("TestChangeList");
            changeList.AppendChange(nameof(UserData.Name), "Updated User");
            changeList.AppendChange(nameof(UserData.Age), 30);
            changeList.AppendChange(nameof(UserData.FavouriteFoods), new List<string> { "Sushi", "Tacos" });

            var propertyChangedEvents = new List<string>();
            simpleData.PropertyUser.OnPropertyChanged += propertyName => propertyChangedEvents.Add(propertyName);

            // Act
            var result = changeList.Submit(out var error);

            // Assert
            Assert.IsTrue(result);
            Assert.IsEmpty(error);
            Assert.Contains(nameof(UserData.Name), propertyChangedEvents);
            Assert.Contains(nameof(UserData.Age), propertyChangedEvents);
            Assert.Contains(nameof(UserData.FavouriteFoods), propertyChangedEvents);
        }

        [Test]
        public void ApplyChangeList_ShouldReturnErrorForInvalidProperty()
        {
            // Arrange
            var simpleData = new TableData();
            var changeList = simpleData.PropertyUser.CreateChangeList("TestChangeList");
            changeList.AppendChange("InvalidProperty", "SomeValue");

            // Act
            var result = changeList.Submit(out var error);

            // Assert
            Assert.IsFalse(result);
            Assert.IsNotEmpty(error);
        }
    }
}