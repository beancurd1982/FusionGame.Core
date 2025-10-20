using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.FusionGame.Core.Runtime.Data;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;

namespace Assets.FusionGame.Core.Tests.Runtime
{
    enum EnumSex
    {
        Unknown = 0,
        Male,
        Female,
    }

    interface IMyData: IDestroyable<IMyData>
    {
        IGetValueStrong<EnumSex> Sex { get; }
        IGetValueStrong<int> Year { get; }
        IGetValueStrong<float> Price { get; }
        IGetValue<IReadOnlyList<int>> BetPositions { get; }
        IGetValue<IReadOnlyDictionary<int, string>> StudentsMap { get; }
        IGetValue<IReadOnlyCollection<string>> Fruits { get; }
        IConfig Config { get; }
    }

    interface IConfig
    {
        IGetValueStrong<int> Time { get; }
        IGetValueStrong<bool> IsLiveDealer { get; }
        IGetValue<IReadOnlyList<uint>> ConnectedCentralDisplayIds { get; }
    }

    class MyData : IMyData
    {
        IGetValueStrong<EnumSex> IMyData.Sex => DataPropertySex;
        IGetValueStrong<int> IMyData.Year => DataPropertyYear;
        IGetValueStrong<float> IMyData.Price => DataPropertyPrice;
        IGetValue<IReadOnlyList<int>> IMyData.BetPositions => DataPropertyBetPositions;
        IGetValue<IReadOnlyDictionary<int, string>> IMyData.StudentsMap => DataPropertyStudentsMap;
        IGetValue<IReadOnlyCollection<string>> IMyData.Fruits => DataPropertyFruits;
        IConfig IMyData.Config => DataPropertyConfig;

        public EnumValue<EnumSex> DataPropertySex { get; } = new EnumValue<EnumSex>(EnumSex.Unknown);
        public IntValue DataPropertyYear { get; } = new IntValue();
        public FloatValue DataPropertyPrice { get; } = new FloatValue();
        public ListValue<int> DataPropertyBetPositions { get; } = new ListValue<int>();
        public DictionaryValue<int, string> DataPropertyStudentsMap { get; } = new DictionaryValue<int, string>();
        public HashSetValue<string> DataPropertyFruits { get; } = new HashSetValue<string>();
        public Config DataPropertyConfig { get; } = new Config();

        public Action<IMyData> OnBeforeDestroy { get; set; }

        public void Destroy()
        {
            OnBeforeDestroy?.Invoke(this);
        }
    }

    class Config : IConfig
    {
        IGetValueStrong<int> IConfig.Time => DataPropertyTime;
        IGetValueStrong<bool> IConfig.IsLiveDealer => DataPropertyIsLiveDealer;
        IGetValue<IReadOnlyList<uint>> IConfig.ConnectedCentralDisplayIds => DataPropertyConnectedCentralDisplayIds;

        public IntValue DataPropertyTime { get; } = new IntValue();
        public BooleanValue DataPropertyIsLiveDealer { get; } = new BooleanValue();
        public ListValue<uint> DataPropertyConnectedCentralDisplayIds { get; } = new ListValue<uint>();
    }

    public class TestDataInterfaces : MonoBehaviour
    {
        private IMyData iData;

        public IEnumerator RunDataTest()
        {
            var data = new MyData();
            iData = data;
            data.DataPropertyYear.SetValue(2025);
            data.DataPropertyPrice.SetValue(100.0f);
            Debug.Log("Start Test");

            // Assert initial values
            Assert.AreEqual(2025, data.DataPropertyYear.Value);
            Assert.AreEqual(100.0f, data.DataPropertyPrice.Value);

            yield return new WaitForSeconds(2f);

            SetupDataModifiedHandler(data);
            ModifyData(data);

            // Assert after modification
            Assert.AreEqual(EnumSex.Female, data.DataPropertySex.Value);
            Assert.AreEqual(2026, data.DataPropertyYear.Value);
            Assert.AreEqual(200.0f, data.DataPropertyPrice.Value);
            CollectionAssert.AreEqual(new List<int> { 1, 3 }, data.DataPropertyBetPositions.Value);
            Assert.IsTrue(data.DataPropertyStudentsMap.ContainsKey(1));
            Assert.IsFalse(data.DataPropertyStudentsMap.ContainsKey(2));
            Assert.AreEqual("Charlie Brown", data.DataPropertyStudentsMap[3]);
            CollectionAssert.AreEquivalent(new[] { "Apple", "Cherry" }, data.DataPropertyFruits.Value);

            yield return new WaitForSeconds(10f);

            data.Destroy();
            iData = null;
        }

