namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// タイピング進捗を表す構造体
    /// </summary>
    public readonly struct TypingProgress
    {
        /// <summary>
        /// 現在の問題インデックス
        /// </summary>
        public int CurrentQuestionIndex { get; }

        /// <summary>
        /// 総問題数
        /// </summary>
        public int TotalQuestions { get; }

        /// <summary>
        /// 現在の入力位置
        /// </summary>
        public int CurrentPosition { get; }

        /// <summary>
        /// 現在の問題の総文字数
        /// </summary>
        public int TotalCharacters { get; }

        /// <summary>
        /// 正解入力数（累計）
        /// </summary>
        public int CorrectCount { get; }

        /// <summary>
        /// ミス入力数（累計）
        /// </summary>
        public int MissCount { get; }

        /// <summary>
        /// 入力済みテキスト
        /// </summary>
        public string TypedText { get; }

        /// <summary>
        /// 残りテキスト
        /// </summary>
        public string RemainingText { get; }

        /// <summary>
        /// 全体の進捗率（0.0 - 1.0）
        /// </summary>
        public float ProgressRate
        {
            get
            {
                if (TotalQuestions == 0) return 0f;
                var questionProgress = (float)CurrentQuestionIndex / TotalQuestions;
                var charProgress = TotalCharacters > 0 ? (float)CurrentPosition / TotalCharacters / TotalQuestions : 0f;
                return questionProgress + charProgress;
            }
        }

        /// <summary>
        /// 正解率（0.0 - 1.0）
        /// </summary>
        public float Accuracy
        {
            get
            {
                var total = CorrectCount + MissCount;
                return total > 0 ? (float)CorrectCount / total : 1f;
            }
        }

        /// <summary>
        /// 現在の問題内での進捗率（0.0 - 1.0）
        /// </summary>
        public float CurrentQuestionProgressRate => TotalCharacters > 0 ? (float)CurrentPosition / TotalCharacters : 0f;

        public TypingProgress(
            int currentQuestionIndex,
            int totalQuestions,
            int currentPosition,
            int totalCharacters,
            int correctCount,
            int missCount,
            string typedText,
            string remainingText)
        {
            CurrentQuestionIndex = currentQuestionIndex;
            TotalQuestions = totalQuestions;
            CurrentPosition = currentPosition;
            TotalCharacters = totalCharacters;
            CorrectCount = correctCount;
            MissCount = missCount;
            TypedText = typedText;
            RemainingText = remainingText;
        }

        /// <summary>
        /// 初期状態の進捗を作成する
        /// </summary>
        public static TypingProgress Initial() => new TypingProgress(0, 0, 0, 0, 0, 0, string.Empty, string.Empty);
    }
}
