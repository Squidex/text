// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Markdig;
using Markdig.Extensions.SelfPipeline;
using Markdig.Parsers;
using Markdig.Renderers.Normalize;

namespace Squidex.Text.Translations.Markdown
{
    public static class MarkdownExtensions
    {
        public static async Task<TranslationResult> TranslateMarkdownAsync(this ITranslator translator, string text, string targetLanguage, string sourceLanguage = null, MarkdownPipeline pipeline = null, CancellationToken ct = default)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            pipeline ??= new MarkdownPipelineBuilder().Build();
            pipeline = CheckForSelfPipeline(pipeline, text);

            var translationRenderer = new TranslationRenderer();

            pipeline.Setup(translationRenderer);

            var document = MarkdownParser.Parse(text, pipeline, null);

            translationRenderer.Pass = TranslationRenderer.RenderPass.Scan;
            translationRenderer.Render(document);

            if (translationRenderer.TextsToTranslate.Count == 0)
            {
                return new TranslationResult(text, sourceLanguage);
            }

            await translator.TranslateUniqueAsync(translationRenderer.TextsToTranslate, targetLanguage, sourceLanguage, ct);

            translationRenderer.Pass = TranslationRenderer.RenderPass.Replace;
            translationRenderer.Render(document);

            var textWriter = new StringWriter();
            var textRenderer = new NormalizeRenderer(textWriter);

            pipeline.Setup(textRenderer);

            textRenderer.Render(document);
            textWriter.Flush();

            return new TranslationResult(textWriter.ToString(), sourceLanguage);
        }

        private static MarkdownPipeline CheckForSelfPipeline(MarkdownPipeline pipeline, string markdown)
        {
            var selfPipeline = pipeline.Extensions.Find<SelfPipelineExtension>();

            if (selfPipeline != null)
            {
                return selfPipeline.CreatePipelineFromInput(markdown);
            }

            return pipeline;
        }
    }
}
