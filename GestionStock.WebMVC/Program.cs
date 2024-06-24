using Microsoft.Extensions.Configuration;
using Stock.Core.Business;
using Stock.Core.Configuration;
using Stock.Core.DataEF;

namespace GestionStock.WebMVC
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            var connectionString = builder.Configuration.GetConnectionString("StockConnectionString");

            // 2:26 Segunda Parte Clase 9
            var config = new Config()
            {
                ConnectionString = connectionString
            };

            builder.Services.AddScoped<Config>(p => {
                return config;
            });

            // Agregado para la configuración de las sesiones de los Usuarios
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });

            // Inyecto el Repositorio de Datos de Productos
            builder.Services.AddScoped<StockRepositoryProducto>();
            // Inyecto a Business de Productos
            builder.Services.AddScoped<StockBusinessProducto>();

            // Inyecto los servicios (business) y repositorios restantes
            builder.Services.AddScoped<StockRepositoryCompra>();
            builder.Services.AddScoped<StockBusinessCompra>();
            builder.Services.AddScoped<StockRepositoryVenta>();
            builder.Services.AddScoped<StockBusinessVenta>();
            builder.Services.AddScoped<StockRepositoryUsuario>();
            builder.Services.AddScoped<StockBusinessUsuario>();

            // Add services to the container.
            builder.Services.AddControllersWithViews();

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

            app.UseAuthentication();
            app.UseAuthorization();

            // Agregado para usar sesiones de Usuario
            app.UseSession();

            // Mapea la Ruta por defecto.
            // https://localhost:7293/Home
            // https://localhost:7293/Home/Index
            //app.MapControllerRoute(
            //    name: "default",
            //    pattern: "{controller=Home}/{action=Index}/{id?}");

            // Modifico para que la ruta por defecto sea la pantalla de Login
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Usuario}/{action=Login}/{id?}");

            app.Run();
        }
    }
}
