using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using razor_pages_net6.Data;
using DevExpress.AspNetCore;
using DevExpress.AspNetCore.Reporting;
using DevExpress.XtraReports.Web.Extensions;
using razor_pages_net6.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

//devexpress - garantiza una serialización JSON adecuada
builder.Services.AddRazorPages().AddJsonOptions(options => options.JsonSerializerOptions.PropertyNamingPolicy = null);
builder.Services.AddDevExpressControls();
builder.Services.AddMvc();
builder.Services.AddScoped<ReportStorageWebExtension, CustomReportStorageWebExtension>();
builder.Services.ConfigureReportingServices(configurator =>
{
    configurator.ConfigureWebDocumentViewer(viewerConfigurator =>
    {
        viewerConfigurator.UseCachedReportSourceBuilder();
    });
});

var serverVersion = new MySqlServerVersion(new Version(8, 0, 29));
builder.Services.AddDbContext<DBContext>(
        dbContextOptions => dbContextOptions
                .UseMySql(builder.Configuration.GetConnectionString("cnDB"), serverVersion)
                // The following three options help with debugging, but should
                // be changed or removed for production.
                .LogTo(Console.WriteLine, LogLevel.Information)
                .EnableSensitiveDataLogging()
                .EnableDetailedErrors()
            );

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    var context = services.GetRequiredService<DBContext>();
    context.Database.EnsureCreated();
    DbInitializer.Initialize(context);
}

app.UseDevExpressControls();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

//devexpress - habilita los controladores en el enrutamiento
app.UseEndpoints(endpoints => {
    endpoints.MapControllers(); // enables controllers in endpoint routing
    endpoints.MapDefaultControllerRoute(); // adds the default route {controller=Home}/{action=Index}/{id?}
});

app.Run();
