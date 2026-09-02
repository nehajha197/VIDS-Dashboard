using ATMS.Components;
using ATMS.Components.Services;
using ATMS.DTOs;
using ATMS.Interfaces;
using ATMS.Services;
using Microsoft.Data.SqlClient;
using MudBlazor.Services;
using QuestPDF.Infrastructure;
using System.Data;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();
builder.Services.AddScoped<IVidsDataService, VidsDataService>();
builder.Services.AddMudServices();
builder.Services.AddScoped<AtmsDeviceService>();
builder.Services.AddScoped<SidebarStateService>();
builder.Services.AddScoped<IDbConnection>(sp =>
    new SqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();



// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);
app.UseHttpsRedirection();
QuestPDF.Settings.License = LicenseType.Community;

app.UseAntiforgery();

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.Run();
