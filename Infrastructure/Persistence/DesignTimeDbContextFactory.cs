using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Kakeibo.Infrastructure.Persistence;

/// <summary>
/// dotnet ef マイグレーション用。実行時の DI は使わない。
/// </summary>
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    private const string DefaultConnectionString =
        "Server=(localdb)\\mssqllocaldb;Database=Kakeibo;Trusted_Connection=True;TrustServerCertificate=True;";

    public AppDbContext CreateDbContext(string[] args)
    {
        var connectionString = GetConnectionString();

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlServer(connectionString)
            .Options;

        return new AppDbContext(options);
    }

    private static string GetConnectionString()
    {
        var dir = Directory.GetCurrentDirectory();
        for (var i = 0; i < 3; i++)
        {
            var path = Path.Combine(dir, "local.settings.json");
            if (File.Exists(path))
            {
                try
                {
                    using var doc = JsonDocument.Parse(File.ReadAllText(path));
                    if (doc.RootElement.TryGetProperty("Values", out var values) &&
                        values.TryGetProperty("SqlConnectionString", out var cs))
                    {
                        var s = cs.GetString();
                        if (!string.IsNullOrWhiteSpace(s))
                            return s;
                    }
                }
                catch
                {
                    // 読み取り失敗時はフォールバックへ
                }
                break;
            }
            dir = Path.GetDirectoryName(dir) ?? dir;
        }

        return DefaultConnectionString;
    }
}
