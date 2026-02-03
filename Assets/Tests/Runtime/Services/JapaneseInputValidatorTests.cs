using NUnit.Framework;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Tests.Services
{
    /// <summary>
    /// JapaneseInputValidator のユニットテスト
    /// </summary>
    [TestFixture]
    public class JapaneseInputValidatorTests
    {
        private JapaneseInputValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new JapaneseInputValidator();
        }

        #region 基本的な変換

        [Test]
        public void Validate_母音一文字_正解を返す()
        {
            // Act
            var result = _validator.Validate('a', "あいう");

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
            Assert.That(result.ConsumedCount, Is.EqualTo(1));
        }

        [Test]
        public void Validate_子音二文字_正解を返す()
        {
            // Arrange & Act
            var result1 = _validator.Validate('k', "かきく");
            var result2 = _validator.Validate('a', "かきく");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
            Assert.That(result2.ConsumedCount, Is.EqualTo(1));
        }

        [Test]
        public void Validate_三文字のローマ字_正解を返す()
        {
            // Arrange & Act (shi → し)
            var result1 = _validator.Validate('s', "しすせ");
            var result2 = _validator.Validate('h', "しすせ");
            var result3 = _validator.Validate('i', "しすせ");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
            Assert.That(result3.IsCorrect, Is.True);
        }

        #endregion

        #region バッファリング

        [Test]
        public void PendingInput_入力途中_バッファに文字が蓄積される()
        {
            // Act
            _validator.Validate('k', "かきく");

            // Assert
            Assert.That(_validator.PendingInput, Is.EqualTo("k"));
        }

        [Test]
        public void PendingInput_変換完了後_バッファがクリアされる()
        {
            // Act
            _validator.Validate('k', "かきく");
            _validator.Validate('a', "かきく");

            // Assert
            Assert.That(_validator.PendingInput, Is.Empty);
        }

        [Test]
        public void ClearBuffer_バッファがクリアされる()
        {
            // Arrange
            _validator.Validate('k', "かきく");
            Assert.That(_validator.PendingInput, Is.EqualTo("k"));

            // Act
            _validator.ClearBuffer();

            // Assert
            Assert.That(_validator.PendingInput, Is.Empty);
        }

        #endregion

        #region 促音処理

        [Test]
        public void Validate_促音_子音連続で変換される()
        {
            // Arrange & Act (kka → っか)
            var result1 = _validator.Validate('k', "っか");
            var result2 = _validator.Validate('k', "っか");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
        }

        [Test]
        public void Validate_促音後の文字_正しく変換される()
        {
            // 「っか」をテスト
            var validator = new JapaneseInputValidator();

            var r1 = validator.Validate('k', "っか");
            Assert.That(r1.IsIgnored, Is.True);

            var r2 = validator.Validate('k', "っか");
            Assert.That(r2.IsIgnored, Is.True);

            var r3 = validator.Validate('a', "っか");
            Assert.That(r3.IsCorrect, Is.True);
            Assert.That(r3.ConsumedCount, Is.EqualTo(2)); // 「っか」の2文字分
        }

        #endregion

        #region 「ん」の処理

        [Test]
        public void Validate_nn入力_んに変換される()
        {
            // Arrange & Act (nn → ん)
            var result1 = _validator.Validate('n', "んあ");
            var result2 = _validator.Validate('n', "んあ");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
            Assert.That(result2.ConsumedCount, Is.EqualTo(1));
        }

        [Test]
        public void Validate_n後に子音_んが確定する()
        {
            // Arrange & Act (nk → ん + k)
            var result1 = _validator.Validate('n', "んか");
            var result2 = _validator.Validate('k', "んか");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
            // バッファには「k」が残っている
            Assert.That(_validator.PendingInput, Is.EqualTo("k"));
        }

        [Test]
        public void Validate_n後に母音_な行に変換される()
        {
            // Arrange & Act (na → な)
            var result1 = _validator.Validate('n', "なにぬ");
            var result2 = _validator.Validate('a', "なにぬ");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
        }

        #endregion

        #region 不正解の入力

        [Test]
        public void Validate_期待と異なる文字_不正解を返す()
        {
            // Act (「あ」を期待して「i」を入力)
            var result = _validator.Validate('i', "あいう");

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('i'));
            Assert.That(result.ExpectedChar, Is.EqualTo('あ'));
        }

        [Test]
        public void Validate_無効なローマ字パターン_不正解を返す()
        {
            // Act
            var result1 = _validator.Validate('x', "あいう");
            var result2 = _validator.Validate('z', "あいう");

            // 「xz」は有効なプレフィックスではないため、2回目で不正解
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.False);
        }

        #endregion

        #region 大文字入力

        [Test]
        public void Validate_大文字入力_小文字として処理される()
        {
            // Act
            var result = _validator.Validate('A', "あいう");

            // Assert
            Assert.That(result.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_大文字混在_正しく変換される()
        {
            // Act (KA → か)
            var result1 = _validator.Validate('K', "かきく");
            var result2 = _validator.Validate('A', "かきく");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
        }

        #endregion

        #region 拗音（複数文字消費）

        [Test]
        public void Validate_拗音きょ_ConsumedCountが2になる()
        {
            // Arrange & Act (kyo → きょ)
            var result1 = _validator.Validate('k', "きょう");
            var result2 = _validator.Validate('y', "きょう");
            var result3 = _validator.Validate('o', "きょう");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
            Assert.That(result3.IsCorrect, Is.True);
            Assert.That(result3.ConsumedCount, Is.EqualTo(2));
        }

        [Test]
        public void Validate_拗音しゃ_ConsumedCountが2になる()
        {
            // Arrange & Act (sha → しゃ)
            var result1 = _validator.Validate('s', "しゃしゅしょ");
            var result2 = _validator.Validate('h', "しゃしゅしょ");
            var result3 = _validator.Validate('a', "しゃしゅしょ");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
            Assert.That(result3.IsCorrect, Is.True);
            Assert.That(result3.ConsumedCount, Is.EqualTo(2));
        }

        [Test]
        public void Validate_拗音ちゃ_ConsumedCountが2になる()
        {
            // Arrange & Act (cha → ちゃ)
            var result1 = _validator.Validate('c', "ちゃちゅちょ");
            var result2 = _validator.Validate('h', "ちゃちゅちょ");
            var result3 = _validator.Validate('a', "ちゃちゅちょ");

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
            Assert.That(result3.IsCorrect, Is.True);
            Assert.That(result3.ConsumedCount, Is.EqualTo(2));
        }

        #endregion

        #region 空・null テスト

        [Test]
        public void Validate_残りテキストが空_Ignoredを返す()
        {
            // Act
            var result = _validator.Validate('a', "");

            // Assert
            Assert.That(result.IsIgnored, Is.True);
        }

        [Test]
        public void Validate_残りテキストがnull_Ignoredを返す()
        {
            // Act
            var result = _validator.Validate('a', null);

            // Assert
            Assert.That(result.IsIgnored, Is.True);
        }

        #endregion

        #region IsCaseSensitive プロパティ

        [Test]
        public void IsCaseSensitive_デフォルトはfalse()
        {
            // Assert
            Assert.That(_validator.IsCaseSensitive, Is.False);
        }

        #endregion
    }
}
