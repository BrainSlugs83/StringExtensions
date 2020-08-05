using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System.Fuzzy.Linq
{
    /// <summary>
    /// Represents a collection of <see cref="FuzzySearchResult{T}" /> objects.
    /// </summary>
    /// <seealso cref="FuzzyCollectionExtensions.FuzzySearch" />
    public class FuzzySearchResultCollection<T> : List<FuzzySearchResult<T>>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FuzzySearchResultCollection{T}" /> class.
        /// </summary>
        [ExcludeFromCodeCoverage]
        public FuzzySearchResultCollection() { }

        /// <summary>
        /// Initializes a new instance of the <see cref="FuzzySearchResultCollection{T}" /> class.
        /// </summary>
        /// <param name="items">The items.</param>
        public FuzzySearchResultCollection(IEnumerable<FuzzySearchResult<T>> items)
        {
            if (!(items is null))
            {
                foreach (var item in items)
                {
                    this.Add(item);
                }
            }
        }
    }
}