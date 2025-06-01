using System.Collections.Generic;
using Assets.FusionGame.Core.Runtime.Data;
using FusionGame.Core.Data;

namespace FusionGame.Core.RuntimeTests
{
    public class UserData : IUserData
    {
        public string Name { get; set; } = "Default User Name";
        public int Age { get; set; } = 0;
        public List<string> FavouriteFoods { get; set; } = new List<string>();
    }

    public interface IUserData
    {
        string Name { get; }
        int Age { get; }
        List<string> FavouriteFoods { get; }
    }

    public interface IReadOnlyUserData
    {
        string Name { get; }
        int Age { get; }
        List<string> FavouriteFoods { get; }
    }

    public interface ITableData
    {
        IPrimitiveDataProperty<int> TableId { get; }
        IPrimitiveDataProperty<int> BettingTimeInSecond { get; }
        IStringDataProperty TableName { get; }
        IStringDataProperty TableTitle { get; }
        IPrimitiveDataProperty<bool> IsLiveDealer { get; }
        IPrimitiveDataProperty<bool> IsOffline { get; }
        //internal IComplexDataProperty<UserData> User { get; }
        IComplexDataProperty<IReadOnlyUserData> ReadOnlyUser { get; }
        //TODO: see how to access User with interface so that it doesn't allow modifying the user
    }

    public class TableData : ITableData
    {
        public readonly PrimitiveDataProperty<int> PropertyTableId = new PrimitiveDataProperty<int>();
        public readonly PrimitiveDataProperty<int> PropertyBettingTimeInSecond = new PrimitiveDataProperty<int>(5);
        public readonly StringDataProperty PropertyTableName = new StringDataProperty();
        public readonly StringDataProperty PropertyTableTitle = new StringDataProperty("Initial table title");
        public readonly PrimitiveDataProperty<bool> PropertyIsOffline = new PrimitiveDataProperty<bool>();
        public readonly PrimitiveDataProperty<bool> PropertyIsLiveDealer = new PrimitiveDataProperty<bool>(true);
        public readonly ComplexDataProperty<UserData> PropertyUser = new ComplexDataProperty<UserData>(new UserData());

        public IPrimitiveDataProperty<int> TableId => PropertyTableId;
        public IPrimitiveDataProperty<int> BettingTimeInSecond => PropertyBettingTimeInSecond;
        public IStringDataProperty TableName => PropertyTableName;
        public IStringDataProperty TableTitle => PropertyTableTitle;
        public IPrimitiveDataProperty<bool> IsLiveDealer => PropertyIsLiveDealer;
        public IPrimitiveDataProperty<bool> IsOffline => PropertyIsOffline;
        public IComplexDataProperty<IReadOnlyUserData> ReadOnlyUser => PropertyUser as IComplexDataProperty<IReadOnlyUserData>;
    }
}