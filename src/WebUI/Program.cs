using Common;
using Havit.Blazor.Components.Web;
using WebUI;
using Serilog;
using Serilog.Enrichers.Span;
using Serilog.Events;
using Prometheus;
using Common.Observe;
using OpenTelemetry.Trace;
using OpenTelemetry.Resources;
using Common.Stateful;

///////////////////////////////////////////////////////////
/// STATICS
///////////////////////////////////////////////////////////
Statics.MetricPrefix = Constants.WebUIName;
Statics.TracingService = Constants.WebUIName;

///////////////////////////////////////////////////////////
/// HOST
///////////////////////////////////////////////////////////
var builder = WebApplication.CreateBuilder(args);

var configProvider = builder.Configuration.AddJsonFile(Path.Join(Environment.CurrentDirectory, "configuration.local.json"), optional: true, reloadOnChange: true)
				.AddEnvironmentVariables(prefix: "P2G_")
				.AddCommandLine(args);

var apiSettings = new ApiSettings();
builder.Configuration.GetSection("Api").Bind(apiSettings);

var config = new AppConfiguration();
builder.Configuration.GetSection("Api").Bind(config.Api);
builder.Configuration.GetSection(nameof(Observability)).Bind(config.Observability);
builder.Configuration.GetSection(nameof(Developer)).Bind(config.Developer);

builder.Host.UseSerilog((ctx, logConfig) =>
{
	logConfig
	.ReadFrom.Configuration(ctx.Configuration, sectionName: $"{nameof(Observability)}:Serilog")
	.Enrich.WithSpan()
	.Enrich.FromLogContext();
});

///////////////////////////////////////////////////////////
/// SERVICES
///////////////////////////////////////////////////////////

builder.Services.AddScoped<IApiClient>(sp => new ApiClient(apiSettings.HostUrl));
builder.Services.AddHxServices();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();

FlurlConfiguration.Configure(config.Observability, 30);
Tracing.EnableTracing(builder.Services, config.Observability.Jaeger);

Log.Logger = new LoggerConfiguration()
				.ReadFrom.Configuration(builder.Configuration, sectionName: $"{nameof(Observability)}:Serilog")
				.Enrich.FromLogContext()
				.CreateLogger();

var runtimeVersion = Environment.Version.ToString();
var os = Environment.OSVersion.Platform.ToString();
var osVersion = Environment.OSVersion.VersionString;
var version = Constants.AppVersion;

Prometheus.Metrics.CreateGauge($"{Statics.MetricPrefix}_build_info", "Build info for the running instance.", new GaugeConfiguration()
{
	LabelNames = new[] { Common.Observe.Metrics.Label.Version, Common.Observe.Metrics.Label.Os, Common.Observe.Metrics.Label.OsVersion, Common.Observe.Metrics.Label.DotNetRuntime }
}).WithLabels(version, os, osVersion, runtimeVersion)
.Set(1);

Log.Debug("P2G WebUI Version: {@Version}", version);
Log.Debug("Operating System: {@Os}", osVersion);
Log.Debug("DotNet Runtime: {@DotnetRuntime}", runtimeVersion);

///////////////////////////////////////////////////////////
/// APP
///////////////////////////////////////////////////////////

var app = builder.Build();

if (Log.IsEnabled(LogEventLevel.Verbose))
	app.UseSerilogRequestLogging();

app.Use((context, next) =>
{
	return next.Invoke();
});

if (config.Observability.Prometheus.Enabled)
{
	Log.Information("Metrics Enabled");
	Common.Observe.Metrics.EnableCollector(config.Observability.Prometheus);

	app.MapMetrics();
	app.UseHttpMetrics();
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

await app.RunAsync();