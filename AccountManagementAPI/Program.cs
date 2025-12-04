using AccountManagementAPI.Database;
using AccountManagementAPI.Repositories;
using AccountManagementAPI.Services;
using AccountManagementAPI.Utils;
using NLog;
using NLog.Web;

var logger = LogManager.Setup()
    .LoadConfigurationFromAppSettings()
    .GetCurrentClassLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // ===== Add NLog =====
    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    // ===== Controllers =====
    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    // ===== Register Database & Utils =====
    builder.Services.AddScoped<IOracleDb, OracleDb>();
    builder.Services.AddSingleton<ConfigurationHelper>();
    builder.Services.AddSingleton<EncryptHelper>();


    // ===== Register Repositories (KHÔNG có interface) =====
    builder.Services.AddScoped<AccountRepository>();
    builder.Services.AddScoped<SubAccountRepository>();
    builder.Services.AddScoped<LogEntryRepository>();

    // ===== Register Services =====
    builder.Services.AddScoped<IAccountService, AccountService>();
    builder.Services.AddScoped<ISubAccountService, SubAccountService>();

    // LoggerService: ghi log vào database → SCOPED
    builder.Services.AddScoped<ILoggerService, LoggerService>();

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
    }

    app.UseHttpsRedirection();
    app.UseAuthorization();
    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}
