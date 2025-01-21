using System;
using System.Collections.Generic;

namespace FusionGame.Core.Data
{
    public interface IComplexDataPropertyChangeList
    {
        /// <summary>
        /// The name of the change list.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Append change to the change list.
        /// </summary>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        /// <returns></returns>
        IComplexDataPropertyChangeList AppendChange(string propertyName, object newValue);

        /// <summary>
        /// Submit the change list to apply the changes to the complex data property.
        /// </summary>
        /// <param name="error"></param>
        /// <returns>true if there is no error, false otherwise (the "error" contains the detail error message).</returns>
        bool Submit(out string error);

        /// <summary>
        /// Get all the changes in this change list.
        /// </summary>
        /// <returns></returns>
        IEnumerable<Tuple<string, object>> GetChanges();
    }
}
