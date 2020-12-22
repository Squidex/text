// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Squidex.Text.Translations
{
    public interface ITranslator
    {
        /// <summary>
        /// Translates multiple texts.
        /// </summary>
        /// <param name="texts">The texts to translated. Cannot be null.</param>
        /// <param name="targetLanguage">The target language. Cannot be null.</param>
        /// <param name="sourceLanguage">The source language. If not defined the translator tries to detect it.</param>
        /// <param name="ct">The cancellation token to abort the request to the external service.</param>
        /// <returns>
        /// The result of the translation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="texts"/> is null.
        /// - or -
        /// <paramref name="targetLanguage"/> is null.
        /// </exception>
        Task<IReadOnlyList<TranslationResult>> TranslateAsync(IEnumerable<string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default);

        /// <summary>
        /// Translates a single text.
        /// </summary>
        /// <param name="text">The text to translated. Cannot be null.</param>
        /// <param name="targetLanguage">The target language. Cannot be null.</param>
        /// <param name="sourceLanguage">The source language. If not defined the translator tries to detect it.</param>
        /// <param name="ct">The cancellation token to abort the request to the external service.</param>
        /// <returns>
        /// The result of the translation.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">
        /// <paramref name="text"/> is null.
        /// - or -
        /// <paramref name="targetLanguage"/> is null.
        /// </exception>
        Task<TranslationResult> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default);
    }
}
