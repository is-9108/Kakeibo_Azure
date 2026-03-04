using Kakeibo.Application.Interfaces;
using Kakeibo.Application.Services;
using Kakeibo.Domain.Entities;
using Kakeibo.Infrastructure.Persistence;
using Kakeibo.Infrastructure.Repositories;
using Microsoft.Azure.Functions.Worker.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = FunctionsApplication.CreateBuilder(args);

builder.ConfigureFunctionsWebApplication();

// === SQL Server ===
var connectionString = builder.Configuration["SqlConnectionString"]
    ?? throw new InvalidOperationException("設定に 'SqlConnectionString' を指定してください。（local.settings.json の Values に追加）");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(connectionString));

// === クリーンアーキテクチャ: DI 登録 ===
builder.Services.AddScoped<ICategory, CategoryRepository>();
builder.Services.AddScoped<ITransaction, TransactionRepository>();
builder.Services.AddScoped<ISubscription,SubscriptionRepository>();

// Application Insights isn't enabled by default. See https://aka.ms/AAt8mw4.
// builder.Services
//     .AddApplicationInsightsTelemetryWorkerService()
//     .ConfigureFunctionsApplicationInsights();

builder.Build().Run();
