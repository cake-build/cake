﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Xunit.Sdk
{
    /// <summary>
    /// Default implementation of <see cref="IEqualityComparer{T}"/> used by the xUnit.net equality assertions.
    /// </summary>
    /// <typeparam name="T">The type that is being compared.</typeparam>
    internal class AssertEqualityComparer<T> : IEqualityComparer<T>
    {
        static readonly IEqualityComparer DefaultInnerComparer = new AssertEqualityComparerAdapter<object>(new AssertEqualityComparer<object>());
        static readonly TypeInfo NullableTypeInfo = typeof(Nullable<>).GetTypeInfo();

        readonly Func<IEqualityComparer> innerComparerFactory;
        readonly bool skipTypeCheck;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssertEqualityComparer{T}" /> class.
        /// </summary>
        /// <param name="skipTypeCheck">Set to <c>true</c> to skip type equality checks.</param>
        /// <param name="innerComparer">The inner comparer to be used when the compared objects are enumerable.</param>
        public AssertEqualityComparer(bool skipTypeCheck = false, IEqualityComparer innerComparer = null)
        {
            this.skipTypeCheck = skipTypeCheck;

            // Use a thunk to delay evaluation of DefaultInnerComparer
            innerComparerFactory = () => innerComparer ?? DefaultInnerComparer;
        }

        /// <inheritdoc/>
        public bool Equals(T x, T y)
        {
            var typeInfo = typeof(T).GetTypeInfo();

            // Null?
            if (!typeInfo.IsValueType || (typeInfo.IsGenericType && typeInfo.GetGenericTypeDefinition().GetTypeInfo().IsAssignableFrom(NullableTypeInfo)))
            {
                if (Object.Equals(x, default(T)))
                    return Object.Equals(y, default(T));

                if (Object.Equals(y, default(T)))
                    return false;
            }

            // Same type?
            if (!skipTypeCheck && x.GetType() != y.GetType())
                return false;

            // Implements IEquatable<T>?
            var equatable = x as IEquatable<T>;
            if (equatable != null)
                return equatable.Equals(y);

            // Implements IComparable<T>?
            var comparableGeneric = x as IComparable<T>;
            if (comparableGeneric != null)
                return comparableGeneric.CompareTo(y) == 0;

            // Implements IComparable?
            var comparable = x as IComparable;
            if (comparable != null)
                return comparable.CompareTo(y) == 0;

            // Dictionaries?
            var dictionariesEqual = CheckIfDictionariesAreEqual(x, y);
            if (dictionariesEqual.HasValue)
                return dictionariesEqual.GetValueOrDefault();

            // Enumerable?
            var enumerablesEqual = CheckIfEnumerablesAreEqual(x, y);
            if (enumerablesEqual.HasValue)
                return enumerablesEqual.GetValueOrDefault();

            // Last case, rely on Object.Equals
            return Object.Equals(x, y);
        }

        private bool? CheckIfEnumerablesAreEqual(T x, T y)
        {
            var enumerableX = x as IEnumerable;
            var enumerableY = y as IEnumerable;

            if (enumerableX == null || enumerableY == null)
                return null;

            var enumeratorX = enumerableX.GetEnumerator();
            var enumeratorY = enumerableY.GetEnumerator();
            var equalityComparer = innerComparerFactory();

            while (true)
            {
                var hasNextX = enumeratorX.MoveNext();
                var hasNextY = enumeratorY.MoveNext();

                if (!hasNextX || !hasNextY)
                    return hasNextX == hasNextY;

                if (!equalityComparer.Equals(enumeratorX.Current, enumeratorY.Current))
                    return false;
            }
        }

        private bool? CheckIfDictionariesAreEqual(T x, T y)
        {
            var dictionaryX = x as IDictionary;
            var dictionaryY = y as IDictionary;

            if (dictionaryX == null || dictionaryY == null)
                return null;

            if (dictionaryX.Count != dictionaryY.Count)
                return false;

            var equalityComparer = innerComparerFactory();
            var dictionaryYKeys = new HashSet<object>(dictionaryY.Keys.Cast<object>());

            foreach (var key in dictionaryX.Keys)
            {
                if (!dictionaryYKeys.Contains(key))
                    return false;

                var valueX = dictionaryX[key];
                var valueY = dictionaryY[key];

                if (!equalityComparer.Equals(valueX, valueY))
                    return false;

                dictionaryYKeys.Remove(key);
            }

            return dictionaryYKeys.Count == 0;
        }

        /// <inheritdoc/>
        public int GetHashCode(T obj)
        {
            throw new NotImplementedException();
        }
    }
}