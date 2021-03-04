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
    public interface ITranslationService
    {
        Task<IReadOnlyList<TranslationResult>> TranslateAsync(IEnumerable<string> texts, string targetLanguage, string sourceLanguage = null, CancellationToken ct = default);
    }
}
