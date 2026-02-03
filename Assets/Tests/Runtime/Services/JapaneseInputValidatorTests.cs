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
            var result = _validator.Validate('a', 'あ');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('a'));
        }

        [Test]
        public void Validate_子音二文字_正解を返す()
        {
            // Arrange & Act
            var result1 = _validator.Validate('k', 'か');
            var result2 = _validator.Validate('a', 'か');

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_三文字のローマ字_正解を返す()
        {
            // Arrange & Act (shi → し)
            var result1 = _validator.Validate('s', 'し');
            var result2 = _validator.Validate('h', 'し');
            var result3 = _validator.Validate('i', 'し');

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
            _validator.Validate('k', 'か');

            // Assert
            Assert.That(_validator.PendingInput, Is.EqualTo("k"));
        }

        [Test]
        public void PendingInput_変換完了後_バッファがクリアされる()
        {
            // Act
            _validator.Validate('k', 'か');
            _validator.Validate('a', 'か');

            // Assert
            Assert.That(_validator.PendingInput, Is.Empty);
        }

        [Test]
        public void ClearBuffer_バッファがクリアされる()
        {
            // Arrange
            _validator.Validate('k', 'か');
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
            var result1 = _validator.Validate('k', 'っ');
            var result2 = _validator.Validate('k', 'っ');

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
        }

        [Test]
        public void Validate_促音後の文字_正しく変換される()
        {
            // 新しいバリデータで「っか」をテスト
            var validator = new JapaneseInputValidator();

            // 「k」入力 → 入力途中
            var r1 = validator.Validate('k', 'っ');
            Assert.That(r1.IsIgnored, Is.True);

            // 「k」入力 → 入力途中（促音パターン）
            var r2 = validator.Validate('k', 'っ');
            Assert.That(r2.IsIgnored, Is.True);

            // 「a」入力 → 「っか」に変換、期待が「っ」なので正解
            var r3 = validator.Validate('a', 'っ');
            Assert.That(r3.IsCorrect, Is.True);
        }

        #endregion

        #region 「ん」の処理

        [Test]
        public void Validate_nn入力_んに変換される()
        {
            // Arrange & Act (nn → ん)
            var result1 = _validator.Validate('n', 'ん');
            var result2 = _validator.Validate('n', 'ん');

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_n後に子音_んが確定する()
        {
            // Arrange & Act (nk → ん + k)
            var result1 = _validator.Validate('n', 'ん');
            var result2 = _validator.Validate('k', 'ん');

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
            var result1 = _validator.Validate('n', 'な');
            var result2 = _validator.Validate('a', 'な');

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
            var result = _validator.Validate('i', 'あ');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('i'));
            Assert.That(result.ExpectedChar, Is.EqualTo('あ'));
        }

        [Test]
        public void Validate_無効なローマ字パターン_不正解を返す()
        {
            // Act
            var result1 = _validator.Validate('x', 'あ');
            var result2 = _validator.Validate('z', 'あ');

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
            var result = _validator.Validate('A', 'あ');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
        }

        [Test]
        public void Validate_大文字混在_正しく変換される()
        {
            // Act (KA → か)
            var result1 = _validator.Validate('K', 'か');
            var result2 = _validator.Validate('A', 'か');

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsCorrect, Is.True);
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

        #region 拗音

        [Test]
        public void Validate_拗音_正しく変換される()
        {
            // Arrange & Act (kya → きゃ)
            var result1 = _validator.Validate('k', 'き');
            var result2 = _validator.Validate('y', 'き');
            var result3 = _validator.Validate('a', 'き');

            // Assert
            Assert.That(result1.IsIgnored, Is.True);
            Assert.That(result2.IsIgnored, Is.True);
            // 「きゃ」の最初の文字「き」と比較されるため正解
            Assert.That(result3.IsCorrect, Is.True);
        }

        #endregion
    }
}
