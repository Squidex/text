// ==========================================================================
//  Squidex Headless CMS
// ==========================================================================
//  Copyright (c) Squidex UG (haftungsbeschraenkt)
//  All rights reserved. Licensed under the MIT license.
// ==========================================================================

using Xunit;

namespace Squidex.Text.Tests
{
    public class WordsTests
    {
        [Fact]
        public void Should_count_zero_words_for_empty_text()
        {
            Assert.Equal(0, Words.Count(string.Empty));
        }

        [Fact]
        public void Should_count_zero_words_for_whitspace_text()
        {
            Assert.Equal(0, Words.Count("  "));
        }

        [Fact]
        public void Should_count_english_words()
        {
            Assert.Equal(4, Words.Count("You can't do that"));
        }

        [Fact]
        public void Should_count_english_words_with_punctuation()
        {
            Assert.Equal(5, Words.Count("You can't do that, Mister."));
        }

        [Fact]
        public void Should_count_english_words_with_extra_whitespaces()
        {
            Assert.Equal(4, Words.Count("You can't do  that. "));
        }

        [Fact]
        public void Should_count_cjk_and_english_words()
        {
            Assert.Equal(7, Words.Count("Makes probably no sense: 空手道"));
        }

        [Fact]
        public void Should_count_cjk_words()
        {
            Assert.Equal(3, Words.Count("空手道"));
        }

        [Fact]
        public void Should_count_katakana_words()
        {
            Assert.Equal(3, Words.Count("カラテ カラテ カラテ"));
        }
    }
}
