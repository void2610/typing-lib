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

        public TypingGamePresenter(ITypingSession session)
        {
            _view = Object.FindAnyObjectByType<TypingGameView>();
            
            _session = session;
            _session.Progress.Subscribe(OnProgressChanged).AddTo(_disposables);
            _session.OnSessionCompleted.Subscribe(OnSessionCompleted).AddTo(_disposables);
        }

        private async UniTask WaitAndStartSession()
        {
            await UniTask.WaitUntil(() => Input.anyKeyDown);
            _session.StartSession(CreateSampleQuestions());
        }
        
        private void OnProgressChanged(TypingProgress progress)
        {
            _view.SetTypedText(progress.TypedText);
            _view.SetRemainingText(progress.RemainingText);
            _view.SetStatus($"{progress.CurrentQuestionIndex + 1}/{progress.TotalQuestions}  正解率: {progress.Accuracy:P0}");
        }

        private void OnSessionCompleted(SessionCompletedEvent e)
        {
            _view.SetTypedText("Complete!");
            _view.SetRemainingText("Press any key to retry");
            _view.SetStatus($"正解: {e.TotalCorrectCount}  ミス: {e.TotalMissCount}  正解率: {e.Accuracy:P1}");
        }

        private IEnumerable<TypingQuestion> CreateSampleQuestions()
        {
            return new[]
            {
                new TypingQuestion("Hello, World!"),
                new TypingQuestion("The quick brown fox"),
                new TypingQuestion("jumps over the lazy dog"),
                new TypingQuestion("Programming is fun!"),
                new TypingQuestion("Unity Game Engine"),
            };
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
    }
}
