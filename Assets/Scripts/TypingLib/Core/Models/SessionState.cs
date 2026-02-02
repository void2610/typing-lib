namespace Void2610.TypingLib.Core.Models
{
    /// <summary>
    /// タイピングセッションの状態を表す列挙型
    /// </summary>
    public enum SessionState
    {
        /// <summary>
        /// 未開始
        /// </summary>
        Idle,

        /// <summary>
        /// 実行中
        /// </summary>
        Running,

        /// <summary>
        /// 一時停止中
        /// </summary>
        Paused,

        /// <summary>
        /// 完了
        /// </summary>
        Completed
    }
}
