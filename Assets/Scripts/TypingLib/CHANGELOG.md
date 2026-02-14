# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [1.0.0] - 2026-02-15

### Added

#### Core Features
- タイピングセッション管理（`ITypingSession`、`TypingSession`）
- 英語入力バリデーション（`EnglishInputValidator`）
- 日本語（ローマ字）入力バリデーション（`JapaneseInputValidator`）
- ローマ字→ひらがな変換テーブル（`RomajiTable`）
- リアクティブな状態管理（R3統合）
- VContainer拡張メソッド（`VContainerExtensions`）

#### Models
- `TypingQuestion` - タイピング問題の定義
- `TypingSessionSettings` - セッション設定
- `InputResult` - 入力結果
- `SessionState` - セッション状態（Idle, Running, Paused, Completed）

#### Japanese Input Support
- 拗音対応（きゃ、しゃ、ちゃ等）
- 促音対応（っ）
- 「ん」の処理（nn, n+子音）
- 複数文字消費（ConsumedCount）
- 入力バッファリング（PendingInput）

#### Settings
- 大文字小文字の区別設定
- 空白スキップ機能
- 記号スキップ機能
- プリセット設定（Default, SkipWhitespaceOnly, SkipAll）

#### Samples
- Basic English Typing - 英語タイピングの基本サンプル
  - Unity UI (uGUI) 使用
  - UniTask による非同期処理
  - VContainer による DI 設定
  - 詳細なセットアップガイド付き

#### Tests
- 172件のユニットテスト（95%以上のカバレッジ）
- Models、Services、Validatorの完全なテスト
- 英語・日本語入力の包括的なテスト

#### Documentation
- README.md - インストール方法、クイックスタート、API説明
- サンプルREADME - セットアップ手順、カスタマイズ方法
- XMLドキュメントコメント - 全クラス・メソッドに日本語コメント

### Dependencies
- jp.hadashikick.vcontainer: 1.15.4
- com.cysharp.r3: 1.2.0

### Technical Details
- Unity 6000.0以上対応
- .NET Framework 4.7.1、C# 9.0
- MVPパターンによるアーキテクチャ
- UI非依存のコアライブラリ設計

[1.0.0]: https://github.com/void2610/typing-lib/releases/tag/v1.0.0
