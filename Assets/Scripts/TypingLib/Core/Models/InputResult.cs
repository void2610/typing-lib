namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// 入力結果を表す構造体
    /// </summary>
    public readonly struct InputResult
    {
        /// <summary>
        /// 正解かどうか
        /// </summary>
        public bool IsCorrect { get; }

        /// <summary>
        /// 入力された文字
        /// </summary>
        public char InputChar { get; }

        /// <summary>
        /// 期待された文字
        /// </summary>
        public char ExpectedChar { get; }

        /// <summary>
        /// 入力が無視されたかどうか（セッションが実行中でない場合など）
        /// </summary>
        public bool IsIgnored { get; }

        /// <summary>
        /// 消費した文字数（拗音など複数文字変換時に使用）
        /// </summary>
        public int ConsumedCount { get; }

        private InputResult(bool isCorrect, char inputChar, char expectedChar, bool isIgnored, int consumedCount = 1)
        {
            IsCorrect = isCorrect;
            InputChar = inputChar;
            ExpectedChar = expectedChar;
            IsIgnored = isIgnored;
            ConsumedCount = consumedCount;
        }

        /// <summary>
        /// 正解の入力結果を作成する
        /// </summary>
        public static InputResult Correct(char inputChar) => new InputResult(true, inputChar, inputChar, false);

        /// <summary>
        /// 正解の入力結果を作成する（消費文字数指定）
        /// </summary>
        public static InputResult Correct(char inputChar, int consumedCount) => new InputResult(true, inputChar, inputChar, false, consumedCount);

        /// <summary>
        /// 不正解の入力結果を作成する
        /// </summary>
        public static InputResult Incorrect(char inputChar, char expectedChar) => new InputResult(false, inputChar, expectedChar, false);

        /// <summary>
        /// 無視された入力結果を作成する
        /// </summary>
        public static InputResult Ignored(char inputChar) => new InputResult(false, inputChar, '\0', true, 0);
    }
}
