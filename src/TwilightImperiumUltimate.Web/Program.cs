using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using TwilightImperiumUltimate.Web;
using TwilightImperiumUltimate.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");
builder.Logging.SetMinimumLevel(LogLevel.Debug);

builder.Services.RegisterServices(builder);

await builder.Build().RunAsync().ConfigureAwait(true);