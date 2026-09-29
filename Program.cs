using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using PaymentGateway.Config;
using PaymentGateway.Data;
using PaymentGateway.Endpoints.Customers;
using PaymentGateway.Interfaces;
using PaymentGateway.Repositories;
using RestSharp;
using RestSharp.Serializers.Json;

var builder = WebApplication.CreateBuilder(args);

IConfigurationSection appConfigSection = builder.Configuration.GetSection(AppConfig.SectionName);
builder.Services.Configure<AppConfig>(appConfigSection);

var appConfig = appConfigSection.Get<AppConfig>();

if (appConfig is null) throw new Exception("AppConfig not provided.");

builder.WebHost.UseUrls($"http://*:{appConfig.Port}");

builder.Services.AddOpenApi();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(appConfig.ConnectionString));

builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
    options.SerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddMassTransit(bus =>
{
    var currentAssembly = Assembly.GetExecutingAssembly();

    bus.SetKebabCaseEndpointNameFormatter();

    bus.AddConsumers(currentAssembly);

    bus.UsingRabbitMq((context, configurator) =>
    {
        configurator.Host(new Uri(appConfig.MessageHost), x =>
      {
          x.Username(appConfig.MessageUser);
          x.Password(appConfig.MessagePassword);
      });

        configurator.ConfigureEndpoints(context);
    });
});

builder.Services.AddSingleton<IRestClient>(serviceProvider =>
{
    RestClientOptions restClientOptions = new RestClientOptions
    {
        BaseUrl = new Uri(appConfig.PaymentUrl),
    };

    RestClient httpClient = new RestClient(
      restClientOptions,
      configureSerialization: s => s.UseSystemTextJson(new JsonSerializerOptions
      {
          PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
          AllowTrailingCommas = true,
      })
      );

    var valueBytes = Encoding.UTF8.GetBytes(appConfig.PaymentGatewayKey + ':');

    string encodedApiKey = Convert.ToBase64String(valueBytes);

    httpClient.AddDefaultHeader("Authorization", $"Basic {encodedApiKey}");

    return httpClient;
});


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapCustomerEndpoints();

app.MapGet("health", () => new
{
    service = appConfig.Service,
    status = appConfig.Status,
    port = appConfig.Port,
    time = appConfig.Time
});

await app.RunAsync();
