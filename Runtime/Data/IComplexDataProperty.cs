using System;

namespace FusionGame.Core.Data
{
    public interface IComplexDataProperty<T> where T : class
    {
        //TODO: add hasSet for each property in the complex data
        //bool HasSet { get; }

        T Value { get; }

        event Action<string> OnPropertyChanged;
    }
}
