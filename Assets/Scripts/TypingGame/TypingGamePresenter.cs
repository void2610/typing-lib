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
        private readonly TypingGameView _view;
        private readonly CompositeDisposable _disposables = new();

        private int _correctCount;
        private int _missCount;
        private int _questionCount;
        private int _currentQuestionIndex;

        public TypingGamePresenter(ITypingSession session)
        {
            _view = Object.FindAnyObjectByType<TypingGameView>();

            _session = session;

            _session.CurrentQuestion.Subscribe(OnQuestionChanged).AddTo(_disposables);
            _session.CurrentPosition.Subscribe(_ => UpdateDisplay()).AddTo(_disposables);
            _session.OnInput.Subscribe(OnInput).AddTo(_disposables);
            _session.OnQuestionCompleted.Subscribe(_ => _currentQuestionIndex++).AddTo(_disposables);
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
            if (result.IsIgnored) return;

            if (result.IsCorrect) _correctCount++;
            else _missCount++;
            UpdateStatus();
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
            var text = question.InputText;

            _view.SetTypedText(text.Substring(0, pos));
            _view.SetRemainingText(pos < text.Length ? text.Substring(pos) : string.Empty);
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
                new("Hello, World!"),
                new("The quick brown fox"),
                new("jumps over the lazy dog"),
                new("Programming is fun!"),
                new("Unity Game Engine"),
            };
        }
        
        public void Dispose()
        {
            _disposables.Dispose();
            _session.Dispose();
        }
    }
}
