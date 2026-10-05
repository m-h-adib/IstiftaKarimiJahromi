using EndPoint.Api.Configurations;
using QuestPDF.Infrastructure;

QuestPDF.Settings.License = LicenseType.Community;

// QuestPDF 2026.9+ does not use Windows/system fonts by default.
// This application uses Tahoma/Arial for Persian PDF rendering.
QuestPDF.Settings.UseSystemFonts = true;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Config(builder.Configuration);
builder.Services.UserConfig(builder.Configuration);
var app = builder.Build();
app.Config();
app.Run();
