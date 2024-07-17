using DevProdWebApp.Repository;
using DevProdWebApp.Utilities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DbConn");
builder.Services.AddDbContext<AppDbContext>(options => 
    options.UseSqlServer(connectionString));
builder.Services.AddScoped<IDeveloperRepo,DeveloperRepo>();
builder.Services.AddScoped<IProjectRepo,ProjectRepo>();
builder.Services.AddScoped<IMetricRepo,MetricRepo>();
builder.Services.AddScoped<ISettingsRepo,SettingsRepo>();


var app = builder.Build();
Initializer init = new Initializer();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=ToolDashboard}/{id?}");

app.Run();
