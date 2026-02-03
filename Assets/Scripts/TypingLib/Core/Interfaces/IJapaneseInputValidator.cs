namespace Void2610.TypingLib.Core.Interfaces
{
    /// <summary>
    /// 日本語入力検証インターフェース
    /// ローマ字入力をバッファリングし、ひらがなへの変換を検証する
    /// </summary>
    public interface IJapaneseInputValidator : IInputValidator
    {
        /// <summary>
        /// 未確定のローマ字バッファ
        /// </summary>
        string PendingInput { get; }

        /// <summary>
        /// バッファをクリアする
        /// </summary>
        void ClearBuffer();
    }
}
