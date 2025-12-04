using AccountManagementAPI.Database;
using AccountManagementAPI.Repositories;
using AccountManagementAPI.Services;
using AccountManagementAPI.Utils;


var builder = WebApplication.CreateBuilder(args);

builder.Logging.ClearProviders();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
 
// đăng ký Database & Utils 
builder.Services.AddScoped<IOracleDb, OracleDb>();
builder.Services.AddSingleton<ConfigurationHelper>();
builder.Services.AddSingleton<EncryptHelper>();


// đăng ký Repositories 
builder.Services.AddScoped<AccountRepository>();
builder.Services.AddScoped<SubAccountRepository>();
builder.Services.AddScoped<LogEntryRepository>();

// đăng ký Services
builder.Services.AddScoped<IAccountService, AccountService>();
builder.Services.AddScoped<ISubAccountService, SubAccountService>();
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
