namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// タイピングセッションの設定
    /// </summary>
    public readonly struct TypingSessionSettings
    {
        /// <summary>
        /// 空白文字をスキップするかどうか
        /// </summary>
        public bool SkipWhitespace { get; }

        /// <summary>
        /// 記号をスキップするかどうか
        /// </summary>
        public bool SkipSymbols { get; }

        /// <summary>
        /// 大文字小文字を区別するかどうか
        /// </summary>
        public bool CaseSensitive { get; }

        public TypingSessionSettings(bool skipWhitespace, bool skipSymbols, bool caseSensitive = true)
        {
            SkipWhitespace = skipWhitespace;
            SkipSymbols = skipSymbols;
            CaseSensitive = caseSensitive;
        }

        /// <summary>
        /// デフォルト設定（スキップなし、大文字小文字区別あり）
        /// </summary>
        public static TypingSessionSettings Default => new(false, false, true);

        /// <summary>
        /// 空白のみスキップする設定
        /// </summary>
        public static TypingSessionSettings SkipWhitespaceOnly => new(true, false, true);

        /// <summary>
        /// 空白と記号をスキップする設定
        /// </summary>
        public static TypingSessionSettings SkipAll => new(true, true, true);
    }
}
