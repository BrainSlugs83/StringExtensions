using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using System.Text.RegularExpressions;

namespace System
{
    /// <summary>
    /// Extension Methods for working with the <see cref="string">String</see> class.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Calls <see cref="string.IsNullOrEmpty(string)" />.
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
        /// Compares two <see cref="string" /> s for equality, ignoring case.
        /// </summary>
        public static bool EqualsIgnoreCase(this string instance, string value)
        {
            return string.Equals(instance, value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a value indicating whether a specified substring value occurs within the input string.
        /// </summary>
        /// <param name="input">The input string.</param>
        /// <param name="substringValue">The substring value to seek.</param>
        /// <returns>
        /// <c>true</c> if the <paramref name="substringValue" /> parameter occurs within the
        /// <paramref name="input" /> string, or if the <paramref name="substringValue" /> is <see
        /// cref="string.Empty" />, or if both parameters are <c>null</c>; otherwise, <c>false</c>.
        /// </returns>
        public static bool ContainsIgnoreCase(this string input, string substringValue)
        {
            if (input == null || substringValue == null) { return input == substringValue; }
            return input.IndexOf(substringValue, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        /// <summary>
        /// Returns a <see cref="string" /> composed of the characters which occur before the first
        /// match of the <paramref name="search" /> string.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains <paramref name="search" />, then the substring
        /// that occurs before the match is returned. Otherwise, <paramref name="input" /> is
        /// returned unmodified.
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
        /// Returns a <see cref="string" /> composed of the characters which occur before the first
        /// case insensitive match of the <paramref name="search" /> string.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains <paramref name="search" />, then the substring
        /// that occurs before the match is returned. Otherwise, <paramref name="input" /> is
        /// returned unmodified.
        /// </returns>
        public static string BeforeIgnoreCase(this string input, string search) => input.Before(search, true);

        /// <summary>
        /// Determines whether the beginning of this string instance matches the specified string
        /// when compared using the <see cref="StringComparison.InvariantCultureIgnoreCase" />
        /// comparison option.
        /// </summary>
        /// <returns>
        /// <c>true</c> if this instance begins with <paramref name="value" />; otherwise, <c>false</c>.
        /// </returns>
        public static bool StartsWithIgnoreCase(this string input, string value)
        {
            return input.StartsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Determines whether the end of this string instance matches the specified string when
        /// compared using the <see cref="StringComparison.InvariantCultureIgnoreCase" /> comparison option.
        /// </summary>
        /// <returns>
        /// <c>true</c> if the <paramref name="value" /> parameter matches the end of this string;
        /// otherwise, <c>false</c>.
        /// </returns>
        public static bool EndsWithIgnoreCase(this string input, string value)
        {
            return input.EndsWith(value, StringComparison.InvariantCultureIgnoreCase);
        }

        /// <summary>
        /// Returns a <see cref="string" /> composed of the characters which occur after the first
        /// match of the <paramref name="search" /><see cref="string" />.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains <paramref name="search" />, then the substring
        /// that occurs after the match is returned. Otherwise, <paramref name="input" /> is
        /// returned unmodified.
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

        /// <summary>
        /// Returns a <see cref="string" /> composed of the characters which occur after the first
        /// case insensitive match of the <paramref name="search" /> string.
        /// </summary>
        /// <returns>
        /// If the <paramref name="input" /> contains <paramref name="search" />, then the substring
        /// that occurs after the match is returned. Otherwise, <paramref name="input" /> is
        /// returned unmodified.
        /// </returns>
        public static string AfterIgnoreCase(this string input, string search) => input.After(search, true);

        private static readonly string[] DecimalMeasurementSuffixes = new[]
        {
            "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB"
        };

        private static readonly string[] BinaryMeasurementSuffixes = new[]
        {
            "B", "KiB", "MiB", "GiB", "TiB", "PiB", "EiB", "ZiB", "YiB"
        };

        /// <summary>
        /// Converts this value from a byte count into a human readable file size.
        /// </summary>
        /// <param name="input">The total number of bytes.</param>
        /// <param name="binary">
        /// If <c>true</c> the result will be a binary file size (i.e. 1024 bytes = 1 KiB, 1024 KiB
        /// = 1 MiB, etc.); otherwise the result will be a decimal file size (i.e. 1000 bytes = 1
        /// KB; 1000 KB = 1 MB, etc.).
        /// </param>
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
                + value.ToString(idx > 0 ? "0.00" : "0") + " " +
                (
                    binary
                    ? BinaryMeasurementSuffixes[idx]
                    : DecimalMeasurementSuffixes[idx]
                );
        }

        private static readonly RegexOptions InvariantCultureIgnoreCaseRegexOptions =
            RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;

        /// <summary>
        /// Returns a new string in which all occurrences of a specified pattern in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="replaceCallback">
        /// Accepts the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, Func<string, string> replaceCallback)
        {
            return input.RegexReplace
            (
                pattern,
                InvariantCultureIgnoreCaseRegexOptions,
                replaceCallback
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified pattern in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="replaceCallback">
        /// Accepts the index and the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, Func<int, string, string> replaceCallback)
        {
            return input.RegexReplace
            (
                pattern,
                InvariantCultureIgnoreCaseRegexOptions,
                replaceCallback
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified pattern in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="options">Regex options to be applied to the pattern.</param>
        /// <param name="replaceCallback">
        /// Accepts the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, RegexOptions options, Func<string, string> replaceCallback)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (replaceCallback is null) { throw new ArgumentNullException(nameof(replaceCallback)); }

            return Regex.Replace
            (
                input,
                pattern ?? string.Empty,
                me => replaceCallback(me.Value),
                options
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified pattern in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="options">Regex options to be applied to the pattern.</param>
        /// <param name="replaceCallback">
        /// Accepts the index and the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, RegexOptions options, Func<int, string, string> replaceCallback)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }
            if (replaceCallback is null) { throw new ArgumentNullException(nameof(replaceCallback)); }

            return Regex.Replace
            (
                input,
                pattern ?? string.Empty,
                me => replaceCallback(me.Index, me.Value),
                options
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified pattern in the current
        /// instance are replaced with another specified string.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="pattern">The pattern to search for.</param>
        /// <param name="newValue">
        /// The string to replace all occurrences of <paramref name="pattern" /> with.
        /// </param>
        /// <param name="options">Regex options to be applied to the pattern.</param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string RegexReplace(this string input, string pattern, string newValue, RegexOptions? options = null)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return Regex.Replace
            (
                input,
                pattern ?? string.Empty,
                newValue ?? string.Empty,
                options ?? InvariantCultureIgnoreCaseRegexOptions
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current
        /// instance are replaced with another specified string.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="oldValue">The string to be replaced (case insensitive).</param>
        /// <param name="newValue">
        /// The string to replace all occurrences of <paramref name="oldValue" />
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string ReplaceIgnoreCase(this string input, string oldValue, string newValue)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.RegexReplace
            (
                Regex.Escape(oldValue ?? string.Empty),
                newValue ?? string.Empty,
                InvariantCultureIgnoreCaseRegexOptions
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="oldValue">The string to be replaced (case insensitive).</param>
        /// <param name="replaceCallback">
        /// Accepts the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string ReplaceIgnoreCase(this string input, string oldValue, Func<string, string> replaceCallback)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.RegexReplace
            (
                Regex.Escape(oldValue ?? string.Empty),
                replaceCallback
            );
        }

        /// <summary>
        /// Returns a new string in which all occurrences of a specified string in the current
        /// instance are replaced with another string specified via a callback.
        /// </summary>
        /// <param name="input">This string.</param>
        /// <param name="oldValue">The string to be replaced (case insensitive).</param>
        /// <param name="replaceCallback">
        /// Accepts the index and the value of the match, and returns the value of a new string.
        /// </param>
        /// <returns>
        /// A string that is equivalent to the current string except that all instances of oldValue
        /// are replaced with newValue. If oldValue is not found in the current instance, the method
        /// returns the current instance unchanged.
        /// </returns>
        public static string ReplaceIgnoreCase(this string input, string oldValue, Func<int, string, string> replaceCallback)
        {
            if (input is null) { throw new ArgumentNullException(nameof(input)); }

            return input.RegexReplace
            (
                Regex.Escape(oldValue ?? string.Empty),
                replaceCallback
            );
        }
    }
}