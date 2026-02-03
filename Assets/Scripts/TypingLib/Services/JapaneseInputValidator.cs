using System.Text;
using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Services
{
    /// <summary>
    /// 日本語入力検証クラス
    /// ローマ字入力をバッファリングし、ひらがなへの変換を検証する
    /// </summary>
    public class JapaneseInputValidator : IJapaneseInputValidator
    {
        /// <summary>
        /// 未確定のローマ字バッファ
        /// </summary>
        public string PendingInput => _buffer.ToString();

        /// <summary>
        /// 大文字小文字を区別するかどうか（日本語入力では通常false）
        /// </summary>
        public bool IsCaseSensitive { get; set; } = false;

        private readonly StringBuilder _buffer = new();

        /// <summary>
        /// 入力文字を検証する
        /// </summary>
        /// <param name="input">入力されたローマ字</param>
        /// <param name="expected">期待されるひらがな</param>
        /// <returns>入力結果</returns>
        public InputResult Validate(char input, char expected)
        {
            // 小文字に正規化
            var normalizedInput = char.ToLower(input);
            _buffer.Append(normalizedInput);

            var bufferStr = _buffer.ToString();

            // 「n」の特殊処理: "n" + 子音 → 「ん」+ 次の処理
            if (TryHandleNConsonant(bufferStr, expected, out var nResult))
            {
                return nResult;
            }

            // 変換を試みる
            if (RomajiTable.TryConvert(bufferStr, out var hiragana))
            {
                _buffer.Clear();

                // 変換されたひらがなが期待文字と一致するか
                if (hiragana.Length > 0 && hiragana[0] == expected)
                {
                    return InputResult.Correct(input);
                }
                return InputResult.Incorrect(input, expected);
            }

            // 入力途中として有効なら継続（Ignoredを返す）
            if (RomajiTable.IsValidPrefix(bufferStr))
            {
                return InputResult.Ignored(input);
            }

            // 無効な入力
            _buffer.Clear();
            return InputResult.Incorrect(input, expected);
        }

        /// <summary>
        /// バッファをクリアする
        /// </summary>
        public void ClearBuffer() => _buffer.Clear();

        /// <summary>
        /// 「n」+ 子音 パターンの処理
        /// 例: "nk" → 「ん」を確定し、"k" をバッファに残す
        /// </summary>
        /// <param name="bufferStr">現在のバッファ</param>
        /// <param name="expected">期待されるひらがな</param>
        /// <param name="result">処理結果</param>
        /// <returns>処理した場合はtrue</returns>
        private bool TryHandleNConsonant(string bufferStr, char expected, out InputResult result)
        {
            result = default;

            // バッファが "n" + 何か の形式で、「ん」を確定できるパターンかチェック
            if (bufferStr.Length >= 2 && bufferStr[0] == 'n')
            {
                var secondChar = bufferStr[1];

                // "n" + 子音（母音・y・n以外）の場合、「ん」を確定
                if (RomajiTable.IsNFollowedByConsonant(secondChar))
                {
                    // 期待される文字が「ん」の場合
                    if (expected == 'ん')
                    {
                        // 「ん」を確定し、残りの文字をバッファに残す
                        _buffer.Clear();
                        _buffer.Append(bufferStr.Substring(1));
                        result = InputResult.Correct(bufferStr[1]);
                        return true;
                    }

                    // 期待される文字が「ん」でない場合は不正解
                    _buffer.Clear();
                    _buffer.Append(bufferStr.Substring(1));
                    result = InputResult.Incorrect(bufferStr[1], expected);
                    return true;
                }
            }

            return false;
        }
    }
}
