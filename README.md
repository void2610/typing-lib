# Void2610 Typing Library

[![Unity Version](https://img.shields.io/badge/Unity-6000.0%2B-blue)](https://unity.com/)
[![License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![Test Coverage](https://img.shields.io/badge/Coverage-95%25-brightgreen.svg)](Assets/Tests/)

Unityで再利用可能なタイピングゲームライブラリ。MVP（Model-View-Presenter）パターンを採用し、英語・日本語（ローマ字）入力に対応。

## ✨ 特徴

- 🎯 **英語・日本語（ローマ字）入力に対応** - 拗音・促音・「ん」も完全サポート
- 🏗️ **MVPパターン** - UI非依存の再利用可能な設計
- ⚡ **リアクティブな状態管理** - R3による効率的なイベント処理
- 🔌 **VContainer統合** - 依存性注入に対応
- 📦 **UPM対応** - Unity Package Managerで簡単インストール
- ✅ **完全なテストカバレッジ** - 172件のユニットテスト（95%以上）

## 📦 インストール

### Unity Package Manager経由（推奨）

1. Unity Editorを開く
2. `Window` → `Package Manager` を開く
3. `+` ボタン → `Add package from git URL...` を選択
4. 以下のURLを入力:

```
https://github.com/void2610/typing-lib.git?path=Assets/Scripts/TypingLib
```

### 依存パッケージ

以下のパッケージが自動的にインストールされます：

- [VContainer](https://github.com/hadashiA/VContainer) 1.15.4+ - 依存性注入
- [R3](https://github.com/Cysharp/R3) 1.2.0+ - リアクティブプログラミング

## 🚀 クイックスタート

### 基本的な英語タイピング

```csharp
using VContainer;
using VContainer.Unity;
using Void2610.TypingLib.Core.Models;
using Void2610.TypingLib.Extensions;

public class TypingLifetimeScope : LifetimeScope
{
    protected override void Configure(IContainerBuilder builder)
    {
        // TypingLibを登録（英語入力）
        builder.RegisterTypingLib(TypingSessionSettings.Default);

        // Presenterを登録
        builder.RegisterEntryPoint<TypingPresenter>();
    }
}
```

### 日本語（ローマ字）タイピング

```csharp
protected override void Configure(IContainerBuilder builder)
{
    // TypingLibを登録（日本語入力）
    builder.RegisterTypingLibJapanese(TypingSessionSettings.Default);

    builder.RegisterEntryPoint<TypingPresenter>();
}
```

詳細は [パッケージドキュメント](Assets/Scripts/TypingLib/README.md) を参照してください。

## 📚 ドキュメント

- **[パッケージREADME](Assets/Scripts/TypingLib/README.md)** - 完全なAPI説明とクイックスタート
- **[CHANGELOG](Assets/Scripts/TypingLib/CHANGELOG.md)** - バージョン履歴
- **[サンプル](Assets/Scripts/TypingLib/Samples~/README.md)** - 実装例とセットアップガイド

## 🎮 サンプル

Package Managerからサンプルをインポートできます：

### Basic English Typing
- 英語専用のシンプルなタイピングゲーム
- Unity UI (uGUI) 使用
- 詳細なセットアップガイド付き

## 🏗️ アーキテクチャ

このライブラリはMVPパターンを採用しています：

```
View (MonoBehaviour)     ← UI表示のみ担当
    ↑
Presenter (ITickable)    ← 入力処理、セッション管理、View更新指示
    ↓
Model (TypingLib)        ← ビジネスロジック（UI依存なし）
```

### ディレクトリ構造

```
Assets/Scripts/TypingLib/          # パッケージルート
├── Core/                          # コアライブラリ
│   ├── Interfaces/               # インターフェース定義
│   │   ├── ITypingSession.cs
│   │   ├── IInputValidator.cs
│   │   └── IJapaneseInputValidator.cs
│   └── Models/                   # データモデル
│       ├── TypingQuestion.cs
│       ├── InputResult.cs
│       ├── SessionState.cs
│       └── TypingSessionSettings.cs
├── Services/                     # サービス実装
│   ├── TypingSession.cs
│   ├── EnglishInputValidator.cs
│   ├── JapaneseInputValidator.cs
│   └── RomajiTable.cs
├── Extensions/                   # 拡張メソッド
│   └── VContainerExtensions.cs
└── Samples~/                     # サンプル実装
    └── BasicEnglishTyping/
```

## 🧪 テスト

172件のユニットテストが含まれています（カバレッジ95%以上）。

```
Assets/Tests/Runtime/
├── Models/                # モデルのテスト
├── Services/              # サービスのテスト
└── ...
```

テストを実行するには：
1. Unity Editorで `Window` → `General` → `Test Runner` を開く
2. `PlayMode` タブを選択
3. `Run All` をクリック

## 📊 主要な機能

### 英語入力
- 大文字小文字の区別設定
- 空白・記号のスキップ機能

### 日本語入力
- ローマ字→ひらがな自動変換
- 拗音対応（きゃ、しゃ、ちゃ等）
- 促音対応（っ）
- 「ん」の処理（nn, n+子音）
- 入力バッファリング

### セッション管理
- 状態管理（Idle, Running, Paused, Completed）
- 一時停止/再開
- 問題のスキップ
- 進捗追跡
- リアクティブなイベント通知

## 🔧 技術スタック

- **Unity** 6000.0以上
- **.NET Framework** 4.7.1
- **C#** 9.0
- **VContainer** - 依存性注入
- **R3** - リアクティブプログラミング

## 🤝 コントリビューション

Issue、Pull Requestを歓迎します！

1. このリポジトリをフォーク
2. フィーチャーブランチを作成 (`git checkout -b feature/amazing-feature`)
3. 変更をコミット (`git commit -m 'Add amazing feature'`)
4. ブランチにプッシュ (`git push origin feature/amazing-feature`)
5. Pull Requestを作成

## 📄 ライセンス

このプロジェクトはMITライセンスの下で公開されています。詳細は [LICENSE](Assets/Scripts/TypingLib/LICENSE.md) を参照してください。

## 🔗 関連リンク

- [パッケージドキュメント](Assets/Scripts/TypingLib/README.md)
- [VContainer](https://github.com/hadashiA/VContainer)
- [R3](https://github.com/Cysharp/R3)
- [UniTask](https://github.com/Cysharp/UniTask)

## 📮 サポート

問題や質問がある場合は、[Issues](https://github.com/void2610/typing-lib/issues) でお知らせください。

---

Made with ❤️ by [void2610](https://github.com/void2610)
