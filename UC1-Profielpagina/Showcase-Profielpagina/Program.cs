using Showcase_Profielpagina.Controllers;

var builder = WebApplication.CreateBuilder(args);
HttpResponseMessage response = new HttpResponseMessage();
// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<ContactController>();
var app = builder.Build();
app.Use(async (context, next) => {
    context.Response.Headers["Content-Security-Policy"] = "default-src 'self'; script-src 'self' https://trusted-source.com;";
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
    }
});

app.Use(async (context, next) => {
    context.Response.OnStarting(() => {
        context.Response.Headers.Remove("Server"); // Verwijder 'Server'-header
        context.Response.Headers.Add("X-Content-Type-Options", "nosniff"); // Voeg beveiligingsheader toe
        return Task.CompletedTask;
    });

    await next();
});
app.UseStaticFiles();
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
