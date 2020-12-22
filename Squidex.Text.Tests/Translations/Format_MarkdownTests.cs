// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Squidex.Text.Translations;
using Squidex.Text.Translations.Markdown;
using Xunit;

namespace Squidex.Text.Tests.Translations
{
    public class Format_MarkdownTests
    {
        private class DummyTranslator : ITranslator
        {
            public async Task<IReadOnlyList<TranslationResult>> TranslateAsync(IEnumerable<string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default)
            {
                await Task.Yield();

                var results = new List<TranslationResult>();

                foreach (var text in texts)
                {
                    results.Add(new TranslationResult($"{text}_t", sourceLanguage));
                }

                return results;
            }

            public async Task<TranslationResult> TranslateAsync(string text, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default)
            {
                await Task.Yield();

                return new TranslationResult($"{text}_t", sourceLanguage);
            }
        }

        public static IEnumerable<object[]> Tests()
        {
            yield return new object[] { string.Empty, string.Empty };
            yield return new object[] { "# Headline", "# Headline_t" };
        }

        [Theory]
        [MemberData(nameof(Tests))]
        public async Task Should_translate_markdown(string source, string expected)
        {
            var result = await new DummyTranslator().TranslateMarkdownAsync(source, "en", "de");

            Assert.Equal(expected, result.Text);
        }
    }
}
