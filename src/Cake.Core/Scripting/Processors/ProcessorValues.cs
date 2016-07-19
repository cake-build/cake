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
        /// Initializes a new instance of the <see cref="ProcessorValues" /> class.
        /// </summary>
        public ProcessorValues()
        {
            _values = new Dictionary<Type, IEnumerable<object>>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorValues" /> class.
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
        /// <param name="key">The line processor to add <paramref name="value"/> for.</param>
        /// <param name="value">The value to set</param>
        /// <exception cref="ArgumentException">Throws if the <paramref name="key"/> is null.</exception>
        public void Add(ILineProcessor key, object value)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var type = key.GetType();

            // Check if this IProcessorExtension already exists in the list.
            if (!_values.ContainsKey(type))
            {
                _values.Add(type, new List<object> { value });
                return;
            }

            // Add the value to existing list
            ((List<object>) _values[type]).Add(value);
        }

        /// <summary>
        /// Get the values for a <see cref="ILineProcessor"/>.
        /// </summary>
        /// <param name="key">The <see cref="ILineProcessor"/>.</param>
        /// <returns>A enumeratable with values as objects, or null if the <see cref="ILineProcessor"/> is not found.</returns>
        /// <exception cref="ArgumentException">Throws if the <paramref name="key"/> is null.</exception>
        public IEnumerable<object> Get(ILineProcessor key)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var type = key.GetType();
            if (_values.ContainsKey(type))
            {
                return _values[type];
            }

            return null;
        }

        /// <summary>
        /// Get the values for a <see cref="ILineProcessor"/> and cast it to T.
        /// </summary>
        /// <param name="key">The <see cref="ILineProcessor"/>.</param>
        /// <typeparam name="T">The type to cast to</typeparam>
        /// <returns>A enumeratable with values as objects, or null if the <see cref="ILineProcessor"/> is not found.</returns>
        /// <exception cref="ArgumentException">Throws if the <paramref name="key"/> is null.</exception>
        public IEnumerable<T> Get<T>(ILineProcessor key)
        {
            return Get(key).OfType<T>();
        }

        /// <summary>
        /// Try to get the values for a <see cref="ILineProcessor"/>.
        /// </summary>
        /// <param name="key">The <see cref="ILineProcessor"/>.</param>
        /// <param name="objects">A enumeratable with values as objects, or null if the <see cref="ILineProcessor"/> is not found.</param>
        /// <returns>True if <paramref name="key"/> is found in the <see cref="ProcessorValues"/>, else False</returns>
        /// <exception cref="ArgumentException">Throws if the <paramref name="key"/> is null.</exception>
        public bool TryGet(ILineProcessor key, out IEnumerable<object> objects)
        {
            if (key == null)
            {
                throw new ArgumentNullException("key");
            }

            var type = key.GetType();
            if (_values.ContainsKey(type))
            {
                objects = _values[type];
                return true;
            }

            objects = null;
            return false;
        }

        /// <summary>
        /// Get the enumeratable
        /// </summary>
        /// <returns>Enumeratable with processors and values.</returns>
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
