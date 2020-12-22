// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Squidex.Text.Translations.Markdown
{
    public sealed class TranslationRenderer : RendererBase
    {
        private readonly Dictionary<string, string> texts = new Dictionary<string, string>();

        public Dictionary<string, string> TextsToTranslate => texts;

        public RenderPass Pass { get; set; }

        public enum RenderPass
        {
            Scan,
            Replace
        }

        private sealed class LeafBlockRenderer : MarkdownObjectRenderer<TranslationRenderer, LeafBlock>
        {
            protected override void Write(TranslationRenderer renderer, LeafBlock obj)
            {
                renderer.Write(obj.Inline);
            }
        }

        private sealed class LiteralInlineRenderer : MarkdownObjectRenderer<TranslationRenderer, LiteralInline>
        {
            public override bool Accept(RendererBase renderer, MarkdownObject obj)
            {
                return base.Accept(renderer, obj);
            }

            protected override void Write(TranslationRenderer renderer, LiteralInline obj)
            {
                var text = obj.Content.Text;

                if (!string.IsNullOrWhiteSpace(text))
                {
                    if (renderer.Pass == RenderPass.Scan)
                    {
                        renderer.AddText(text);
                    }
                    else
                    {
                        obj.Content = new StringSlice(renderer.GetText(text));
                    }
                }
            }
        }

        public TranslationRenderer()
        {
            ObjectRenderers.Add(new LiteralInlineRenderer());
            ObjectRenderers.Add(new LeafBlockRenderer());

            ObjectWriteBefore += TranslationRenderer_ObjectWriteBefore;
        }

        private void TranslationRenderer_ObjectWriteBefore(IMarkdownRenderer arg1, MarkdownObject arg2)
        {
        }

        public void AddText(string text)
        {
            texts[text] = text;
        }

        public string GetText(string text)
        {
            if (texts.TryGetValue(text, out var result))
            {
                return result;
            }

            return text;
        }

        public override object Render(MarkdownObject markdownObject)
        {
            Write(markdownObject);

            return null;
        }
    }
}
