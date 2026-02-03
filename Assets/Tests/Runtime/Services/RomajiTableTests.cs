using NUnit.Framework;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Tests.Services
{
    /// <summary>
    /// RomajiTable のユニットテスト
    /// </summary>
    [TestFixture]
    public class RomajiTableTests
    {
        #region TryConvert テスト

        [Test]
        [TestCase("a", "あ")]
        [TestCase("i", "い")]
        [TestCase("u", "う")]
        [TestCase("e", "え")]
        [TestCase("o", "お")]
        public void TryConvert_母音_ひらがなに変換される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("ka", "か")]
        [TestCase("ki", "き")]
        [TestCase("ku", "く")]
        [TestCase("ke", "け")]
        [TestCase("ko", "こ")]
        [TestCase("sa", "さ")]
        [TestCase("shi", "し")]
        [TestCase("si", "し")]
        [TestCase("ta", "た")]
        [TestCase("chi", "ち")]
        [TestCase("ti", "ち")]
        [TestCase("tsu", "つ")]
        [TestCase("tu", "つ")]
        public void TryConvert_子音_ひらがなに変換される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("kya", "きゃ")]
        [TestCase("kyu", "きゅ")]
        [TestCase("kyo", "きょ")]
        [TestCase("sha", "しゃ")]
        [TestCase("shu", "しゅ")]
        [TestCase("sho", "しょ")]
        [TestCase("cha", "ちゃ")]
        [TestCase("chu", "ちゅ")]
        [TestCase("cho", "ちょ")]
        [TestCase("ja", "じゃ")]
        [TestCase("ju", "じゅ")]
        [TestCase("jo", "じょ")]
        [TestCase("nya", "にゃ")]
        [TestCase("nyu", "にゅ")]
        [TestCase("nyo", "にょ")]
        public void TryConvert_拗音_ひらがなに変換される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("kka", "っか")]
        [TestCase("tta", "った")]
        [TestCase("ppa", "っぱ")]
        [TestCase("sshi", "っし")]
        [TestCase("cchi", "っち")]
        public void TryConvert_促音_っが付加される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("nn", "ん")]
        [TestCase("n'", "ん")]
        [TestCase("xn", "ん")]
        public void TryConvert_ん_ひらがなに変換される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        [TestCase("xtu", "っ")]
        [TestCase("xtsu", "っ")]
        [TestCase("ltu", "っ")]
        [TestCase("ltsu", "っ")]
        public void TryConvert_小さいっ_明示的入力で変換される(string romaji, string expected)
        {
            // Act
            var result = RomajiTable.TryConvert(romaji, out var hiragana);

            // Assert
            Assert.That(result, Is.True);
            Assert.That(hiragana, Is.EqualTo(expected));
        }

        [Test]
        public void TryConvert_空文字列_falseを返す()
        {
            // Act
            var result = RomajiTable.TryConvert("", out var hiragana);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(hiragana, Is.Null);
        }

        [Test]
        public void TryConvert_null_falseを返す()
        {
            // Act
            var result = RomajiTable.TryConvert(null, out var hiragana);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(hiragana, Is.Null);
        }

        [Test]
        public void TryConvert_無効なローマ字_falseを返す()
        {
            // Act
            var result = RomajiTable.TryConvert("xyz", out var hiragana);

            // Assert
            Assert.That(result, Is.False);
            Assert.That(hiragana, Is.Null);
        }

        #endregion

        #region IsValidPrefix テスト

        [Test]
        [TestCase("k")]
        [TestCase("sh")]
        [TestCase("ch")]
        [TestCase("ts")]
        [TestCase("ky")]
        public void IsValidPrefix_有効なプレフィックス_trueを返す(string prefix)
        {
            // Act
            var result = RomajiTable.IsValidPrefix(prefix);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("kk")]
        [TestCase("tt")]
        [TestCase("pp")]
        [TestCase("ss")]
        public void IsValidPrefix_促音の途中_trueを返す(string prefix)
        {
            // Act
            var result = RomajiTable.IsValidPrefix(prefix);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase("n")]
        public void IsValidPrefix_n単独_trueを返す(string prefix)
        {
            // Act
            var result = RomajiTable.IsValidPrefix(prefix);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void IsValidPrefix_空文字列_falseを返す()
        {
            // Act
            var result = RomajiTable.IsValidPrefix("");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidPrefix_null_falseを返す()
        {
            // Act
            var result = RomajiTable.IsValidPrefix(null);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsValidPrefix_完全なローマ字_trueを返す()
        {
            // Act
            var result = RomajiTable.IsValidPrefix("ka");

            // Assert
            Assert.That(result, Is.True);
        }

        #endregion

        #region IsNFollowedByConsonant テスト

        [Test]
        [TestCase('k')]
        [TestCase('s')]
        [TestCase('t')]
        [TestCase('h')]
        [TestCase('m')]
        [TestCase('r')]
        [TestCase('w')]
        [TestCase('g')]
        [TestCase('z')]
        [TestCase('d')]
        [TestCase('b')]
        [TestCase('p')]
        public void IsNFollowedByConsonant_子音_trueを返す(char nextChar)
        {
            // Act
            var result = RomajiTable.IsNFollowedByConsonant(nextChar);

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        [TestCase('a')]
        [TestCase('i')]
        [TestCase('u')]
        [TestCase('e')]
        [TestCase('o')]
        public void IsNFollowedByConsonant_母音_falseを返す(char nextChar)
        {
            // Act
            var result = RomajiTable.IsNFollowedByConsonant(nextChar);

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNFollowedByConsonant_y_falseを返す()
        {
            // Act
            var result = RomajiTable.IsNFollowedByConsonant('y');

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void IsNFollowedByConsonant_n_falseを返す()
        {
            // Act
            var result = RomajiTable.IsNFollowedByConsonant('n');

            // Assert
            Assert.That(result, Is.False);
        }

        #endregion
    }
}
