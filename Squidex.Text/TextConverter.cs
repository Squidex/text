// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Text;
using HtmlAgilityPack;
using Markdig;

namespace Squidex.Text
{
    /// <summary>
    /// Converts texts.
    /// </summary>
    public static class TextConverter
    {
        /// <summary>
        /// Converts markdown to plain text.
        /// </summary>
        /// <param name="markdown">The markdown to convert.</param>
        /// <returns>
        /// The plain text.
        /// </returns>
        public static string Markdown2Text(string markdown)
        {
            if (string.IsNullOrWhiteSpace(markdown))
            {
                return markdown;
            }

            return Markdown.ToPlainText(markdown).Trim(' ', '\n', '\r');
        }

        /// <summary>
        /// Converts html to plain text.
        /// </summary>
        /// <param name="html">The html to convert.</param>
        /// <returns>
        /// The plain text.
        /// </returns>
        public static string Html2Text(string html)
        {
            if (string.IsNullOrWhiteSpace(html))
            {
                return html;
            }

            var document = LoadHtml(html);

            var sb = new StringBuilder();

            WriteTextTo(document.DocumentNode, sb);

            return sb.ToString().Trim(' ', '\n', '\r');
        }

        private static HtmlDocument LoadHtml(string text)
        {
            var document = new HtmlDocument();

            document.LoadHtml(text);

            return document;
        }

        private static void WriteTextTo(HtmlNode node, StringBuilder sb)
        {
            switch (node.NodeType)
            {
                case HtmlNodeType.Comment:
                    break;
                case HtmlNodeType.Document:
                    WriteChildrenTextTo(node, sb);
                    break;
                case HtmlNodeType.Text:
                    var html = ((HtmlTextNode)node).Text;

                    if (HtmlNode.IsOverlappedClosingElement(html))
                    {
                        break;
                    }

                    if (!string.IsNullOrWhiteSpace(html))
                    {
                        sb.Append(HtmlEntity.DeEntitize(html));
                    }

                    break;

                case HtmlNodeType.Element:
                    switch (node.Name)
                    {
                        case "p":
                            sb.AppendLine();
                            break;
                        case "br":
                            sb.AppendLine();
                            break;
                        case "style":
                            return;
                        case "script":
                            return;
                    }

                    if (node.HasChildNodes)
                    {
                        WriteChildrenTextTo(node, sb);
                    }

                    break;
            }
        }

        private static void WriteChildrenTextTo(HtmlNode node, StringBuilder sb)
        {
            foreach (var child in node.ChildNodes)
            {
                WriteTextTo(child, sb);
            }
        }
    }
}
