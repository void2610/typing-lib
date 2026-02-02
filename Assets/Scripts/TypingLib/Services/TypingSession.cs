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
        public ReadOnlyReactiveProperty<TypingProgress> Progress => _progress;
        public ReadOnlyReactiveProperty<SessionState> State => _state;

        public Observable<CharacterInputEvent> OnCharacterInput => _onCharacterInput;
        public Observable<QuestionCompletedEvent> OnQuestionCompleted => _onQuestionCompleted;
        public Observable<SessionCompletedEvent> OnSessionCompleted => _onSessionCompleted;

        private readonly IInputValidator _inputValidator;
        private readonly CompositeDisposable _disposables = new();

        private readonly ReactiveProperty<TypingQuestion> _currentQuestion = new(null);
        private readonly ReactiveProperty<TypingProgress> _progress = new(TypingProgress.Initial());
        private readonly ReactiveProperty<SessionState> _state = new(SessionState.Idle);

        private readonly Subject<CharacterInputEvent> _onCharacterInput = new();
        private readonly Subject<QuestionCompletedEvent> _onQuestionCompleted = new();
        private readonly Subject<SessionCompletedEvent> _onSessionCompleted = new();

        private List<TypingQuestion> _questions = new();
        private int _currentQuestionIndex;
        private int _currentPosition;
        private int _totalCorrectCount;
        private int _totalMissCount;
        private int _currentQuestionCorrectCount;
        private int _currentQuestionMissCount;

        public TypingSession(IInputValidator inputValidator)
        {
            _inputValidator = inputValidator;

            _disposables.Add(_currentQuestion);
            _disposables.Add(_progress);
            _disposables.Add(_state);
            _disposables.Add(_onCharacterInput);
            _disposables.Add(_onQuestionCompleted);
            _disposables.Add(_onSessionCompleted);
        }

        public void StartSession(IEnumerable<TypingQuestion> questions)
        {
            _questions = questions.ToList();

            _currentQuestionIndex = 0;
            _currentPosition = 0;
            _totalCorrectCount = 0;
            _totalMissCount = 0;
            _currentQuestionCorrectCount = 0;
            _currentQuestionMissCount = 0;

            _currentQuestion.Value = _questions[0];
            _state.Value = SessionState.Running;
            UpdateProgress();
        }

        public InputResult ProcessInput(char input)
        {
            if (_state.Value != SessionState.Running)
            {
                return InputResult.Ignored(input);
            }

            var currentQ = _currentQuestion.Value;
            if (_currentPosition >= currentQ.Length)
            {
                return InputResult.Ignored(input);
            }

            var expectedChar = currentQ.InputText[_currentPosition];
            var result = _inputValidator.Validate(input, expectedChar);

            if (result.IsCorrect)
            {
                _currentPosition++;
                _totalCorrectCount++;
                _currentQuestionCorrectCount++;
            }
            else
            {
                _totalMissCount++;
                _currentQuestionMissCount++;
            }

            UpdateProgress();

            _onCharacterInput.OnNext(new CharacterInputEvent(result, _progress.Value));

            if (result.IsCorrect && _currentPosition >= currentQ.Length)
            {
                OnCurrentQuestionCompleted();
            }

            return result;
        }

        public void Pause()
        {
            if (_state.Value == SessionState.Running)
            {
                _state.Value = SessionState.Paused;
            }
        }

        public void Resume()
        {
            if (_state.Value == SessionState.Paused)
            {
                _state.Value = SessionState.Running;
            }
        }

        public void EndSession()
        {
            if (_state.Value == SessionState.Idle || _state.Value == SessionState.Completed)
            {
                return;
            }

            CompleteSession();
        }

        public void SkipCurrentQuestion()
        {
            if (_state.Value != SessionState.Running && _state.Value != SessionState.Paused)
            {
                return;
            }

            MoveToNextQuestion();
        }

        public void Dispose()
        {
            _disposables.Dispose();
        }

        private void OnCurrentQuestionCompleted()
        {
            var completedQuestion = _currentQuestion.Value;
            var isLastQuestion = _currentQuestionIndex >= _questions.Count - 1;
            var nextIndex = _currentQuestionIndex + 1;

            _onQuestionCompleted.OnNext(new QuestionCompletedEvent(
                completedQuestion,
                _currentQuestionCorrectCount,
                _currentQuestionMissCount,
                nextIndex,
                isLastQuestion
            ));

            if (isLastQuestion)
            {
                CompleteSession();
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
                CompleteSession();
                return;
            }

            _currentPosition = 0;
            _currentQuestionCorrectCount = 0;
            _currentQuestionMissCount = 0;
            _currentQuestion.Value = _questions[_currentQuestionIndex];
            UpdateProgress();
        }

        private void CompleteSession()
        {
            _state.Value = SessionState.Completed;

            _onSessionCompleted.OnNext(new SessionCompletedEvent(
                _totalCorrectCount,
                _totalMissCount,
                _currentQuestionIndex + 1,
                _questions.Count
            ));
        }

        private void UpdateProgress()
        {
            var currentQ = _currentQuestion.Value;
            var inputText = currentQ.InputText;

            _progress.Value = new TypingProgress(
                _currentQuestionIndex,
                _questions.Count,
                _currentPosition,
                currentQ.Length,
                _totalCorrectCount,
                _totalMissCount,
                inputText.Substring(0, _currentPosition),
                _currentPosition < inputText.Length ? inputText.Substring(_currentPosition) : string.Empty
            );
        }
    }
}
