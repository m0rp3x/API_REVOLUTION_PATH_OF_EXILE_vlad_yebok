using Microsoft.EntityFrameworkCore;
using WebApplication3.Data;
using WebApplication3.Service;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<DatabaseUpdate>();

builder.Services.AddSingleton<PeriodicHostedService>();

builder.Services.AddHostedService(
    provider => provider.GetRequiredService<PeriodicHostedService>());

builder.Services.AddControllersWithViews();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(o => o.UseSqlServer(connectionString));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();