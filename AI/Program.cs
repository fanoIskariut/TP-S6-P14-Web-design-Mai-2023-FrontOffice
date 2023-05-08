using AI.Context;
using Microsoft.AspNetCore.Rewrite;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ApplicationDbContext>();

String connString = builder.Configuration.GetConnectionString("postgresql");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connString));
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles(new StaticFileOptions
{
    OnPrepareResponse = ctx =>
    {
        // Add cache control header for static files
        ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=3600");
    }
});


app.UseStaticFiles();
// Configure URL rewriting
var options = new RewriteOptions()
    .AddIISUrlRewrite(app.Environment.ContentRootFileProvider, "URewrite.xml");
app.UseRewriter(options);
app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();