using EndPoint.Api.Configurations.Middlewares;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Extensions.Primitives;
using Swashbuckle.AspNetCore.SwaggerUI;

namespace EndPoint.Api.Configurations;

public static class MiddlewaresConfiguration
{
    public static void Config(this WebApplication app)
    {
        #region middlewares

        var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";
        app.UseCors(MyAllowSpecificOrigins);


        var provider = new FileExtensionContentTypeProvider();
        provider.Mappings[".epub"] = "application/epub+zip";

        app.UseStaticFiles(options: new StaticFileOptions
        {
            ContentTypeProvider = provider
        });





        //app.UseRequestLocalization(LocalizationConfig.Config());
        // if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.InjectStylesheet("/swagger-ui/SwaggerDark.css");
                c.DocExpansion(DocExpansion.None);
            });
        }
        app.UseChallengeMiddleware();

       // app.UseIpRateLimiting();


        app.UseAuthentication();
        app.UseAuthorization();


        app.MapControllers();

        //app.MapHub<StatisticsHub>("/statisticsHub");

        app.Use(async (context, next) =>
        {
            context.Response.Headers.Add("referrer-policy", new StringValues("no-referrer-when-downgrade"));
            await next();
        });

        // Policy.Handle<HttpRequestException>().Retry(3);

        #endregion middlewares
    }
}
