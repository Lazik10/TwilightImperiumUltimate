using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web;
using TwilightImperiumUltimate.Web.Helpers.Culture;
using TwilightImperiumUltimate.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.RegisterServices(builder);
var host = builder.Build();

await CultureSetup.SetApplicationCultureAsync(host);

await host.RunAsync();