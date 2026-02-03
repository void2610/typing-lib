using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Core.Interfaces
{
    /// <summary>
    /// 入力検証インターフェース
    /// </summary>
    public interface IInputValidator
    {
        /// <summary>
        /// 入力文字を検証する
        /// </summary>
        /// <param name="input">入力された文字</param>
        /// <param name="remainingText">残りの入力対象テキスト</param>
        /// <returns>入力結果</returns>
        InputResult Validate(char input, string remainingText);

        /// <summary>
        /// 大文字小文字を区別するかどうか
        /// </summary>
        bool IsCaseSensitive { get; set; }
    }
}
