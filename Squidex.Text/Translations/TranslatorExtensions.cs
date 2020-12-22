// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Squidex.Text.Translations
{
    internal static class TranslatorExtensions
    {
        public static async Task TranslateUniqueAsync(this ITranslator translator, Dictionary<string, string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default)
        {
            var translations = await translator.TranslateAsync(texts.Keys, targetLanguage, sourceLanguage, ct);

            var i = 0;
            foreach (var source in texts.Keys.ToList())
            {
                texts[source] = translations[i].Text ?? source;
            }
        }
    }
}
