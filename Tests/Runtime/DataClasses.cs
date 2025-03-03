using System.Collections.Generic;
using FusionGame.Core.Data;

namespace FusionGame.Core.RuntimeTests
{
    public class UserData
    {
        public string Name { get; set; } = "Default User Name";
        public int Age { get; set; } = 0;
        public List<string> FavouriteFoods { get; set; } = new List<string>();
    }

    public interface ITableData
    {
        IAtomicDataProperty<int> TableId { get; }
        IAtomicDataProperty<int> BettingTimeInSecond { get; }
        IAtomicDataProperty<string> TableName { get; }
        IAtomicDataProperty<string> TableTitle { get; }
        IAtomicDataProperty<bool> IsLiveDealer { get; }
        IAtomicDataProperty<bool> IsOffline { get; }

        IComplexDataProperty<UserData> User { get; }
    }

    public class TableData : ITableData
    {
        public readonly AtomicDataProperty<int> PropertyTableId = new AtomicDataProperty<int>();
        public readonly AtomicDataProperty<int> PropertyBettingTimeInSecond = new AtomicDataProperty<int>(5);
        public readonly AtomicDataProperty<string> PropertyTableName = new AtomicDataProperty<string>();
        public readonly AtomicDataProperty<string> PropertyTableTitle = new AtomicDataProperty<string>("Initial Value");
        public readonly AtomicDataProperty<bool> PropertyIsOffline = new AtomicDataProperty<bool>();
        public readonly AtomicDataProperty<bool> PropertyIsLiveDealer = new AtomicDataProperty<bool>(true);

        public readonly ComplexDataProperty<UserData> PropertyUser = new ComplexDataProperty<UserData>(new UserData());
        public IAtomicDataProperty<int> TableId => PropertyTableId;
        public IAtomicDataProperty<int> BettingTimeInSecond => PropertyBettingTimeInSecond;
        public IAtomicDataProperty<string> TableName => PropertyTableName;
        public IAtomicDataProperty<string> TableTitle => PropertyTableTitle;
        public IAtomicDataProperty<bool> IsLiveDealer => PropertyIsLiveDealer;
        public IAtomicDataProperty<bool> IsOffline => PropertyIsOffline;
        public IComplexDataProperty<UserData> User => PropertyUser;
    }
}