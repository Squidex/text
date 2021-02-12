// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;

namespace Squidex.Text.Translations.GoogleCloud
{
    public sealed class GoogleCloudTranslationOptions
    {
        public string ProjectId { get; set; }

        public Dictionary<string, string> Mapping { get; set; }
    }
}
