using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace System.Fuzzy
{
    /// <summary>
    /// Extension Methods for Fuzzy String Operations.
    /// </summary>
    public static class FuzzyStringExtensions
    {
        /// <summary>
        /// Compares two strings, in simplified format, for aproximate equality using the computed
        /// Levenshtein Distance between them.
        /// </summary>
        /// <param name="value1">The first string.</param>
        /// <param name="value2">The second string.</param>
        /// <param name="minScore">
        /// A value between 0 and 1, which is used to specify the minimum equality threshold.
        /// </param>
        public static bool FuzzyEquals(this string value1, string value2, double minScore = (2d / 3d))
        {
            if (minScore < 0d || minScore > 1d) { throw new ArgumentOutOfRangeException(nameof(minScore)); }
            return StringUtils.Score(value1, value2) >= minScore;
        }
    }
}