using GiveTurn.Blazor;
using GiveTurn.Blazor.Services;
using GiveTurn.Blazor.Services.Interfaces;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ITurnServices, TurnServices>();
builder.Services.AddScoped<IUserServices, UserServices>();

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7251/") });

await builder.Build().RunAsync();
