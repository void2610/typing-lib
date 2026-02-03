using System.Collections.Generic;
using NUnit.Framework;
using R3;
using Void2610.TypingLib.Core.Models;
using Void2610.TypingLib.Services;

namespace Void2610.TypingLib.Tests.Services
{
    /// <summary>
    /// TypingSession のユニットテスト
    /// </summary>
    [TestFixture]
    public class TypingSessionTests
    {
        private TypingSession _session;
        private EnglishInputValidator _validator;

        [SetUp]
        public void SetUp()
        {
            _validator = new EnglishInputValidator();
            _session = new TypingSession(_validator);
        }

        [TearDown]
        public void TearDown()
        {
            _session?.Dispose();
        }

        #region 状態遷移テスト

        [Test]
        public void State_初期状態はIdle()
        {
            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Idle));
        }

        [Test]
        public void StartSession_状態がRunningになる()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };

            // Act
            _session.StartSession(questions);

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Running));
        }

        [Test]
        public void Pause_Running状態からPausedになる()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            // Act
            _session.Pause();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Paused));
        }

        [Test]
        public void Resume_Paused状態からRunningになる()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);
            _session.Pause();

            // Act
            _session.Resume();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Running));
        }

        [Test]
        public void EndSession_状態がCompletedになる()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            // Act
            _session.EndSession();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Completed));
        }

        [Test]
        public void Pause_Idle状態では変化なし()
        {
            // Act
            _session.Pause();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Idle));
        }

        [Test]
        public void Resume_Idle状態では変化なし()
        {
            // Act
            _session.Resume();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Idle));
        }

        #endregion

        #region ProcessInput テスト

        [Test]
        public void ProcessInput_正しい入力_Correctを返す()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            // Act
            var result = _session.ProcessInput('h');

            // Assert
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(result.InputChar, Is.EqualTo('h'));
        }

        [Test]
        public void ProcessInput_間違った入力_Incorrectを返す()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            // Act
            var result = _session.ProcessInput('x');

            // Assert
            Assert.That(result.IsCorrect, Is.False);
            Assert.That(result.InputChar, Is.EqualTo('x'));
            Assert.That(result.ExpectedChar, Is.EqualTo('h'));
        }

        [Test]
        public void ProcessInput_Idle状態_Ignoredを返す()
        {
            // Act
            var result = _session.ProcessInput('a');

            // Assert
            Assert.That(result.IsIgnored, Is.True);
        }

        [Test]
        public void ProcessInput_Paused状態_Ignoredを返す()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);
            _session.Pause();

            // Act
            var result = _session.ProcessInput('h');

            // Assert
            Assert.That(result.IsIgnored, Is.True);
        }

        [Test]
        public void ProcessInput_正しい入力_CurrentPositionが進む()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);
            Assert.That(_session.CurrentPosition.CurrentValue, Is.EqualTo(0));

            // Act
            _session.ProcessInput('h');

            // Assert
            Assert.That(_session.CurrentPosition.CurrentValue, Is.EqualTo(1));
        }

        [Test]
        public void ProcessInput_間違った入力_CurrentPositionは進まない()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);
            Assert.That(_session.CurrentPosition.CurrentValue, Is.EqualTo(0));

            // Act
            _session.ProcessInput('x');

            // Assert
            Assert.That(_session.CurrentPosition.CurrentValue, Is.EqualTo(0));
        }

        #endregion

        #region ExpectedChar テスト

        [Test]
        public void ExpectedChar_セッション開始前_nullを返す()
        {
            // Assert
            Assert.That(_session.ExpectedChar, Is.Null);
        }

        [Test]
        public void ExpectedChar_セッション中_現在位置の文字を返す()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            // Assert
            Assert.That(_session.ExpectedChar, Is.EqualTo('h'));
        }

        [Test]
        public void ExpectedChar_入力後_次の文字を返す()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);
            _session.ProcessInput('h');

            // Assert
            Assert.That(_session.ExpectedChar, Is.EqualTo('e'));
        }

        #endregion

        #region スキップ機能テスト

        [Test]
        public void SkipWhitespace_空白がスキップされる()
        {
            // Arrange
            var settings = TypingSessionSettings.SkipWhitespaceOnly;
            var session = new TypingSession(_validator, settings);
            var questions = new List<TypingQuestion> { new TypingQuestion("a b") };
            session.StartSession(questions);

            // Act
            session.ProcessInput('a');

            // Assert
            // 空白がスキップされて「b」が期待される
            Assert.That(session.ExpectedChar, Is.EqualTo('b'));

            session.Dispose();
        }

        [Test]
        public void SkipSymbols_記号がスキップされる()
        {
            // Arrange
            var settings = TypingSessionSettings.SkipAll;
            var session = new TypingSession(_validator, settings);
            var questions = new List<TypingQuestion> { new TypingQuestion("a!b") };
            session.StartSession(questions);

            // Act
            session.ProcessInput('a');

            // Assert
            // 記号がスキップされて「b」が期待される
            Assert.That(session.ExpectedChar, Is.EqualTo('b'));

            session.Dispose();
        }

        [Test]
        public void SkipWhitespace_開始位置で空白がスキップされる()
        {
            // Arrange
            var settings = TypingSessionSettings.SkipWhitespaceOnly;
            var session = new TypingSession(_validator, settings);
            var questions = new List<TypingQuestion> { new TypingQuestion(" ab") };
            session.StartSession(questions);

            // Assert
            // 開始位置の空白がスキップされて「a」が期待される
            Assert.That(session.ExpectedChar, Is.EqualTo('a'));

            session.Dispose();
        }

        #endregion

        #region イベント発火テスト

        [Test]
        public void OnInput_入力時にイベントが発火する()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            InputResult? receivedResult = null;
            using var subscription = _session.OnInput.Subscribe(r => receivedResult = r);

            // Act
            _session.ProcessInput('h');

            // Assert
            Assert.That(receivedResult, Is.Not.Null);
            Assert.That(receivedResult.Value.IsCorrect, Is.True);
        }

        [Test]
        public void OnQuestionCompleted_問題完了時にイベントが発火する()
        {
            // Arrange
            var questions = new List<TypingQuestion>
            {
                new TypingQuestion("ab"),
                new TypingQuestion("cd")
            };
            _session.StartSession(questions);

            TypingQuestion completedQuestion = null;
            using var subscription = _session.OnQuestionCompleted.Subscribe(q => completedQuestion = q);

            // Act
            _session.ProcessInput('a');
            _session.ProcessInput('b');

            // Assert
            Assert.That(completedQuestion, Is.Not.Null);
            Assert.That(completedQuestion.InputText, Is.EqualTo("ab"));
        }

        [Test]
        public void OnSessionCompleted_セッション完了時にイベントが発火する()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("ab") };
            _session.StartSession(questions);

            var sessionCompleted = false;
            using var subscription = _session.OnSessionCompleted.Subscribe(_ => sessionCompleted = true);

            // Act
            _session.ProcessInput('a');
            _session.ProcessInput('b');

            // Assert
            Assert.That(sessionCompleted, Is.True);
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Completed));
        }

        [Test]
        public void OnSessionCompleted_EndSession呼び出し時にイベントが発火する()
        {
            // Arrange
            var questions = new List<TypingQuestion> { new TypingQuestion("hello") };
            _session.StartSession(questions);

            var sessionCompleted = false;
            using var subscription = _session.OnSessionCompleted.Subscribe(_ => sessionCompleted = true);

            // Act
            _session.EndSession();

            // Assert
            Assert.That(sessionCompleted, Is.True);
        }

        #endregion

        #region CurrentQuestion テスト

        [Test]
        public void CurrentQuestion_セッション開始前_nullを返す()
        {
            // Assert
            Assert.That(_session.CurrentQuestion.CurrentValue, Is.Null);
        }

        [Test]
        public void CurrentQuestion_セッション開始後_最初の問題を返す()
        {
            // Arrange
            var question = new TypingQuestion("hello");
            var questions = new List<TypingQuestion> { question };
            _session.StartSession(questions);

            // Assert
            Assert.That(_session.CurrentQuestion.CurrentValue, Is.EqualTo(question));
        }

        [Test]
        public void CurrentQuestion_問題完了後_次の問題に切り替わる()
        {
            // Arrange
            var question1 = new TypingQuestion("ab");
            var question2 = new TypingQuestion("cd");
            var questions = new List<TypingQuestion> { question1, question2 };
            _session.StartSession(questions);

            // Act
            _session.ProcessInput('a');
            _session.ProcessInput('b');

            // Assert
            Assert.That(_session.CurrentQuestion.CurrentValue, Is.EqualTo(question2));
        }

        #endregion

        #region NextQuestion テスト

        [Test]
        public void NextQuestion_次の問題に切り替わる()
        {
            // Arrange
            var question1 = new TypingQuestion("ab");
            var question2 = new TypingQuestion("cd");
            var questions = new List<TypingQuestion> { question1, question2 };
            _session.StartSession(questions);

            // Act
            _session.NextQuestion();

            // Assert
            Assert.That(_session.CurrentQuestion.CurrentValue, Is.EqualTo(question2));
            Assert.That(_session.CurrentPosition.CurrentValue, Is.EqualTo(0));
        }

        [Test]
        public void NextQuestion_最後の問題で呼ぶとセッション完了()
        {
            // Arrange
            var question = new TypingQuestion("ab");
            var questions = new List<TypingQuestion> { question };
            _session.StartSession(questions);

            // Act
            _session.NextQuestion();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Completed));
        }

        [Test]
        public void NextQuestion_Idle状態では何も起きない()
        {
            // Arrange
            var question1 = new TypingQuestion("ab");
            var question2 = new TypingQuestion("cd");
            var questions = new List<TypingQuestion> { question1, question2 };

            // Act (セッション開始前に呼び出し)
            _session.NextQuestion();

            // Assert
            Assert.That(_session.State.CurrentValue, Is.EqualTo(SessionState.Idle));
            Assert.That(_session.CurrentQuestion.CurrentValue, Is.Null);
        }

        #endregion

        #region 大文字小文字区別テスト

        [Test]
        public void CaseSensitive_設定がValidatorに反映される()
        {
            // Arrange
            var settings = new TypingSessionSettings(false, false, false);
            var session = new TypingSession(_validator, settings);
            var questions = new List<TypingQuestion> { new TypingQuestion("Hello") };
            session.StartSession(questions);

            // Act
            var result = session.ProcessInput('h');

            // Assert (大文字小文字区別なしなので正解)
            Assert.That(result.IsCorrect, Is.True);

            session.Dispose();
        }

        #endregion

        #region 日本語入力（拗音）テスト

        [Test]
        public void ProcessInput_拗音_位置が2文字分進む()
        {
            // Arrange
            var japaneseValidator = new JapaneseInputValidator();
            var session = new TypingSession(japaneseValidator);
            // 「きょう」= 3文字（き、ょ、う）
            var questions = new List<TypingQuestion> { new TypingQuestion("きょう", "きょう") };
            session.StartSession(questions);

            // Act: kyo を入力
            session.ProcessInput('k');
            session.ProcessInput('y');
            var result = session.ProcessInput('o');

            // Assert: 「きょ」の2文字分が消費されて位置が2になる
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(session.CurrentPosition.CurrentValue, Is.EqualTo(2));
            Assert.That(session.ExpectedChar, Is.EqualTo('う'));

            session.Dispose();
        }

        [Test]
        public void ProcessInput_拗音含む文字列_正しく完了する()
        {
            // Arrange
            var japaneseValidator = new JapaneseInputValidator();
            var session = new TypingSession(japaneseValidator);
            var questions = new List<TypingQuestion> { new TypingQuestion("きょう", "きょう") };
            session.StartSession(questions);

            var completed = false;
            using var subscription = session.OnSessionCompleted.Subscribe(_ => completed = true);

            // Act: kyou を入力
            session.ProcessInput('k');
            session.ProcessInput('y');
            session.ProcessInput('o');
            session.ProcessInput('u');

            // Assert
            Assert.That(completed, Is.True);
            Assert.That(session.State.CurrentValue, Is.EqualTo(SessionState.Completed));

            session.Dispose();
        }

        [Test]
        public void ProcessInput_しゃ行_位置が2文字分進む()
        {
            // Arrange
            var japaneseValidator = new JapaneseInputValidator();
            var session = new TypingSession(japaneseValidator);
            var questions = new List<TypingQuestion> { new TypingQuestion("しゃしん", "しゃしん") };
            session.StartSession(questions);

            // Act: sha を入力
            session.ProcessInput('s');
            session.ProcessInput('h');
            var result = session.ProcessInput('a');

            // Assert: 「しゃ」の2文字分が消費されて位置が2になる
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(session.CurrentPosition.CurrentValue, Is.EqualTo(2));
            Assert.That(session.ExpectedChar, Is.EqualTo('し'));

            session.Dispose();
        }

        [Test]
        public void ProcessInput_日本語単純な入力_位置が1文字分進む()
        {
            // Arrange
            var japaneseValidator = new JapaneseInputValidator();
            var session = new TypingSession(japaneseValidator);
            var questions = new List<TypingQuestion> { new TypingQuestion("かき", "かき") };
            session.StartSession(questions);

            // Act: ka を入力
            session.ProcessInput('k');
            var result = session.ProcessInput('a');

            // Assert: 「か」の1文字分が消費されて位置が1になる
            Assert.That(result.IsCorrect, Is.True);
            Assert.That(session.CurrentPosition.CurrentValue, Is.EqualTo(1));
            Assert.That(session.ExpectedChar, Is.EqualTo('き'));

            session.Dispose();
        }

        #endregion
    }
}
