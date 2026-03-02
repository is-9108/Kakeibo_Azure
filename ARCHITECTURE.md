# クリーンアーキテクチャ構成

このプロジェクトは HTTP Trigger の Azure Functions をクリーンアーキテクチャで構成したベースコードです。

## レイヤー構成

```
Kakeibo/
├── Domain/           … ドメイン層（他に依存しない）
│   ├── Entities/     … エンティティ
│   └── Common/       … 共通型（Result など）
├── Application/      … アプリケーション層（Domain に依存）
│   ├── Interfaces/   … ユースケース・リポジトリのインターフェース
│   ├── DTOs/         … 入出力 DTO
│   └── Services/     … ユースケースの実装
├── Infrastructure/   … インフラ層（Application のインターフェースを実装）
│   ├── Persistence/ … DbContext・EF 設定・マイグレーション用ファクトリ
│   └── Repositories/ … リポジトリ実装（SQL Server / インメモリ）
├── Api/              … プレゼンテーション層（HTTP Trigger）
│   └── *Function.cs  … エンドポイント。Application のユースケースを呼び出す
└── Program.cs        … 起動・DI 登録
```

## 依存の向き

- **Domain** ← Application ← Infrastructure  
- **Domain** ← Application ← **Api**

Api と Infrastructure は Application のインターフェースにのみ依存し、互いに参照しません。

## サンプル API

| メソッド | ルート    | 説明           |
|----------|-----------|----------------|
| GET      | /api/sample | サンプル一覧取得 |
| POST     | /api/sample | サンプル登録     |

ローカル実行時: `http://localhost:7071/api/sample`  
（`local.settings.json` のポートに合わせてください。）

## 拡張の例

1. **新しいユースケース**  
   `Application/Interfaces/` にインターフェース、`Application/Services/` に実装を追加し、`Api` の Function から注入して呼び出す。

2. **永続化**  
   `IRepository<T>` の実装を `Infrastructure/Repositories/` に追加（例: Cosmos DB / SQL）。  
   `Program.cs` でインメモリの代わりにその実装を登録する。

---

## SQL Server 利用時

- **接続文字列**: `local.settings.json` の `Values` に `SqlConnectionString` を追加（既定で LocalDB の例を記載済み）。
- **DB 作成**: 初回またはスキーマ変更後に以下で DB を更新する。
  ```bash
  dotnet ef database update --context AppDbContext
  ```
- **インメモリに戻す場合**: `Program.cs` で `SqlServerSampleRepository` の登録をやめ、`InMemorySampleRepository` を `AddSingleton` で再度登録する。`AddDbContext` と `SqlConnectionString` の取得は削除してよい。

3. **ドメインの追加**  
   `Domain/Entities/` にエンティティ、`Domain/Common/` に値オブジェクトやドメインルールを追加する。
