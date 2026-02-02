using System;
using System.Collections.Generic;
using R3;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Core.Interfaces
{
    /// <summary>
    /// タイピングセッション管理インターフェース
    /// </summary>
    public interface ITypingSession : IDisposable
    {
        ReadOnlyReactiveProperty<TypingQuestion> CurrentQuestion { get; }
        ReadOnlyReactiveProperty<int> CurrentPosition { get; }
        ReadOnlyReactiveProperty<SessionState> State { get; }

        /// <summary>
        /// 現在期待される文字（セッション未開始や完了時はnull）
        /// </summary>
        char? ExpectedChar { get; }

        /// <summary>
        /// 入力イベント（正誤判定結果）
        /// </summary>
        Observable<InputResult> OnInput { get; }

        /// <summary>
        /// 問題完了イベント（完了した問題を通知）
        /// </summary>
        Observable<TypingQuestion> OnQuestionCompleted { get; }

        /// <summary>
        /// セッション完了イベント
        /// </summary>
        Observable<Unit> OnSessionCompleted { get; }

        void StartSession(IEnumerable<TypingQuestion> questions);
        InputResult ProcessInput(char input);
        void NextQuestion();
        void Pause();
        void Resume();
        void EndSession();
    }
}
