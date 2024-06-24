
using Stock.Core.Business;
using Stock.Core.Configuration;
using Stock.Core.DataEF;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Stock.Core.Entidades;

namespace GestionStock.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Obtener el String de conexión para la BD
            var connectionString = builder.Configuration.GetConnectionString("StockConnectionString");

            var config = new Config()
            {
                ConnectionString = connectionString
            };
            // Obtener el String de conexión para la BD

            builder.Services.AddScoped<Config>(p => {
                return config;
            });

            // Inyecto el Repositorio y Business de Venta (donde obtengo el Stock)
            builder.Services.AddScoped<StockRepositoryVenta>();
            builder.Services.AddScoped<StockBusinessVenta>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
