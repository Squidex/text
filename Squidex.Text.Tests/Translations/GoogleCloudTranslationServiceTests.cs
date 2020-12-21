// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using System.Threading.Tasks;
using Squidex.Text.Translations;
using Squidex.Text.Translations.GoogleCloud;
using Xunit;

namespace Squidex.Text.Tests.Translations
{
    public class GoogleCloudTranslationServiceTests : TranslationServiceTestsBase<GoogleCloudTranslationService>
    {
        protected override GoogleCloudTranslationService CreateService()
        {
            return new GoogleCloudTranslationService(new GoogleCloudTranslationOptions
            {
                ProjectId = "squidex-157415"
            });
        }

        [Fact]
        public async Task Should_return_result_if_not_configured()
        {
            var sut = new GoogleCloudTranslationService(new GoogleCloudTranslationOptions());

            var results = await sut.TranslateAsync(new[] { "Hello" }, "en");

            Assert.All(results, x => Assert.Equal(TranslationResultCode.NotConfigured, x.Code));
        }
    }
}
