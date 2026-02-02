using System;
using System.Collections.Generic;
using System.Linq;
using R3;
using Void2610.TypingLib.Core.Interfaces;
using Void2610.TypingLib.Core.Models;

namespace Void2610.TypingLib.Services
{
    /// <summary>
    /// タイピングセッション管理サービス
    /// </summary>
    public class TypingSession : ITypingSession
    {
        public ReadOnlyReactiveProperty<TypingQuestion> CurrentQuestion => _currentQuestion;
        public ReadOnlyReactiveProperty<int> CurrentPosition => _currentPosition;
        public ReadOnlyReactiveProperty<SessionState> State => _state;

        public char? ExpectedChar
        {
            get
            {
                var q = _currentQuestion.CurrentValue;
                var pos = _currentPosition.CurrentValue;
                if (q == null || pos >= q.Length) return null;
                return q.InputText[pos];
            }
        }

        public Observable<InputResult> OnInput => _onInput;
        public Observable<TypingQuestion> OnQuestionCompleted => _onQuestionCompleted;
        public Observable<Unit> OnSessionCompleted => _onSessionCompleted;

        private readonly IInputValidator _inputValidator;
        private readonly TypingSessionSettings _settings;
        private readonly CompositeDisposable _disposables = new();

        private readonly ReactiveProperty<TypingQuestion> _currentQuestion = new(null);
        private readonly ReactiveProperty<int> _currentPosition = new(0);
        private readonly ReactiveProperty<SessionState> _state = new(SessionState.Idle);

        private readonly Subject<InputResult> _onInput = new();
        private readonly Subject<TypingQuestion> _onQuestionCompleted = new();
        private readonly Subject<Unit> _onSessionCompleted = new();

        private List<TypingQuestion> _questions = new();
        private int _currentQuestionIndex;

        public TypingSession(IInputValidator inputValidator) : this(inputValidator, TypingSessionSettings.Default)
        {
        }

        public TypingSession(IInputValidator inputValidator, TypingSessionSettings settings)
        {
            _inputValidator = inputValidator;
            _settings = settings;
            _inputValidator.IsCaseSensitive = settings.CaseSensitive;

            _disposables.Add(_currentQuestion);
            _disposables.Add(_currentPosition);
            _disposables.Add(_state);
            _disposables.Add(_onInput);
            _disposables.Add(_onQuestionCompleted);
            _disposables.Add(_onSessionCompleted);
        }

        public void StartSession(IEnumerable<TypingQuestion> questions)
        {
            _questions = questions.ToList();
            _currentQuestionIndex = 0;
            _currentPosition.Value = 0;
            _currentQuestion.Value = _questions[0];
            _state.Value = SessionState.Running;

            SkipCharactersAtCurrentPosition();
        }

        public InputResult ProcessInput(char input)
        {
            if (_state.CurrentValue != SessionState.Running)
            {
                return InputResult.Ignored(input);
            }

            var currentQ = _currentQuestion.CurrentValue;
            if (_currentPosition.CurrentValue >= currentQ.Length)
            {
                return InputResult.Ignored(input);
            }

            var expectedChar = currentQ.InputText[_currentPosition.CurrentValue];
            var result = _inputValidator.Validate(input, expectedChar);

            if (result.IsCorrect)
            {
                _currentPosition.Value++;
                SkipCharactersAtCurrentPosition();

                if (_currentPosition.CurrentValue >= currentQ.Length)
                {
                    HandleQuestionCompleted();
                }
            }

            _onInput.OnNext(result);
            return result;
        }

        public void NextQuestion()
        {
            if (_state.CurrentValue != SessionState.Running && _state.CurrentValue != SessionState.Paused)
            {
                return;
            }

            MoveToNextQuestion();
        }

        public void Pause()
        {
            if (_state.CurrentValue == SessionState.Running)
            {
                _state.Value = SessionState.Paused;
            }
        }

        public void Resume()
        {
            if (_state.CurrentValue == SessionState.Paused)
            {
                _state.Value = SessionState.Running;
            }
        }

        public void EndSession()
        {
            if (_state.CurrentValue is SessionState.Idle or SessionState.Completed)
            {
                return;
            }

            _state.Value = SessionState.Completed;
            _onSessionCompleted.OnNext(Unit.Default);
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void SkipCharactersAtCurrentPosition()
        {
            var currentQ = _currentQuestion.CurrentValue;
            while (_currentPosition.CurrentValue < currentQ.Length && ShouldSkip(currentQ.InputText[_currentPosition.CurrentValue]))
            {
                _currentPosition.Value++;
            }
        }

        private bool ShouldSkip(char c)
        {
            if (_settings.SkipWhitespace && char.IsWhiteSpace(c))
            {
                return true;
            }

            if (_settings.SkipSymbols && IsSymbol(c))
            {
                return true;
            }

            return false;
        }

        private static bool IsSymbol(char c) => char.IsPunctuation(c) || char.IsSymbol(c);

        private void HandleQuestionCompleted()
        {
            var completedQuestion = _currentQuestion.CurrentValue;
            var isLastQuestion = _currentQuestionIndex >= _questions.Count - 1;

            _onQuestionCompleted.OnNext(completedQuestion);

            if (isLastQuestion)
            {
                _state.Value = SessionState.Completed;
                _onSessionCompleted.OnNext(Unit.Default);
            }
            else
            {
                MoveToNextQuestion();
            }
        }

        private void MoveToNextQuestion()
        {
            _currentQuestionIndex++;

            if (_currentQuestionIndex >= _questions.Count)
            {
                _state.Value = SessionState.Completed;
                _onSessionCompleted.OnNext(Unit.Default);
                return;
            }

            _currentPosition.Value = 0;
            _currentQuestion.Value = _questions[_currentQuestionIndex];
            SkipCharactersAtCurrentPosition();
        }
    }
}
