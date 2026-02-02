namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// 文字入力イベント
    /// </summary>
    public readonly struct CharacterInputEvent
    {
        /// <summary>
        /// 入力結果
        /// </summary>
        public InputResult Result { get; }

        /// <summary>
        /// 現在の進捗
        /// </summary>
        public TypingProgress Progress { get; }

        public CharacterInputEvent(InputResult result, TypingProgress progress)
        {
            Result = result;
            Progress = progress;
        }
    }

    /// <summary>
    /// 問題完了イベント
    /// </summary>
    public readonly struct QuestionCompletedEvent
    {
        /// <summary>
        /// 完了した問題
        /// </summary>
        public TypingQuestion Question { get; }

        /// <summary>
        /// この問題での正解数
        /// </summary>
        public int CorrectCount { get; }

        /// <summary>
        /// この問題でのミス数
        /// </summary>
        public int MissCount { get; }

        /// <summary>
        /// 次の問題インデックス
        /// </summary>
        public int NextQuestionIndex { get; }

        /// <summary>
        /// 最後の問題かどうか
        /// </summary>
        public bool IsLastQuestion { get; }

        public QuestionCompletedEvent(
            TypingQuestion question,
            int correctCount,
            int missCount,
            int nextQuestionIndex,
            bool isLastQuestion)
        {
            Question = question;
            CorrectCount = correctCount;
            MissCount = missCount;
            NextQuestionIndex = nextQuestionIndex;
            IsLastQuestion = isLastQuestion;
        }
    }

    /// <summary>
    /// セッション完了イベント
    /// </summary>
    public readonly struct SessionCompletedEvent
    {
        /// <summary>
        /// 総正解数
        /// </summary>
        public int TotalCorrectCount { get; }

        /// <summary>
        /// 総ミス数
        /// </summary>
        public int TotalMissCount { get; }

        /// <summary>
        /// 完了した問題数
        /// </summary>
        public int CompletedQuestions { get; }

        /// <summary>
        /// 総問題数
        /// </summary>
        public int TotalQuestions { get; }

        /// <summary>
        /// 正解率（0.0 - 1.0）
        /// </summary>
        public float Accuracy
        {
            get
            {
                var total = TotalCorrectCount + TotalMissCount;
                return total > 0 ? (float)TotalCorrectCount / total : 1f;
            }
        }

        public SessionCompletedEvent(
            int totalCorrectCount,
            int totalMissCount,
            int completedQuestions,
            int totalQuestions)
        {
            TotalCorrectCount = totalCorrectCount;
            TotalMissCount = totalMissCount;
            CompletedQuestions = completedQuestions;
            TotalQuestions = totalQuestions;
        }
    }
}
