using CD5000Dashboard.Components;
using CD5000Dashboard.Data;
using SQLitePCL;

var builder = WebApplication.CreateBuilder(args);

// Initialize SQLCipher provider
Batteries_V2.Init();

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddSingleton<SqliteConnectionFactory>();
builder.Services.AddScoped<DashboardRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();