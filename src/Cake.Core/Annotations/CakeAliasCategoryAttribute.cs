using System;

namespace Cake.Core.Annotations
{
    /// <summary>
    /// An attribute used for documentation of alias methods/properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = false)]
    public sealed class CakeAliasCategoryAttribute : Attribute
    {
        private readonly string _name;

        /// <summary>
        /// Gets the category name.
        /// </summary>
        /// <value>The category name.</value>
        public string Name
        {
            get { return _name; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeAliasCategoryAttribute"/> class.
        /// </summary>
        /// <param name="name">The category name.</param>
        public CakeAliasCategoryAttribute(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _name = name;
        }
    }
}