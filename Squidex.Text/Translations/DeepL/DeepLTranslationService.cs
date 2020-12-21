// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading;
using System.Threading.Tasks;

namespace Squidex.Text.Translations
{
    [ExcludeFromCodeCoverage]
    public sealed class DeepLTranslationService : ITranslationService
    {
        private const string Url = "https://api.deepl.com/v2/translate";
        private readonly DeepLOptions deeplOptions;
        private HttpClient httpClient;

        private sealed class Response
        {
            [JsonPropertyName("translations")]
            public ResponseTranslation[] Translations { get; set; }
        }

        private sealed class ResponseTranslation
        {
            [JsonPropertyName("text")]
            public string Text { get; set; }

            [JsonPropertyName("detected_source_language")]
            public string DetectedSourceLanguage { get; set; }
        }

        public DeepLTranslationService(DeepLOptions deeplOptions)
        {
            this.deeplOptions = deeplOptions;
        }

        public async Task<IReadOnlyList<TranslationResult>> TranslateAsync(IEnumerable<string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default)
        {
            var results = new List<TranslationResult>();

            if (string.IsNullOrWhiteSpace(deeplOptions.AuthKey))
            {
                for (var i = 0; i < texts.Count(); i++)
                {
                    results.Add(TranslationResult.NotConfigured);
                }

                return results;
            }

            if (httpClient == null)
            {
                httpClient = new HttpClient();
            }

            var parameters = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("auth_key", deeplOptions.AuthKey),
                new KeyValuePair<string, string>("target_lang", GetLanguageCode(targetLanguage))
            };

            foreach (var text in texts)
            {
                parameters.Add(new KeyValuePair<string, string>("text", text));
            }

            if (sourceLanguage != null)
            {
                parameters.Add(new KeyValuePair<string, string>("source_lang", GetLanguageCode(sourceLanguage)));
            }

            var body = new FormUrlEncodedContent(parameters!);

            using (var response = await httpClient.PostAsync(Url, body, ct))
            {
                var responseString = await response.Content.ReadAsStringAsync();

                if (response.IsSuccessStatusCode)
                {
                    var result = JsonSerializer.Deserialize<Response>(responseString);

                    foreach (var item in result.Translations)
                    {
                        results.Add(new TranslationResult(item.Text,
                            GetSourceLanguage(item.DetectedSourceLanguage, sourceLanguage)));
                    }
                }
                else
                {
                    var result = GetResult(response);

                    for (var i = 0; i < texts.Count(); i++)
                    {
                        results.Add(result);
                    }
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

        private static TranslationResult GetResult(HttpResponseMessage response)
        {
            switch (response.StatusCode)
            {
                case HttpStatusCode.BadRequest:
                    return TranslationResult.LanguageNotSupported;
                case HttpStatusCode.Forbidden:
                case HttpStatusCode.Unauthorized:
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
