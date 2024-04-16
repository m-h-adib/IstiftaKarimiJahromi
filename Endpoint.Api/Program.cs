using EndPoint.Api.Configurations;

var builder = WebApplication.CreateBuilder(args);
builder.Services.Config(builder.Configuration);
builder.Services.UserConfig(builder.Configuration);
var app = builder.Build();
app.Config();
app.Run();