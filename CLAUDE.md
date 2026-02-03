# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## プロジェクト概要

Unityで再利用可能なタイピングゲームライブラリ。MVP（Model-View-Presenter）パターンを採用し、コア部分をライブラリとして提供。ゲームロジックは各プロジェクト側で実装する設計。

## コーディング規約

- **コメント:** プログラム内の全てのコメントは日本語で記述
- **命名規則:**
  - クラス/インターフェース/メソッド/プロパティ: PascalCase
  - プライベートフィールド: _camelCase（例: `_inputBuffer`）

## アーキテクチャ

### ディレクトリ構造

```
Assets/Scripts/
├── TypingLib/          # メインライブラリ（ゲーム実装から独立）
│   ├── Core/
│   │   ├── Models/     # TypingQuestion, SessionState, InputResult, TypingSessionSettings
│   │   └── Interfaces/ # ITypingSession, IInputValidator, IJapaneseInputValidator
│   └── Services/       # TypingSession, EnglishInputValidator, JapaneseInputValidator, RomajiTable
├── TypingGame/         # テスト実装用（Presenter/View）
└── VContainer/         # DI設定（LifetimeScope）
```

### MVP構成

```
View (MonoBehaviour)     ← UI表示のみ担当
    ↑
Presenter (ITickable)    ← 入力処理、セッション管理、View更新指示
    ↓
Model (TypingLib)        ← ビジネスロジック（UI依存なし）
```

- **Model（TypingLib）:** `ITypingSession`でセッション管理、R3のReactivePropertyで状態変更を通知
- **View（TypingGame）:** `SetTypedText()`, `SetRemainingText()`, `SetStatus()`の3メソッドでUI更新
- **Presenter（TypingGame）:** VContainerの`ITickable`/`IStartable`を実装、入力イベントを購読してViewを更新

### 主要インターフェース

- `ITypingSession`: セッション管理（開始/終了/一時停止、入力処理、状態監視）
- `IInputValidator`: 1文字入力検証
- `IJapaneseInputValidator`: ローマ字→ひらがな変換検証（バッファリング機能付き）

### DI登録（VContainer）

```csharp
// 英語入力用
builder.RegisterTypingLib(settings);

// 日本語入力用
builder.RegisterTypingLibJapanese(settings);
```

## 外部依存関係

| パッケージ | 用途 |
|----------|------|
| R3 | リアクティブプログラミング（状態管理） |
| VContainer | 依存性注入 |
| UniTask | 非同期処理 |
| Unity Input System | キーボード入力（`Keyboard.onTextInput`） |
| TextMeshPro | UI表示 |

## ビルド

Unity 6000.0系、.NET Framework 4.7.1、C# 9.0

アセンブリ定義:
- `Void2610.TypingLib`: ライブラリ本体
- `Void2610.TypingGame`: テスト実装
