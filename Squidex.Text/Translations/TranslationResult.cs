// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

namespace Squidex.Text.Translations
{
    public sealed class TranslationResult
    {
        public static readonly TranslationResult Unauthorized = new TranslationResult(TranslationResultCode.Unauthorized);
        public static readonly TranslationResult NotConfigured = new TranslationResult(TranslationResultCode.NotConfigured);
        public static readonly TranslationResult NotTranslated = new TranslationResult(TranslationResultCode.NotTranslated);
        public static readonly TranslationResult Failed = new TranslationResult(TranslationResultCode.Failed);
        public static readonly TranslationResult LanguageNotSupported = new TranslationResult(TranslationResultCode.LanguageNotSupported);

        public TranslationResultCode Code { get; }

        public string Translation { get; set; }

        public string TranslationLanguage { get; set; }

        public TranslationResult(string translation, string translationLanguage)
            : this(TranslationResultCode.Translated)
        {
            Translation = translation;
            TranslationLanguage = translationLanguage;
        }

        private TranslationResult(TranslationResultCode code)
        {
            Code = code;
        }
    }
}
