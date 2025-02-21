using Microsoft.AspNetCore.StaticFiles;
using Showcase_Profielpagina.Controllers;

var builder = WebApplication.CreateBuilder(args);
HttpResponseMessage response = new HttpResponseMessage();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ContactController>();
var app = builder.Build();
app.Use(async (context, next) => {
    await next();
});
app.UseStaticFiles(new StaticFileOptions {
    OnPrepareResponse = ctx => {
        var fileName = ctx.File.Name;
        if (fileName.EndsWith("Thumbs.db", StringComparison.OrdinalIgnoreCase) ||
                fileName.EndsWith(".DS_Store", StringComparison.OrdinalIgnoreCase) ||
                fileName.StartsWith(".git", StringComparison.OrdinalIgnoreCase) ||
                fileName.StartsWith(".svn", StringComparison.OrdinalIgnoreCase)) {
            ctx.Context.Response.StatusCode = 403;
            ctx.Context.Response.Body = Stream.Null;
        }
    },
    ContentTypeProvider = new FileExtensionContentTypeProvider {
        Mappings =
        {
            [".js"] = "application/javascript",
            [".css"] = "text/css"
        }
    }
});
app.UseCookiePolicy(new CookiePolicyOptions {
    Secure = CookieSecurePolicy.Always,
    MinimumSameSitePolicy = SameSiteMode.Strict
});
app.UseCookiePolicy(new CookiePolicyOptions {
    Secure = CookieSecurePolicy.Always,  // Cookies worden alleen verzonden over HTTPS
    MinimumSameSitePolicy = SameSiteMode.Strict // Beschermt tegen CSRF-aanvallen
});

app.Use(async (context, next) => {
    context.Response.OnStarting(() => {
        context.Response.Headers.Add("Content-Security-Policy",
        $"default-src 'self'; " +
        "script-src 'self' https://www.google.com/ https://www.gstatic.com/; " + 
        "style-src 'self' https://trusted-styles.com 'unsafe-inline'; " + // Je kunt hier 'unsafe-inline' gebruiken, of je kunt een nonce gebruiken voor inline styles
        "connect-src 'self' ws://localhost:* http://localhost:* wss://www.google.com https://www.google.com/; " + 
        "img-src 'self' https://trusted-images.com data:; " +
        "frame-src 'self' https://www.google.com/ https://www.recaptcha.net/; " +
        "font-src 'self';");

        context.Response.Headers.Remove("Server"); // Verwijder 'Server'-header
        context.Response.Headers.Add("X-XSS-Protection", "1; mode=block"); // Extra bescherming tegen XSS
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // Vermijd MIME-sniffing

        return Task.CompletedTask;
    });

    await next();
});
app.UseStaticFiles();
app.UseRouting();
app.UseHttpsRedirection();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