        private void OnDestroy()
        {
            if (iData != null)
            {
                ClearDataModifiedHandler(iData);
            }
        }

        private static void ModifyData(MyData data)
        {
            // Set some initial values
            data.DataPropertySex.SetValue(EnumSex.Female);
            data.DataPropertyYear.SetValue(2026);
            data.DataPropertyPrice.SetValue(200.0f);

            // Assert value changes
            Assert.AreEqual(EnumSex.Female, data.DataPropertySex.Value);
            Assert.AreEqual(2026, data.DataPropertyYear.Value);
            Assert.AreEqual(200.0f, data.DataPropertyPrice.Value);

            // Add some bet positions and remove some
            data.DataPropertyBetPositions.Add(1);
            data.DataPropertyBetPositions.Add(2);
            data.DataPropertyBetPositions.Add(3);
            data.DataPropertyBetPositions.Remove(0);
            data.DataPropertyBetPositions.Remove(2);

            // Assert bet positions
            CollectionAssert.AreEqual(new List<int> { 1, 3 }, data.DataPropertyBetPositions.Value);

            // Add some students to the map
            data.DataPropertyStudentsMap.Add(1, "Alice");
            data.DataPropertyStudentsMap.Add(2, "Bob");
            data.DataPropertyStudentsMap.Add(3, "Charlie");
            // Remove a student
            data.DataPropertyStudentsMap.Remove(2);
            // Update a student
            data.DataPropertyStudentsMap[3] = "Charlie Brown";

            // Assert students map
            Assert.IsTrue(data.DataPropertyStudentsMap.ContainsKey(1));
            Assert.IsFalse(data.DataPropertyStudentsMap.ContainsKey(2));
            Assert.AreEqual("Charlie Brown", data.DataPropertyStudentsMap[3]);

            // Add some fruits
            data.DataPropertyFruits.Add("Apple");
            data.DataPropertyFruits.Add("Banana");
            data.DataPropertyFruits.Add("Cherry");
            // Remove a fruit
            data.DataPropertyFruits.Remove("Banana");

            // Assert fruits
            CollectionAssert.AreEquivalent(new[] { "Apple", "Cherry" }, data.DataPropertyFruits.Value);

            // Modify config properties
            data.DataPropertyConfig.DataPropertyTime.SetValue(0);
            data.DataPropertyConfig.DataPropertyIsLiveDealer.SetValue(false);
            data.DataPropertyConfig.DataPropertyTime.SetValue(0);
            data.DataPropertyConfig.DataPropertyIsLiveDealer.SetValue(false);
            data.DataPropertyConfig.DataPropertyTime.SetValue(10);
            data.DataPropertyConfig.DataPropertyIsLiveDealer.SetValue(true);
            data.DataPropertyConfig.DataPropertyConnectedCentralDisplayIds.Add(1001);
            data.DataPropertyConfig.DataPropertyConnectedCentralDisplayIds.Add(1002);
            data.DataPropertyConfig.DataPropertyConnectedCentralDisplayIds.Add(1003);
            data.DataPropertyConfig.DataPropertyConnectedCentralDisplayIds.Remove(1001);

            // Assert config properties
            Assert.AreEqual(10, data.DataPropertyConfig.DataPropertyTime.Value);
            Assert.IsTrue(data.DataPropertyConfig.DataPropertyIsLiveDealer.Value);
            CollectionAssert.AreEquivalent(new[] { 1002u, 1003u }, data.DataPropertyConfig.DataPropertyConnectedCentralDisplayIds.Value);
        }

        private void SetupDataModifiedHandler(IMyData data)
        {
            data.Sex.OnValueChanged += OnSexValueChanged;
            data.Year.OnValueChanged += OnYearValueChanged;
            data.Price.OnValueChanged += OnPriceValueChanged;
            data.BetPositions.OnValueChanged += OnBetPositionsValueChanged;
            data.StudentsMap.OnValueChanged += OnStudentsMapValueChanged;
            data.Fruits.OnValueChanged += OnFruitsValueChanged;
            data.Config.Time.OnValueChanged += OnTimerValueChanged;
            data.Config.IsLiveDealer.OnValueChanged += OnIsLiveDealerValueChanged;
            data.Config.ConnectedCentralDisplayIds.OnValueChanged += OnConnectedCentralDisplayIdsValueChanged;

            data.OnBeforeDestroy += OnBeforeDataDestroy;

            // Assert handlers are assigned
            Assert.IsNotNull(data.Sex.OnValueChanged);
            Assert.IsNotNull(data.Year.OnValueChanged);
            Assert.IsNotNull(data.Price.OnValueChanged);
            Assert.IsNotNull(data.BetPositions.OnValueChanged);
            Assert.IsNotNull(data.StudentsMap.OnValueChanged);
            Assert.IsNotNull(data.Fruits.OnValueChanged);
            Assert.IsNotNull(data.Config.Time.OnValueChanged);
            Assert.IsNotNull(data.Config.IsLiveDealer.OnValueChanged);
            Assert.IsNotNull(data.Config.ConnectedCentralDisplayIds.OnValueChanged);
            Assert.IsNotNull(data.OnBeforeDestroy);
        }

