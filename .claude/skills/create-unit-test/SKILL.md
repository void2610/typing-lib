# Unity Test Framework ユニットテスト作成スキル

指定されたクラスまたはメソッドに対して、Unity Test Framework (NUnit) を使用したユニットテストを作成します。

## 入力

$ARGUMENTS

## テスト作成ガイドライン

### 1. ファイル構成

テストファイルは以下の規則に従って配置してください：

```
Assets/Tests/
├── Runtime/           # PlayMode テスト（ゲーム実行時のテスト）
│   ├── [AssemblyName].Tests.asmdef
│   └── [対象クラス名]Tests.cs
└── Editor/            # EditMode テスト（エディタ拡張のテスト）
    ├── [AssemblyName].Editor.Tests.asmdef
    └── [対象クラス名]Tests.cs
```

### 2. asmdef ファイル（必要な場合のみ作成）

```json
{
    "name": "[AssemblyName].Tests",
    "rootNamespace": "[Namespace].Tests",
    "references": [
        "[テスト対象のアセンブリ]",
        "UnityEngine.TestRunner",
        "UnityEditor.TestRunner"
    ],
    "includePlatforms": [],
    "excludePlatforms": [],
    "allowUnsafeCode": false,
    "overrideReferences": true,
    "precompiledReferences": [
        "nunit.framework.dll"
    ],
    "autoReferenced": false,
    "defineConstraints": [
        "UNITY_INCLUDE_TESTS"
    ],
    "versionDefines": [],
    "noEngineReferences": false
}
```

### 3. テストクラスの構造

```csharp
using NUnit.Framework;

namespace [Namespace].Tests
{
    /// <summary>
    /// [テスト対象クラス名] のユニットテスト
    /// </summary>
    [TestFixture]
    public class [テスト対象クラス名]Tests
    {
        private [テスト対象クラス] _sut; // System Under Test

        [SetUp]
        public void SetUp()
        {
            // 各テスト前の初期化処理
            _sut = new [テスト対象クラス]();
        }

        [TearDown]
        public void TearDown()
        {
            // 各テスト後のクリーンアップ処理（必要な場合）
        }

        // テストメソッド...
    }
}
```

### 4. テストメソッドの命名規則

**日本語を活用した明確な命名**を使用してください：

```csharp
[Test]
public void メソッド名_条件や状況_期待される結果()
{
    // ...
}
```

**命名例：**
- `Validate_正しい入力_正解を返す()`
- `Calculate_ゼロで除算_例外をスローする()`
- `GetItem_存在しないID_nullを返す()`
- `Constructor_パラメータなし_デフォルト値が設定される()`

### 5. AAA パターン（Arrange-Act-Assert）

すべてのテストは AAA パターンに従って構造化してください：

```csharp
[Test]
public void Add_二つの正の数_合計を返す()
{
    // Arrange（準備）
    var calculator = new Calculator();
    int a = 5;
    int b = 10;

    // Act（実行）
    int result = calculator.Add(a, b);

    // Assert（検証）
    Assert.That(result, Is.EqualTo(15));
}
```

### 6. Assert の書き方

**Constraint Model（推奨）を使用：**

```csharp
// 等価性
Assert.That(result, Is.EqualTo(expected));
Assert.That(result, Is.Not.EqualTo(unexpected));

// null チェック
Assert.That(result, Is.Null);
Assert.That(result, Is.Not.Null);

// 真偽値
Assert.That(condition, Is.True);
Assert.That(condition, Is.False);

// コレクション
Assert.That(list, Is.Empty);
Assert.That(list, Is.Not.Empty);
Assert.That(list, Has.Count.EqualTo(3));
Assert.That(list, Contains.Item(item));

// 文字列
Assert.That(str, Does.StartWith("prefix"));
Assert.That(str, Does.Contain("substring"));
Assert.That(str, Is.Empty);

// 範囲
Assert.That(value, Is.GreaterThan(0));
Assert.That(value, Is.InRange(1, 10));

// 例外
Assert.Throws<ArgumentException>(() => method());
Assert.DoesNotThrow(() => method());

// 型
Assert.That(obj, Is.InstanceOf<ExpectedType>());
```

### 7. パラメータ化テスト

同じロジックを複数の入力でテストする場合：

```csharp
[Test]
[TestCase("a", "あ")]
[TestCase("ka", "か")]
[TestCase("shi", "し")]
public void TryConvert_有効なローマ字_ひらがなに変換される(string romaji, string expected)
{
    // Act
    var result = RomajiTable.TryConvert(romaji, out var hiragana);

    // Assert
    Assert.That(result, Is.True);
    Assert.That(hiragana, Is.EqualTo(expected));
}
```

### 8. テストカテゴリ

関連するテストをグループ化：

```csharp
#region 基本的な変換テスト

[Test]
public void Convert_母音_正しく変換される() { /* ... */ }

[Test]
public void Convert_子音_正しく変換される() { /* ... */ }

#endregion

#region エッジケース

[Test]
public void Convert_空文字列_falseを返す() { /* ... */ }

[Test]
public void Convert_null_falseを返す() { /* ... */ }

#endregion
```

### 9. テストすべき項目

1. **正常系（Happy Path）**: 期待通りの入力での動作
2. **境界値**: 最小値、最大値、境界付近の値
3. **エッジケース**: 空文字列、null、空のコレクション
4. **異常系**: 無効な入力、例外がスローされるべきケース
5. **状態遷移**: オブジェクトの状態変化

### 10. PlayMode テスト（UnityTest）

複数フレームにまたがるテストや非同期処理のテスト：

```csharp
[UnityTest]
public IEnumerator MovePlayer_目標位置まで移動_正しい位置に到達する()
{
    // Arrange
    var player = new GameObject().AddComponent<PlayerController>();
    var targetPosition = new Vector3(10, 0, 0);

    // Act
    player.MoveTo(targetPosition);

    // 移動完了まで待機
    yield return new WaitForSeconds(1f);

    // Assert
    Assert.That(player.transform.position, Is.EqualTo(targetPosition).Using(Vector3EqualityComparer.Instance));
}
```

### 11. モック・スタブの使用

依存関係を分離してテストする場合：

```csharp
// インターフェースを使用した依存性注入
public interface ITimeProvider
{
    float DeltaTime { get; }
}

// テスト用のモック実装
public class MockTimeProvider : ITimeProvider
{
    public float DeltaTime { get; set; } = 0.016f;
}

[Test]
public void Update_一定時間経過_位置が更新される()
{
    // Arrange
    var mockTime = new MockTimeProvider { DeltaTime = 1f };
    var mover = new Mover(mockTime);

    // Act
    mover.Update();

    // Assert
    Assert.That(mover.Position, Is.EqualTo(expectedPosition));
}
```

## 実行手順

1. まず、テスト対象のコードを読み込んで理解する
2. テストすべき項目をリストアップする
3. asmdef ファイルが必要か確認し、必要なら作成する
4. テストクラスを作成する
5. AAA パターンに従ってテストメソッドを実装する
6. `#region` を使ってテストをカテゴリ分けする

## 参考

作成するテストは以下のベストプラクティスに基づいています：
- [Unity Test Framework 公式ドキュメント](https://docs.unity3d.com/Packages/com.unity.test-framework@1.4/manual/index.html)
- [Microsoft .NET Unit Testing Best Practices](https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices)
- [NUnit Documentation](https://docs.nunit.org/)
