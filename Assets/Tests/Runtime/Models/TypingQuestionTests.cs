using System.Collections.Generic;
using NUnit.Framework;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Tests.Models
{
    /// <summary>
    /// TypingQuestion のユニットテスト
    /// </summary>
    [TestFixture]
    public class TypingQuestionTests
    {
        [Test]
        public void Constructor_フルパラメータ_全プロパティが設定される()
        {
            // Arrange
            var metadata = new Dictionary<string, object> { { "key", "value" } };

            // Act
            var question = new TypingQuestion("id1", "表示テキスト", "inputtext", metadata);

            // Assert
            Assert.That(question.Id, Is.EqualTo("id1"));
            Assert.That(question.DisplayText, Is.EqualTo("表示テキスト"));
            Assert.That(question.InputText, Is.EqualTo("inputtext"));
            Assert.That(question.Metadata["key"], Is.EqualTo("value"));
        }

        [Test]
        public void Constructor_フルパラメータ_メタデータnull_空の辞書が設定される()
        {
            // Arrange & Act
            var question = new TypingQuestion("id1", "表示テキスト", "inputtext", null);

            // Assert
            Assert.That(question.Metadata, Is.Not.Null);
            Assert.That(question.Metadata.Count, Is.EqualTo(0));
        }

        [Test]
        public void Constructor_DisplayTextとInputText_IDが自動生成される()
        {
            // Arrange & Act
            var question = new TypingQuestion("表示テキスト", "inputtext");

            // Assert
            Assert.That(question.Id, Is.Not.Null);
            Assert.That(question.Id, Is.Not.Empty);
            Assert.That(question.DisplayText, Is.EqualTo("表示テキスト"));
            Assert.That(question.InputText, Is.EqualTo("inputtext"));
        }

        [Test]
        public void Constructor_DisplayTextとInputText_異なるIDが生成される()
        {
            // Arrange & Act
            var question1 = new TypingQuestion("text1", "input1");
            var question2 = new TypingQuestion("text2", "input2");

            // Assert
            Assert.That(question1.Id, Is.Not.EqualTo(question2.Id));
        }

        [Test]
        public void Constructor_テキストのみ_DisplayTextとInputTextが同一()
        {
            // Arrange & Act
            var question = new TypingQuestion("hello");

            // Assert
            Assert.That(question.Id, Is.Not.Null);
            Assert.That(question.Id, Is.Not.Empty);
            Assert.That(question.DisplayText, Is.EqualTo("hello"));
            Assert.That(question.InputText, Is.EqualTo("hello"));
        }

        [Test]
        public void Length_InputTextの文字数を返す()
        {
            // Arrange
            var question = new TypingQuestion("hello");

            // Act & Assert
            Assert.That(question.Length, Is.EqualTo(5));
        }

        [Test]
        public void Length_日本語テキスト_文字数を正しく返す()
        {
            // Arrange
            var question = new TypingQuestion("表示", "あいう");

            // Act & Assert
            Assert.That(question.Length, Is.EqualTo(3));
        }

        [Test]
        public void Length_空文字列_0を返す()
        {
            // Arrange
            var question = new TypingQuestion("id", "表示", "");

            // Act & Assert
            Assert.That(question.Length, Is.EqualTo(0));
        }
    }
}
