using DataLayer;

using Microsoft.OpenApi.Models;

using System.Reflection;

using TestTaskWebApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "TestTaskWebApi", Version = "v1" });

    // Set the comments path for the Swagger JSON and UI.
    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);
});

builder.Services.AddSingleton<ITestTaskDbService>(dbService =>
{
    IConfigurationSection configurationSection = builder.Configuration.GetSection("TestTaskCosmosDb");

    var databaseName = configurationSection["DatabaseName"];
    var containerName = configurationSection["ContainerName"];
    var account = configurationSection["Account"];
    var key = configurationSection["Key"];
    var client = new Microsoft.Azure.Cosmos.CosmosClient(account, key);
    var cosmosDbService = new TestTaskDbService(client, databaseName, containerName);

    return cosmosDbService;
});

builder.Services.AddTransient<ITestTaskService, TestTaskService>();

// The cors allow specific origins name
var corsAllowSpecificOriginsName = "_corsAllowSpecificOrigins";
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: corsAllowSpecificOriginsName,
                      builder =>
                      {
                          builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                      });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors(corsAllowSpecificOriginsName);

app.UseAuthorization();

app.MapControllers();

app.Run();