# ThockKit

Unityで再利用可能なタイピングゲームライブラリ。MVP（Model-View-Presenter）パターンを採用し、コアロジックをライブラリとして提供します。

[![Unity Version](https://img.shields.io/badge/Unity-6000.0%2B-blue)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE.md)

## ✨ 特徴

- 🎯 **英語・日本語（ローマ字）入力に対応**
- 🏗️ **MVPパターン** - UI非依存の再利用可能な設計
- ⚡ **リアクティブな状態管理** - R3による効率的なイベント処理
- 🔌 **VContainer統合** - 依存性注入に対応
- 📦 **UPM対応** - Unity Package Managerで簡単インストール
- ✅ **完全なテストカバレッジ** - 172件のユニットテスト

## 📦 インストール

### Unity Package Manager経由（推奨）

1. Unity Editorを開く
2. `Window` → `Package Manager` を開く
3. `+` ボタン → `Add package from git URL...` を選択
4. 以下のURLを入力:

```
https://github.com/void2610/thock-kit.git?path=Assets/Scripts/ThockKit
```

### 依存パッケージ

以下のパッケージが自動的にインストールされます：

- [VContainer](https://github.com/hadashiA/VContainer) - 依存性注入
- [R3](https://github.com/Cysharp/R3) - リアクティブプログラミング

## 🚀 クイックスタート

### 1. 基本的な英語タイピング

```csharp
using VContainer;
using VContainer.Unity;
using Void2610.ThockKit.Core.Models;
using Void2610.ThockKit.Extensions;

public class MyLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // ThockKitを登録（英語入力）
        var settings = TypingSessionSettings.Default;
        builder.RegisterThockKit(settings);

        // Presenterを登録
        builder.RegisterEntryPoint<MyTypingPresenter>();
    }
}
```

### 2. 日本語（ローマ字）タイピング

```csharp
protected override void Configure(IContainerBuilder builder)
{
    // ThockKitを登録（日本語入力）
    var settings = TypingSessionSettings.Default;
    builder.RegisterThockKitJapanese(settings);

    builder.RegisterEntryPoint<MyTypingPresenter>();
}
```

### 3. Presenterの実装

```csharp
using VContainer.Unity;
using Void2610.ThockKit.Core.Interfaces;
using Void2610.ThockKit.Core.Models;

public class MyTypingPresenter : ITickable, IStartable
{
    private readonly ITypingSession _session;

    public MyTypingPresenter(ITypingSession session)
    {
        _session = session;

        // イベント購読
        _session.OnInput.Subscribe(OnInput);
        _session.OnSessionCompleted.Subscribe(_ => OnCompleted());
    }

    public void Start()
    {
        // 問題リストを作成
        var questions = new List<TypingQuestion>
        {
            new("hello"),
            new("world"),
        };

        _session.StartSession(questions);
    }

    public void Tick()
    {
        // キーボード入力を処理
        foreach (char c in Input.inputString)
        {
            _session.ProcessInput(c);
        }
    }

    private void OnInput(InputResult result)
    {
        if (result.IsCorrect)
        {
            Debug.Log("正解！");
        }
    }

    private void OnCompleted()
    {
        Debug.Log("セッション完了！");
    }
}
```

## 📚 主要なクラスとインターフェース

### ITypingSession

タイピングセッションの管理を行うインターフェース。

```csharp
public interface ITypingSession
{
    // 状態プロパティ
    ReadOnlyReactiveProperty<TypingQuestion> CurrentQuestion { get; }
    ReadOnlyReactiveProperty<int> CurrentPosition { get; }
    ReadOnlyReactiveProperty<SessionState> State { get; }
    char? ExpectedChar { get; }

    // イベント
    Observable<InputResult> OnInput { get; }
    Observable<TypingQuestion> OnQuestionCompleted { get; }
    Observable<Unit> OnSessionCompleted { get; }

    // メソッド
    void StartSession(IEnumerable<TypingQuestion> questions);
    InputResult ProcessInput(char input);
    void NextQuestion();
    void Pause();
    void Resume();
    void EndSession();
}
```

### TypingQuestion

タイピング問題を表すクラス。

```csharp
// 英語の場合
var question = new TypingQuestion("hello");

// 日本語の場合（表示テキスト、入力テキスト）
var question = new TypingQuestion("東京", "とうきょう");

// カスタムメタデータ付き
var metadata = new Dictionary<string, object> { { "difficulty", 3 } };
var question = new TypingQuestion("id1", "表示", "入力", metadata);
```

### TypingSessionSettings

セッション設定を表すクラス。

```csharp
// デフォルト設定
var settings = TypingSessionSettings.Default;

// 空白をスキップ
var settings = TypingSessionSettings.SkipWhitespaceOnly;

// 空白と記号をスキップ
var settings = TypingSessionSettings.SkipAll;

// カスタム設定
var settings = new TypingSessionSettings(
    skipWhitespace: true,
    skipSymbols: false,
    caseSensitive: false
);
```

## 🎮 サンプル

Package Managerからサンプルをインポートできます：

1. Package Managerでこのパッケージを選択
2. 「Samples」タブを開く
3. 「Basic English Typing」の「Import」をクリック

サンプルの詳細は [Samples~/README.md](Samples~/README.md) を参照してください。

## 📖 ドキュメント

### アーキテクチャ

このライブラリはMVPパターンを採用しています：

```
View (MonoBehaviour)     ← UI表示のみ担当
    ↑
Presenter (ITickable)    ← 入力処理、セッション管理、View更新指示
    ↓
Model (ThockKit)        ← ビジネスロジック（UI依存なし）
```

### 日本語入力の仕組み

日本語入力は`IJapaneseInputValidator`により、ローマ字からひらがなへの変換を自動的に処理します：

```csharp
// "きょう" と入力する場合
// k → Ignored（バッファに蓄積）
// y → Ignored（バッファに蓄積）
// o → Correct（"きょ"の2文字分消費、ConsumedCount=2）
// u → Correct（"う"の1文字分消費）
```

拗音（きゃ、しゃ等）や促音（っ）、「ん」の入力にも対応しています。

## 🧪 テスト

このライブラリには172件のユニットテストが含まれています（カバレッジ95%以上）。

テストを実行するには：

```bash
# Unity Test Runnerで実行
Window → General → Test Runner → PlayMode → Run All
```

## 🤝 コントリビューション

Issue、Pull Requestを歓迎します！

## 📄 ライセンス

MIT License - 詳細は [LICENSE.md](LICENSE.md) を参照してください。

## 🔗 関連リンク

- [GitHub Repository](https://github.com/void2610/thock-kit)
- [VContainer](https://github.com/hadashiA/VContainer)
- [R3](https://github.com/Cysharp/R3)
- [UniTask](https://github.com/Cysharp/UniTask)

## 変更履歴

変更履歴の詳細は [CHANGELOG.md](CHANGELOG.md) を参照してください。
