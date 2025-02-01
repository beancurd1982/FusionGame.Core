using System;

namespace FusionGame.Core.Utils
{
    /// <summary>
    /// A generic class that represents a nullable enum without using built-in nullable types.
    /// </summary>
    /// <typeparam name="T">An enum type.</typeparam>
    public class NullableEnum<T> where T : Enum
    {
        private T _value;
        private bool _hasValue;

        /// <summary>
        /// Gets whether the enum has a valid value assigned.
        /// </summary>
        public bool HasValue => _hasValue;

        /// <summary>
        /// Gets or sets the value of the nullable enum.
        /// Setting a value makes HasValue true.
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if trying to get value when none is set.</exception>
        public T Value
        {
            get
            {
                if (!_hasValue)
                {
                    throw new InvalidOperationException("Cannot retrieve value because it has not been set.");
                }

                return _value;
            }
            set
            {
                _value = value;
                _hasValue = true;
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NullableEnum{T}"/> with no value set.
        /// </summary>
        public NullableEnum()
        {
            _hasValue = false;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="NullableEnum{T}"/> with a specified value.
        /// </summary>
        /// <param name="value">The enum value to initialize.</param>
        public NullableEnum(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Sets the enum value.
        /// </summary>
        /// <param name="value">The enum value to set.</param>
        public void SetValue(T value)
        {
            _value = value;
            _hasValue = true;
        }

        /// <summary>
        /// Clears the value, making the object "null".
        /// </summary>
        public void Clear()
        {
            _hasValue = false;
        }

        /// <summary>
        /// Returns the string representation of the current value.
        /// If no value is set, returns "null".
        /// </summary>
        /// <returns>A string representation of the stored value.</returns>
        public override string ToString()
        {
            return _hasValue ? _value.ToString() : "null";
        }
    }
}