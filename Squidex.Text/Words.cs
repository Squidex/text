// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;

namespace Squidex.Text
{
    /// <summary>
    /// Helper methods to deal with words.
    /// </summary>
    public static class Words
    {
        /// <summary>
        /// Counts the number of words in a text.
        /// </summary>
        /// <param name="text">The given text.</param>
        /// <returns>
        /// The number of found words in the text.
        /// </returns>
        public static int Count(string text)
        {
            if (text == null)
            {
                return 0;
            }

            return Count(text.AsSpan());
        }

        /// <summary>
        /// Counts the number of words in a text.
        /// </summary>
        /// <param name="text">The given text.</param>
        /// <returns>
        /// The number of found words in the text.
        /// </returns>
        public static int Count(ReadOnlySpan<char> text)
        {
            if (text.Length == 0)
            {
                return 0;
            }

            var result = 0;

            for (var i = 0; i < text.Length; i++)
            {
                var current = text[i];

                if (!char.IsWhiteSpace(current) && !char.IsPunctuation(current) && WordBoundary.IsBoundary(text, i))
                {
                    result++;
                }
            }

            return result;
        }
    }
}
