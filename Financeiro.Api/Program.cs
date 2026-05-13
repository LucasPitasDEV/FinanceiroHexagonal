using Financeiro.Application;
using Financeiro.Infrastructure;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .WriteTo.File("logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks()
    .AddMongoDb(builder.Configuration.GetConnectionString("MongoDb")!, name: "mongodb")
    .AddRabbitMQ(new Uri(builder.Configuration.GetConnectionString("RabbitMq")!), name: "rabbitmq");

var app = builder.Build();

app.UseMiddleware<Financeiro.Api.Middleware.ExceptionHandlingMiddleware>();

app.MapHealthChecks("/health");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapControllers();

try
{
    Log.Information("Iniciando a API Financeiro...");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A API falhou ao iniciar.");
}
finally
{
    Log.CloseAndFlush();
}