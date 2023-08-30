using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Serilog;
using System.Globalization;
using TwilightImperiumUltimate.Web;
using TwilightImperiumUltimate.Web.Helpers.Culture;
using TwilightImperiumUltimate.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Verbose()
    .WriteTo.BrowserConsole(formatProvider: CultureInfo.InvariantCulture)
    .CreateLogger();
builder.Services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));

builder.Services.RegisterServices(builder);
var host = builder.Build();

await CultureSetup.SetApplicationCultureAsync(host);

await host.RunAsync();
