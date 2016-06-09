// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.Text;

namespace Cake.Common.Text
{
    /// <summary>
    /// Contains extension methods for <see cref="TextTransformation{TTemplate}"/>.
    /// </summary>
    public static class TextTransformationExtensions
    {
        /// <summary>
        /// Registers a key and a value to be used with the
        /// text transformation.
        /// </summary>
        /// <typeparam name="TTemplate">The text transformation template.</typeparam>
        /// <param name="transformation">The text transformation.</param>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The same <see cref="TextTransformation{TTemplate}" /> instance so that multiple calls can be chained.
        /// </returns>
        public static TextTransformation<TTemplate> WithToken<TTemplate>(
            this TextTransformation<TTemplate> transformation, string key, object value)
            where TTemplate : class, ITextTransformationTemplate
        {
            if (transformation != null)
            {
                transformation.Template.Register(key, value);
            }
            return transformation;
        }
    }
}
