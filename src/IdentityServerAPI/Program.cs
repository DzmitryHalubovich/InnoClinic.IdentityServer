using DuendeIdentityServerFromScratch.TestData;
using DuendeInMemoryTemplate;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();

builder.Services.AddIdentityServer()
  .AddInMemoryIdentityResources(Config.IdentityResources)
  .AddInMemoryApiScopes(Config.ApiScopes)
  .AddInMemoryClients(Config.Clients)
  .AddTestUsers(TestUsers.Users)
  .AddDeveloperSigningCredential();

builder.Services.AddHttpClient();

var app = builder.Build();

app.UseStaticFiles();

app.UseRouting();

app.UseIdentityServer();

app.UseAuthorization();
app.MapRazorPages().RequireAuthorization();

app.Run();
