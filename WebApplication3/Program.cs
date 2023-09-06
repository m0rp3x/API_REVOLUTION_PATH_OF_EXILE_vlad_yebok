using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using WebApplication3.Controllers;
using WebApplication3.Data;

namespace WebApplication3
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // Добавьте сервисы, необходимые для вашего приложения, включая контекст базы данных.

            services.AddDbContext<ApplicationContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });

            // Создайте таймер для выполнения UpdateData каждые 2 минуты
            var timer = new Timer(UpdateDataCallback, null, 0, 120000); // 120000 миллисекунд (2 минуты)
        }

        public void UpdateDataCallback(object state)
        {
            // Получите сервис провайдера
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var serviceProvider = scope.ServiceProvider;

                try
                {
                    // Получите экземпляр контроллера
                    var controller = serviceProvider.GetRequiredService<HomeController>();

                    // Вызовите метод UpdateData
                    controller.UpdateData();
                }
                catch (Exception ex)
                {
                    // Обработка ошибок, если необходимо
                    Console.WriteLine($"Произошла ошибка: {ex.Message}");
                }
            }
        }
    }
}
