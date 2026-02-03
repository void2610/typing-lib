using NUnit.Framework;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Tests.Models
{
    /// <summary>
    /// InputResult のユニットテスト
    /// </summary>
    [TestFixture]
    public class InputResultTests
    {
        [Test]
        public void Correct_正解の結果を返す()
        {
            // Arrange & Act
            var result = InputResult.Correct('a');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
            Assert.That(result.ExpectedChar, Is.EqualTo('a'));
            Assert.That(result.IsIgnored, Is.False);
        }

        [Test]
        public void Incorrect_不正解の結果を返す()
        {
            // Arrange & Act
            var result = InputResult.Incorrect('b', 'a');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('b'));
            Assert.That(result.ExpectedChar, Is.EqualTo('a'));
            Assert.That(result.IsIgnored, Is.False);
        }

        [Test]
        public void Ignored_無視された結果を返す()
        {
            // Arrange & Act
            var result = InputResult.Ignored('x');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('x'));
            Assert.That(result.ExpectedChar, Is.EqualTo('\0'));
            Assert.That(result.IsIgnored, Is.True);
        }

        [Test]
        public void Correct_異なる文字でも正解として扱われる()
        {
            // Arrange & Act
            var result1 = InputResult.Correct('A');
            var result2 = InputResult.Correct('1');
            var result3 = InputResult.Correct(' ');

            // Assert
            Assert.That(result1.IsCorrect, Is.True);
            Assert.That(result1.InputChar, Is.EqualTo('A'));

            Assert.That(result2.IsCorrect, Is.True);
            Assert.That(result2.InputChar, Is.EqualTo('1'));

            Assert.That(result3.IsCorrect, Is.True);
            Assert.That(result3.InputChar, Is.EqualTo(' '));
        }

        [Test]
        public void Correct_デフォルトのConsumedCountは1()
        {
            // Arrange & Act
            var result = InputResult.Correct('a');

            // Assert
            Assert.That(result.ConsumedCount, Is.EqualTo(1));
        }

        [Test]
        public void Correct_ConsumedCount指定_正しく設定される()
        {
            // Arrange & Act
            var result = InputResult.Correct('a', 2);

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.ConsumedCount, Is.EqualTo(2));
        }

        [Test]
        public void Ignored_ConsumedCountは0()
        {
            // Arrange & Act
            var result = InputResult.Ignored('x');

            // Assert
            Assert.That(result.ConsumedCount, Is.EqualTo(0));
        }

        [Test]
        public void Incorrect_ConsumedCountは1()
        {
            // Arrange & Act
            var result = InputResult.Incorrect('b', 'a');

            // Assert
            Assert.That(result.ConsumedCount, Is.EqualTo(1));
        }
    }
}
