using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Manages processor values.
    /// </summary>
    public sealed class ProcessorValues : IEnumerable<KeyValuePair<Type, IEnumerable<object>>>
    {
        private readonly Dictionary<Type, IEnumerable<object>> _values;

        /// <summary>
        /// Responsible for managing the <see cref="IProcessorExtension"/> values.
        /// </summary>
        public ProcessorValues()
        {
            _values = new Dictionary<Type, IEnumerable<object>>();
        }

        /// <summary>
        /// Responsible for managing the <see cref="IProcessorExtension"/> values.
        /// </summary>
        /// <param name="enumerable">the values to set</param>
        /// <exception cref="ArgumentNullException"><paramref name="enumerable" /> is null. </exception>
        public ProcessorValues(IEnumerable<KeyValuePair<Type, IEnumerable<object>>> enumerable)
        {
            if (enumerable == null)
            {
                throw new ArgumentException("Key cannot be null.", "enumerable");
            }

            _values = enumerable.ToDictionary(x => x.Key, x => x.Value);
        }

        /// <summary>
        /// Add a value
        /// </summary>
        /// <param name="key">The <see cref="IProcessorExtension"/></param>
        /// <param name="value">The value to set</param>
        public void Add(ILineProcessor key, object value)
        {
            var type = key.GetType();
            // Check if this IProcessorExtension already exists in the list.
            if (!_values.ContainsKey(type))
            {
                _values.Add(type, new List<object> { value });
                return;
            }
            
            // Add the value to existing list
            ((List<object>)_values[type]).Add(value);
        }

        /// <summary>
        /// Get the enumeratable
        /// </summary>
        /// <returns></returns>
        public IEnumerator<KeyValuePair<Type, IEnumerable<object>>> GetEnumerator()
        {
            return _values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
