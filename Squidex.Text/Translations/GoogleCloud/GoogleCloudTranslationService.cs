﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Google.Cloud.Translate.V3;
using Grpc.Core;

#pragma warning disable IDE0066 // Convert switch statement to expression

namespace Squidex.Text.Translations.GoogleCloud
{
    public sealed class GoogleCloudTranslationService : ITranslationService
    {
        private readonly TranslationServiceClient service;
        private readonly GoogleCloudTranslationOptions options;

        public GoogleCloudTranslationService(GoogleCloudTranslationOptions options)
        {
            service =
                new TranslationServiceClientBuilder()
                    .Build();

            this.options = options;
        }

        public async Task<IReadOnlyList<TranslationResult>> TranslateAsync(IEnumerable<string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default)
        {
            var results = new List<TranslationResult>();

            if (string.IsNullOrWhiteSpace(options.ProjectId))
            {
                for (var i = 0; i < texts.Count(); i++)
                {
                    results.Add(TranslationResult.NotConfigured);
                }

                return results;
            }

            var request = new TranslateTextRequest
            {
                Parent = $"projects/{options.ProjectId}"
            };

            foreach (var text in texts)
            {
                request.Contents.Add(text);
            }

            request.TargetLanguageCode = GetLanguageCode(targetLanguage);

            if (sourceLanguage != null)
            {
                request.SourceLanguageCode = GetLanguageCode(sourceLanguage);
            }

            request.MimeType = "text/plain";

            try
            {
                var response = await service.TranslateTextAsync(request, ct);

                results.AddRange(response.Translations.Select(x => new TranslationResult(x.TranslatedText,
                    GetSourceLanguage(x.DetectedLanguageCode, sourceLanguage))));
            }
            catch (RpcException ex)
            {
                var result = GetResult(ex.Status);

                for (var i = 0; i < texts.Count(); i++)
                {
                    results.Add(result);
                }
            }

            return results;
        }

        private static string GetSourceLanguage(string language, string fallback)
        {
            var result = language?.ToLowerInvariant();

            if (string.IsNullOrWhiteSpace(result))
            {
                result = fallback;
            }

            return result;
        }

        private static TranslationResult GetResult(Status status)
        {
            switch (status.StatusCode)
            {
                case StatusCode.InvalidArgument:
                    return TranslationResult.LanguageNotSupported;
                case StatusCode.PermissionDenied:
                    return TranslationResult.Unauthorized;
                default:
                    return TranslationResult.Failed;
            }
        }

        private static string GetLanguageCode(string language)
        {
            return language.Substring(0, 2).ToUpperInvariant();
        }
    }
}