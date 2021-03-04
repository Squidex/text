﻿// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Collections.Generic;

namespace Squidex.Text.Translations
{
    public sealed class DeepLOptions
    {
        public string AuthKey { get; set; }

        public Dictionary<string, string> Mapping { get; set; }
    }
}
