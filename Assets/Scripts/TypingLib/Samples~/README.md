# TypingLib Samples

TypingLibの使用方法を示すサンプル集です。

## サンプル一覧

### Basic English Typing
基本的な英語タイピングゲームのサンプルです。

**特徴:**
- 英語のみ
- Unity UI (uGUI) を使用
- 最小限の依存関係
- シンプルな実装

**学べること:**
- TypingLib の基本的な使い方
- VContainer での DI 設定
- MVP パターンの実装例

詳細は [BasicEnglishTyping/README.md](./BasicEnglishTyping/README.md) を参照してください。

## インポート方法

Unity Package Manager でこのパッケージをインポートすると、サンプルが利用可能になります。

1. Package Manager を開く
2. TypingLib パッケージを選択
3. 「Samples」タブを開く
4. インポートしたいサンプルの「Import」ボタンをクリック

サンプルは `Assets/Samples/Void2610 Typing Library/[version]/[sample-name]/` にインポートされます。

## カスタマイズのヒント

### 1. UI のカスタマイズ
- TextMeshPro に変更
- カスタムフォントの使用
- アニメーション効果の追加

### 2. 機能の追加
- タイマー機能
- スコアシステム
- ランキング機能
- 難易度選択

### 3. 日本語対応
- `RegisterTypingLibJapanese()` を使用
- IJapaneseInputValidator を DI
- ローマ字入力のバッファ表示

## サポート

問題や質問がある場合は、GitHubリポジトリの Issues をご利用ください。
