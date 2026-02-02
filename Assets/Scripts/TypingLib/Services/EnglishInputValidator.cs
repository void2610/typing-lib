using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Services
{
    /// <summary>
    /// 英語入力検証サービス
    /// </summary>
    public class EnglishInputValidator : IInputValidator
    {
        /// <summary>
        /// 大文字小文字を区別するかどうか
        /// </summary>
        public bool IsCaseSensitive { get; set; } = true;

        /// <summary>
        /// 入力文字を検証する
        /// </summary>
        public InputResult Validate(char input, char expected)
        {
            bool isCorrect;

            if (IsCaseSensitive)
            {
                isCorrect = input == expected;
            }
            else
            {
                isCorrect = char.ToLowerInvariant(input) == char.ToLowerInvariant(expected);
            }

            return isCorrect
                ? InputResult.Correct(input)
                : InputResult.Incorrect(input, expected);
        }
    }
}
