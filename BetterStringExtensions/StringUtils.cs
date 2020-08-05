using System;
using System.Collections.Generic;
using System.Fuzzy.Linq;
using System.Linq;
using System.Text;

namespace System
{
    /// <summary>
    /// Utilities for working with strings.
    /// </summary>
    public static class StringUtils
    {
        /// <summary>
        /// Computes the distance between two string values.
        /// </summary>
        public static int ComputeLevenshteinDistance(string value1, string value2)
        {
            if (value1 is null) { throw new ArgumentNullException(nameof(value1)); }
            if (value2 is null) { throw new ArgumentNullException(nameof(value2)); }

            int n = value1.Length;
            int m = value2.Length;
            int[,] d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0)
            {
                return m;
            }

            if (m == 0)
            {
                return n;
            }

            // Step 2
            for (int i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (int j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (int i = 1; i <= n; i++)
            {
                //Step 4
                for (int j = 1; j <= m; j++)
                {
                    // Step 5
                    int cost = (value2[j - 1] == value1[i - 1]) ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }
            // Step 7
            return d[n, m];
        }

        internal static string Simplify(string input)
        {
            return new string
            (
                (input ?? string.Empty).Where
                (
                    c => char.IsLetterOrDigit(c)
                )
                .ToArray()
            )
            .ToUpperInvariant();
        }

        internal static double Score(string a, string b)
        {
            a = Simplify(a);
            b = Simplify(b);

            if (a == b) { return double.MaxValue; }

            double score = ComputeLevenshteinDistance(a, b);
            double max = (a.Length + b.Length) / 2d;

            double low = Math.Min(a.Length, b.Length);
            double high = Math.Max(a.Length, b.Length);
            var mod = Math.Max(low / high, 2d / 3d);

            score = Math.Min(Math.Max((max - (score * mod)) / max, 0d), 1d);

            return score;
        }

        /// <summary>
        /// Parses a string to the closest enum (if any are applicable).
        /// </summary>
        public static T? ParseEnum<T>(string input) where T : struct
        {
            if (Enum.TryParse(input, true, out T result))
            {
                return result;
            }

            // for if they typed a period or something.
            input = Enum.GetNames(typeof(T)).FuzzySearch(input).FirstOrDefault()?.Value;

            T? returnResult = null;
            if (!string.IsNullOrEmpty(input))
            {
                if (Enum.TryParse(input, true, out result))
                {
                    returnResult = result;
                }
            }

            return returnResult;
        }
    }
}