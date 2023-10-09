using DocumentStorage.Configuration;
using DocumentStorage.Factories;
using DocumentStorage.Services;
using MessagePack.AspNetCoreMvcFormatter;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);
builder.Logging.ClearProviders();
builder.Logging.AddConsole();

builder.Services.AddControllersWithViews().AddNewtonsoftJson();

// Load configuration from appsettings.json
builder.Configuration.AddJsonFile("appsettings.json");

builder.Services.AddControllers().AddMvcOptions(options =>
{
    options.OutputFormatters.Add(new MessagePackOutputFormatter());
    options.InputFormatters.Add(new MessagePackInputFormatter());
});

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Add services to the container.
var storageSettings = new StorageSettings();
// Load configuration into a strongly-typed object
builder.Configuration.GetSection("StorageSettings").Bind(storageSettings);

builder.Services.Configure<StorageSettings>(
    builder.Configuration.GetSection("StorageSettings")
);

builder.Services.AddSingleton<IDocumentStorageFactory, DocumentStorageFactory>();
builder.Services.AddSingleton<ICloudDocumentStorage, CloudDocumentStorage>();
builder.Services.AddSingleton<IHddDocumentStorage, HddDocumentStorage>();

// Register other implementations as needed
// builder.Services.AddSingleton<IOtherDocumentStorage, OtherDocumentStorage>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Logger.LogInformation("Starting the app");

app.Run();
