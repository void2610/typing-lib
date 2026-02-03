using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using R3;
using UnityEngine;
using VContainer.Unity;
using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Core.Models;
using Object = UnityEngine.Object;

namespace Void2610.TypingGame
{
    public class TypingGamePresenter : ITickable, IStartable, IDisposable
    {
        private readonly ITypingSession _session;
        private readonly IJapaneseInputValidator _japaneseValidator;
        private readonly TypingGameView _view;
        private readonly CompositeDisposable _disposables = new();

        private int _correctCount;
        private int _missCount;
        private int _questionCount;
        private int _currentQuestionIndex;

        public TypingGamePresenter(ITypingSession session, IJapaneseInputValidator japaneseValidator)
        {
            _view = Object.FindAnyObjectByType<TypingGameView>();

            _session = session;
            _japaneseValidator = japaneseValidator;

            _session.CurrentQuestion.Subscribe(OnQuestionChanged).AddTo(_disposables);
            _session.CurrentPosition.Subscribe(_ => UpdateDisplay()).AddTo(_disposables);
            _session.OnInput.Subscribe(OnInput).AddTo(_disposables);
            _session.OnQuestionCompleted.Subscribe(_ => OnQuestionCompleted()).AddTo(_disposables);
            _session.OnSessionCompleted.Subscribe(_ => OnSessionCompleted()).AddTo(_disposables);
        }

        public void Start()
        {
            _view.SetTypedText("");
            _view.SetRemainingText("Press any key to start");
            _view.SetStatus("");

            WaitAndStartSession().Forget();
        }

        public void Tick()
        {
            foreach (var c in Input.inputString)
            {
                if (char.IsControl(c)) continue;
                _session.ProcessInput(c);
            }
        }

        public void Dispose()
        {
            _disposables.Dispose();
            _session.Dispose();
        }

        private async UniTask WaitAndStartSession()
        {
            await UniTask.WaitUntil(() => Input.anyKeyDown);

            var questions = CreateSampleQuestions();
            _questionCount = questions.Count;
            _currentQuestionIndex = 0;
            _correctCount = 0;
            _missCount = 0;

            _session.StartSession(questions);
        }

        private void OnQuestionChanged(TypingQuestion question)
        {
            if (question == null) return;
            UpdateDisplay();
        }

        private void OnInput(InputResult result)
        {
            if (result.IsIgnored)
            {
                // 未確定入力でも表示を更新
                UpdateDisplay();
                return;
            }

            if (result.IsCorrect) _correctCount++;
            else _missCount++;
            UpdateDisplay();
        }

        private void OnQuestionCompleted()
        {
            _currentQuestionIndex++;
            // 問題が完了したらバッファをクリア
            _japaneseValidator.ClearBuffer();
        }

        private void OnSessionCompleted()
        {
            var accuracy = (_correctCount + _missCount) > 0
                ? (float)_correctCount / (_correctCount + _missCount)
                : 1f;

            _view.SetTypedText("Complete!");
            _view.SetRemainingText("Press any key to retry");
            _view.SetStatus($"正解: {_correctCount}  ミス: {_missCount}  正解率: {accuracy:P1}");
        }

        private void UpdateDisplay()
        {
            var question = _session.CurrentQuestion.CurrentValue;
            if (question == null) return;

            var pos = _session.CurrentPosition.CurrentValue;
            var inputText = question.InputText;
            var pendingRomaji = _japaneseValidator.PendingInput;

            // 入力済みのひらがな + 未確定のローマ字
            var typedText = inputText.Substring(0, pos) + pendingRomaji;
            _view.SetTypedText(typedText);

            // 残りのひらがな
            _view.SetRemainingText(pos < inputText.Length ? inputText.Substring(pos) : string.Empty);
            UpdateStatus();
        }

        private void UpdateStatus()
        {
            var accuracy = (_correctCount + _missCount) > 0
                ? (float)_correctCount / (_correctCount + _missCount)
                : 1f;

            _view.SetStatus($"{_currentQuestionIndex + 1}/{_questionCount}  正解率: {accuracy:P0}");
        }

        private List<TypingQuestion> CreateSampleQuestions()
        {
            return new List<TypingQuestion>
            {
                new("桜", "さくら"),
                new("東京", "とうきょう"),
                new("散歩", "さんぽ"),
                new("学校", "がっこう"),
                new("日本語", "にほんご"),
            };
        }
    }
}
