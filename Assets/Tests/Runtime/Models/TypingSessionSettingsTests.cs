using NUnit.Framework;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Tests.Models
{
    /// <summary>
    /// TypingSessionSettings のユニットテスト
    /// </summary>
    [TestFixture]
    public class TypingSessionSettingsTests
    {
        [Test]
        public void Constructor_全パラメータが設定される()
        {
            // Arrange & Act
            var settings = new TypingSessionSettings(true, true, false);

            // Assert
            Assert.That(settings.SkipWhitespace, Is.True);
            Assert.That(settings.SkipSymbols, Is.True);
            Assert.That(settings.CaseSensitive, Is.False);
        }

        [Test]
        public void Constructor_CaseSensitiveのデフォルト値はtrue()
        {
            // Arrange & Act
            var settings = new TypingSessionSettings(false, false);

            // Assert
            Assert.That(settings.CaseSensitive, Is.True);
        }

        [Test]
        public void Default_スキップなし_大文字小文字区別あり()
        {
            // Arrange & Act
            var settings = TypingSessionSettings.Default;

            // Assert
            Assert.That(settings.SkipWhitespace, Is.False);
            Assert.That(settings.SkipSymbols, Is.False);
            Assert.That(settings.CaseSensitive, Is.True);
        }

        [Test]
        public void SkipWhitespaceOnly_空白のみスキップ()
        {
            // Arrange & Act
            var settings = TypingSessionSettings.SkipWhitespaceOnly;

            // Assert
            Assert.That(settings.SkipWhitespace, Is.True);
            Assert.That(settings.SkipSymbols, Is.False);
            Assert.That(settings.CaseSensitive, Is.True);
        }

        [Test]
        public void SkipAll_空白と記号をスキップ()
        {
            // Arrange & Act
            var settings = TypingSessionSettings.SkipAll;

            // Assert
            Assert.That(settings.SkipWhitespace, Is.True);
            Assert.That(settings.SkipSymbols, Is.True);
            Assert.That(settings.CaseSensitive, Is.True);
        }

        [Test]
        public void プリセット間で値が独立している()
        {
            // Arrange & Act
            var defaultSettings = TypingSessionSettings.Default;
            var skipWhitespaceSettings = TypingSessionSettings.SkipWhitespaceOnly;
            var skipAllSettings = TypingSessionSettings.SkipAll;

            // Assert
            Assert.That(defaultSettings.SkipWhitespace, Is.Not.EqualTo(skipWhitespaceSettings.SkipWhitespace));
            Assert.That(skipWhitespaceSettings.SkipSymbols, Is.Not.EqualTo(skipAllSettings.SkipSymbols));
        }
    }
}
