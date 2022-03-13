using Blazored.Toast;
using Merkallow.Web;
using Merkallow.Web.Services;
using MetaMask.Blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

var config = new ClientConfig() { ApiUrl = builder.Configuration["ApiUrl"] };

builder.Services.AddSingleton<ClientConfig>(config);
builder.Services.AddScoped<IDoProjects, ProjectsClient>();
builder.Services.AddScoped<IDoAddresses, AddressClient>();
builder.Services.AddScoped<AppState>();

builder.Services.AddBlazoredToast();
builder.Services.AddMetaMaskBlazor();

await builder.Build().RunAsync();
