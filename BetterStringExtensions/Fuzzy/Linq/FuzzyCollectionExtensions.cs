using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Fuzzy.Linq
{
    /// <summary>
    /// Extension Methods for performing fuzzy operatons on generic enumerables within LINQ queries.
    /// </summary>
    public static class FuzzyCollectionExtensions
    {
        /// <summary>
        /// Searches an enumerable for instances which meet the criteria of a fuzzy comparison (see
        /// <see cref="FuzzyStringExtensions.FuzzyEquals" /> for more info), and returns an
        /// enumerable of search results, sorted with the best matches first.
        /// </summary>
        /// <param name="objects">The enumerable to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="converter">
        /// (Optional) a converter which will convert from <typeparamref name="T" /> to <see
        /// cref="string" />.
        /// </param>
        /// <returns>
        /// A filtered enumerable of items which are fuzzily equal (per the min score) to the
        /// <paramref name="value" /> parameter, sorted to put the best matches first.
        /// </returns>
        public static FuzzySearchResultCollection<T> FuzzySearch<T>(this IEnumerable<T> objects, string value, Func<T, string> converter = null)
        {
            return objects.FuzzySearch(value, (2d / 3d), converter);
        }

        /// <summary>
        /// Searches an enumerable for instances which meet the criteria of a fuzzy comparison (see
        /// <see cref="FuzzyStringExtensions.FuzzyEquals" /> for more info), and returns an
        /// enumerable of search results, sorted with the best matches first.
        /// </summary>
        /// <param name="objects">The enumerable to search.</param>
        /// <param name="value">The value to search for.</param>
        /// <param name="minScore">
        /// A value between 0 and 1, which is used to specify the minimum equality threshold for the
        /// fuzzy comparison.
        /// </param>
        /// <param name="converter">
        /// (Optional) a converter which will convert from <typeparamref name="T" /> to <see
        /// cref="string" />.
        /// </param>
        /// <returns>
        /// A filtered enumerable of items which are fuzzily equal (per the min score) to the
        /// <paramref name="value" /> parameter, sorted to put the best matches first.
        /// </returns>
        public static FuzzySearchResultCollection<T> FuzzySearch<T>(this IEnumerable<T> objects, string value, double minScore, Func<T, string> converter = null)
        {
            if (objects is null) { throw new ArgumentNullException(nameof(objects)); }
            if (minScore < 0d || minScore > 1d) { throw new ArgumentOutOfRangeException(nameof(minScore)); }

            if (converter is null)
            {
                converter = x =>
                {
                    return (x?.ToString()) ?? string.Empty;
                };
            }

            int idx = 0;
            var results =
            (
                from o in objects
                select new FuzzySearchResult<T>
                {
                    Value = o,
                    Index = idx++,
                    Score = StringUtils.Score(value, converter(o))
                }
            )
            .ToArray() // Solidify the indexes before sorting, etc.
            .Where(o => o.Score >= minScore)
            .OrderByDescending(o => o.Score);

            return new FuzzySearchResultCollection<T>(results);
        }
    }
}