        private void ClearDataModifiedHandler(IMyData data)
        {
            data.Sex.OnValueChanged -= OnSexValueChanged;
            data.Year.OnValueChanged -= OnYearValueChanged;
            data.Price.OnValueChanged -= OnPriceValueChanged;
            data.BetPositions.OnValueChanged -= OnBetPositionsValueChanged;
            data.StudentsMap.OnValueChanged -= OnStudentsMapValueChanged;
            data.Fruits.OnValueChanged -= OnFruitsValueChanged;
            data.Config.Time.OnValueChanged -= OnTimerValueChanged;
            data.Config.IsLiveDealer.OnValueChanged -= OnIsLiveDealerValueChanged;
            data.Config.ConnectedCentralDisplayIds.OnValueChanged -= OnConnectedCentralDisplayIdsValueChanged;

            data.OnBeforeDestroy -= OnBeforeDataDestroy;

            // Assert handlers are unassigned (null or empty invocation list)
            Assert.IsTrue(data.Sex.OnValueChanged == null);
            Assert.IsTrue(data.Year.OnValueChanged == null );
            Assert.IsTrue(data.Price.OnValueChanged == null);
            Assert.IsTrue(data.BetPositions.OnValueChanged == null);
            Assert.IsTrue(data.StudentsMap.OnValueChanged == null);
            Assert.IsTrue(data.Fruits.OnValueChanged == null);
            Assert.IsTrue(data.Config.Time.OnValueChanged == null);
            Assert.IsTrue(data.Config.IsLiveDealer.OnValueChanged == null);
            Assert.IsTrue(data.Config.ConnectedCentralDisplayIds.OnValueChanged == null);
            Assert.IsTrue(data.OnBeforeDestroy == null);
        }

        private static void OnSexValueChanged(bool firstTimeSet, EnumSex oldValue, EnumSex value)
        {
            Debug.Log($"Sex changed to: {value}");
        }

        private static void OnYearValueChanged(bool firstTimeSet, int oldValue, int value)
        {
            Debug.Log($"Year changed to: {value}");
        }

        private static void OnPriceValueChanged(bool firstTimeSet, float oldValue, float value)
        {
            Debug.Log($"Price changed to: {value}");
        }

        private static void OnBetPositionsValueChanged(IReadOnlyList<int> value)
        {
            Debug.Log($"BetPositions changed to: {string.Join(", ", value)}");
        }

        private static void OnStudentsMapValueChanged(IReadOnlyDictionary<int, string> value)
        {
            Debug.Log($"StudentsMap changed to: {string.Join(", ", value)}");
        }

        private static void OnFruitsValueChanged(IReadOnlyCollection<string> value)
        {
            Debug.Log($"Fruits changed to: {string.Join(", ", value)}");
        }

        private static void OnTimerValueChanged(bool firstTimeSet, int oldValue, int value)
        {
            Debug.Log($"Config Time changed to: {value}");
        }

        private static void OnIsLiveDealerValueChanged(bool firstTimeSet, bool oldValue, bool value)
        {
            Debug.Log($"Config IsLiveDealer changed to: {value}");
        }

        private static void OnConnectedCentralDisplayIdsValueChanged(IReadOnlyList<uint> value)
        {
            Debug.Log($"Config ConnectedCentralDisplayIds changed to: {string.Join(", ", value)}");
        }

        private void OnBeforeDataDestroy(IMyData data)
        {
            ClearDataModifiedHandler(data);
            Assert.Pass("OnBeforeDataDestroy called and handlers cleared.");
        }
    }

    public class TestDataInterfacesPlayModeTest
    {
        [UnityTest]
        public IEnumerator TestDataInterfaces_RunDataTest_Completes()
        {
            var go = new GameObject("TestDataInterfaces");
            var testComponent = go.AddComponent<TestDataInterfaces>();

            yield return testComponent.RunDataTest();

            UnityEngine.Object.Destroy(go);
        }
    }
}
