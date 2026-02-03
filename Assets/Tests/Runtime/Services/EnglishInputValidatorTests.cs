using NUnit.Framework;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Tests.Services
{
    /// <summary>
    /// EnglishInputValidator のユニットテスト
    /// </summary>
    [TestFixture]
    public class EnglishInputValidatorTests
    {
        private EnglishInputValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new EnglishInputValidator();
        }

        #region 大文字小文字区別あり（デフォルト）

        [Test]
        public void Validate_同じ文字_正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('a', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_異なる文字_不正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('b', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('b'));
            Assert.That(result.ExpectedChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_大文字小文字区別あり_大文字と小文字_不正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('A', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('A'));
            Assert.That(result.ExpectedChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_大文字小文字区別あり_小文字と大文字_不正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('a', 'A');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('a'));
            Assert.That(result.ExpectedChar, Is.EqualTo('A'));
        }

        #endregion

        #region 大文字小文字区別なし

        [Test]
        public void Validate_大文字小文字区別なし_同じ文字_正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = false;

            // Act
            var result = _validator.Validate('a', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_大文字小文字区別なし_大文字入力で小文字期待_正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = false;

            // Act
            var result = _validator.Validate('A', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('A'));
        }

        [Test]
        public void Validate_大文字小文字区別なし_小文字入力で大文字期待_正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = false;

            // Act
            var result = _validator.Validate('a', 'A');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_大文字小文字区別なし_異なる文字_不正解を返す()
        {
            // Arrange
            _validator.IsCaseSensitive = false;

            // Act
            var result = _validator.Validate('b', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('b'));
            Assert.That(result.ExpectedChar, Is.EqualTo('a'));
        }

        #endregion

        #region 記号・数字

        [Test]
        public void Validate_数字_同じ数字は正解()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('1', '1');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_記号_同じ記号は正解()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate('!', '!');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_空白_同じ空白は正解()
        {
            // Arrange
            _validator.IsCaseSensitive = true;

            // Act
            var result = _validator.Validate(' ', ' ');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
        }

        #endregion

        #region IsCaseSensitive プロパティ

        [Test]
        public void IsCaseSensitive_デフォルトはtrue()
        {
            // Arrange
            var validator = new EnglishInputValidator();

            // Assert
            Assert.That(validator.IsCaseSensitive, Is.True);
        }

        [Test]
        public void IsCaseSensitive_設定変更が反映される()
        {
            // Arrange
            var validator = new EnglishInputValidator();

            // Act
            validator.IsCaseSensitive = false;

            // Assert
            Assert.That(validator.IsCaseSensitive, Is.False);
        }

        #endregion
    }
}
