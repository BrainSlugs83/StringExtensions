using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Common Extension Methods for Collections.
    /// </summary>
    public static class CollectionExtensions
    {
        /// <summary>
        /// Determines whether a sequence of <c>string</c> s contains a specified value using a case
        /// insensitive string comparer.
        /// </summary>
        /// <param name="source">A sequence in which to locate a value.</param>
        /// <param name="value">The value to locate in the sequence.</param>
        /// <returns>
        /// <c>true</c> if the source sequence contains an element that has the specified value;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsIgnoreCase(this IEnumerable<string> source, string value)
        {
            return source?.Where(x => x.EqualsIgnoreCase(value)).Any() ?? false;
        }

        /// <summary>
        /// Returns distinct elements from a sequence by using the a case insensitive string
        /// comparer to compare values.
        /// </summary>
        /// <param name="source">The sequence to remove duplicate elements from.</param>
        /// <returns>
        /// An sequence of <c>string</c> s that contains distinct elements from the source sequence.
        /// </returns>
        public static IEnumerable<string> DistinctIgnoreCase(this IEnumerable<string> source)
        {
            return source?.Distinct(StringComparer.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Adds the sequence of <paramref name="elements" /> to the end of the specified <paramref
        /// name="collection" />.
        /// </summary>
        /// <typeparam name="TSource">The type of the source.</typeparam>
        /// <typeparam name="TElement">The type of the element.</typeparam>
        /// <param name="collection">The collection.</param>
        /// <param name="elements">The elements.</param>
        /// <returns>The original <paramref name="collection" />.</returns>
        public static ICollection<TSource> AddRange<TSource, TElement>
        (
            this ICollection<TSource> collection,
            IEnumerable<TElement> elements
        )
        where TElement : TSource
        {
            if (elements is { })
            {
                foreach (var item in elements)
                {
                    collection.Add(item);
                }
            }

            return collection;
        }

        /// <summary>
        /// Breaks a sequence of elements into batches of elements.
        /// </summary>
        /// <typeparam name="TSource">The type of elements in the source sequence.</typeparam>
        /// <param name="source">The sequence to be batched.</param>
        /// <param name="batchSize">The maximum size of each batch.</param>
        public static IEnumerable<ICollection<TSource>> BatchesOf<TSource>
        (
            this IEnumerable<TSource> source,
            int batchSize
        )
        {
            if (batchSize <= 1) { throw new ArgumentOutOfRangeException(nameof(batchSize)); }
            if (source is { })
            {
                var batch = new List<TSource>();
                foreach (var item in source)
                {
                    batch.Add(item);
                    if (batch.Count >= batchSize)
                    {
                        yield return batch;
                        batch = new List<TSource>();
                    }
                }

                if (batch.Any()) { yield return batch; }
            }
        }
    }
}