using System.Collections.Generic;

namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// タイピング問題を表すクラス
    /// </summary>
    public class TypingQuestion
    {
        /// <summary>
        /// 問題ID
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// 表示テキスト（画面に表示される文字列）
        /// </summary>
        public string DisplayText { get; }

        /// <summary>
        /// 入力対象テキスト（実際にタイプする文字列）
        /// </summary>
        public string InputText { get; }

        /// <summary>
        /// カスタムメタデータ
        /// </summary>
        public IReadOnlyDictionary<string, object> Metadata { get; }

        /// <summary>
        /// 入力対象テキストの文字数
        /// </summary>
        public int Length => InputText.Length;

        public TypingQuestion(string id, string displayText, string inputText, Dictionary<string, object> metadata = null)
        {
            Id = id;
            DisplayText = displayText;
            InputText = inputText;
            Metadata = metadata ?? new Dictionary<string, object>();
        }

        /// <summary>
        /// 簡易コンストラクタ（IDは自動生成）
        /// </summary>
        public TypingQuestion(string displayText, string inputText)
            : this(System.Guid.NewGuid().ToString(), displayText, inputText)
        {
        }

        /// <summary>
        /// 表示テキストと入力テキストが同じ場合の簡易コンストラクタ
        /// </summary>
        public TypingQuestion(string text)
            : this(text, text)
        {
        }
    }
}
