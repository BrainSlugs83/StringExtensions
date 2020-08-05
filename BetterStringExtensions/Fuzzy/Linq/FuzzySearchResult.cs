using System;
using System.Collections.Generic;
using System.Text;

namespace System.Fuzzy.Linq
{
    /// <summary>
    /// Represents an individual, strong typed, fuzzy search result.
    /// </summary>
    /// <seealso cref="FuzzyCollectionExtensions.FuzzySearch" />
    public sealed class FuzzySearchResult<T>
    {
        /// <summary>
        /// Gets the index of this search result (relative to it's position in the original enumeration).
        /// </summary>
        public int Index { get; internal set; }

        /// <summary>
        /// Gets the value of this search result.
        /// </summary>
        public T Value { get; internal set; }

        /// <summary>
        /// Gets the score of the search result.
        /// </summary>
        public double Score { get; internal set; }
    }
}