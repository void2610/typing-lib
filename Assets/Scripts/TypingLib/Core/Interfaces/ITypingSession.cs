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
        /// <summary>
        /// 現在の問題
        /// </summary>
        ReadOnlyReactiveProperty<TypingQuestion> CurrentQuestion { get; }

        /// <summary>
        /// 現在の進捗
        /// </summary>
        ReadOnlyReactiveProperty<TypingProgress> Progress { get; }

        /// <summary>
        /// セッションの状態
        /// </summary>
        ReadOnlyReactiveProperty<SessionState> State { get; }

        /// <summary>
        /// 文字入力イベントストリーム
        /// </summary>
        Observable<CharacterInputEvent> OnCharacterInput { get; }

        /// <summary>
        /// 問題完了イベントストリーム
        /// </summary>
        Observable<QuestionCompletedEvent> OnQuestionCompleted { get; }

        /// <summary>
        /// セッション完了イベントストリーム
        /// </summary>
        Observable<SessionCompletedEvent> OnSessionCompleted { get; }

        /// <summary>
        /// セッションを開始する
        /// </summary>
        /// <param name="questions">タイピング問題のリスト</param>
        void StartSession(IEnumerable<TypingQuestion> questions);

        /// <summary>
        /// 入力を処理する
        /// </summary>
        /// <param name="input">入力文字</param>
        /// <returns>入力結果</returns>
        InputResult ProcessInput(char input);

        /// <summary>
        /// セッションを一時停止する
        /// </summary>
        void Pause();

        /// <summary>
        /// セッションを再開する
        /// </summary>
        void Resume();

        /// <summary>
        /// セッションを終了する
        /// </summary>
        void EndSession();

        /// <summary>
        /// 現在の問題をスキップする
        /// </summary>
        void SkipCurrentQuestion();
    }
}
