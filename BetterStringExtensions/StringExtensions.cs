using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace System
{
    /// <summary>
    /// Extension Methods for working with the <see cref="string">String</see> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Calls <see cref="string.IsNullOrEmpty(string)"/>.
        /// </summary>
        public static bool IsNullOrEmpty(this string input)
        {
            return string.IsNullOrEmpty(input);
        }

        /// <summary>
        /// Calls <see cref="string.IsNullOrWhiteSpace(string)" />.
        /// </summary>
        public static bool IsNullOrWhiteSpace(this string input)
        {
            return string.IsNullOrWhiteSpace(input);
        }

        /// <summary>
        /// Compares two <see cref="string"/>s for equality, ignoring case.
        /// </summary>
        [SuppressMessage("Microsoft.Globalization", "CA1309:UseOrdinalStringComparison",
            Justification = "Azure SQL Database comparisons were failing with Ordinal Culture.")]
        public static bool EqualsIgnoreCase(this string instance, string value)
        {
            return string.Equals(instance, value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a value indicating whether a specified substring value occurs within
        /// the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="substringValue">The substring value to seek.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="substringValue" /> parameter occurs within
        /// the <paramref name="input" /> string, or if the <paramref name="substringValue"
        /// /> is <see cref="string.Empty" />, or if both parameters are <c>null</c>;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsIgnoreCase(this string input, string substringValue)
        {
            if (input == null || substringValue == null) { return input == substringValue; }
            return input.IndexOf(substringValue, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// Returns a <see cref="string" /> composed of the characters which occur before
        /// the first match of the <paramref name="search" /> <see cref="string" />.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains 
        /// <paramref name="search" />, then the substring that occurs before the 
        /// match is returned.  Otherwise, <paramref name="input" /> is returned unmodified.
        /// </returns>
        public static string Before(this string input, string search, bool ignoreCase = false)
        {
            if (!search.IsNullOrEmpty())
            {
                var culture = ignoreCase
                    ? StringComparison.InvariantCultureIgnoreCase
                    : StringComparison.InvariantCulture;

                int idx = input?.IndexOf(search, culture) ?? -1;
                if (idx >= 0)
                {
                    input = input.Substring(0, idx);
                }
            }

            return input;
        }

        /// <summary>
        /// Returns a <see cref="string" /> composed of the characters which occur after
        /// the first match of the <paramref name="search" /> <see cref="string" />.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains <paramref name="search" />, then the
        /// substring that occurs after the match is returned.  Otherwise, 
        /// <paramref name="input" /> is returned unmodified.
        /// </returns>
        public static string After(this string input, string search, bool ignoreCase = false)
        {
            if (!search.IsNullOrEmpty())
            {
                var culture = ignoreCase
                    ? StringComparison.InvariantCultureIgnoreCase
                    : StringComparison.InvariantCulture;

                int idx = input?.IndexOf(search, culture) ?? -1;
                if (idx >= 0)
                {
                    input = input.Substring(idx + search.Length);
                }
            }

            return input;
        }

        private static string[] DecimalMeasurementSuffixes = new[]
        {
            "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

        private static string[] BinaryMeasurementSuffixes = new[]
        {
            "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
        };

        public static string ToHumanReadableFileSize(this long input, bool binary = true)
        {
            bool negative = false;
            if (input < 0) { input *= -1; negative = true; }

            double value = input;
            double divisor = binary ? 1024d : 1000d;
            int idx = 0;
            int maxIdx = binary
                ? BinaryMeasurementSuffixes.Length
                : DecimalMeasurementSuffixes.Length;

            while (value >= divisor && (idx + 1) < maxIdx)
            {
                value /= divisor;
                idx++;
            }

            return (negative ? "-" : string.Empty)
                + value.ToString("0.00") + " " +
                (
                    binary
                    ? BinaryMeasurementSuffixes[idx]
                    : DecimalMeasurementSuffixes[idx]
                );
        }
    }
}
