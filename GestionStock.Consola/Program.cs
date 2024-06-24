using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Stock.Core.Business;
using Stock.Core.Configuration;
using Stock.Core.DataEF;
using Stock.Core.Entidades;

namespace GestionStock.Consola
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Configura la lectura del archivo Json
            var configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            // Configura la inyección de dependencias
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection, configuration);
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Obtiene la instancia StockBusiness. 
            var stockBusinessProducto = serviceProvider.GetService<StockBusinessProducto>();

            // Menú para visualizar las opciones del CRUD
            bool continuar = true;

            while (continuar)
            {
                Console.WriteLine("Seleccione una opción:");
                Console.WriteLine("1. Crear un nuevo Producto");
                Console.WriteLine("2. Editar un Producto");
                Console.WriteLine("3. Eliminar un Producto");
                Console.WriteLine("4. Ver un Producto");
                Console.WriteLine("5. Listar todos los Productos");
                Console.WriteLine("6. Salir");

                var opcion = Console.ReadLine();

                switch (opcion)
                {
                    case "1":
                        CrearProducto(stockBusinessProducto);
                        break;
                    case "2":
                        EditarProducto(stockBusinessProducto);
                        break;
                    case "3":
                        EliminarProducto(stockBusinessProducto);
                        break;
                    case "4":
                        VerProductoPorNombre(stockBusinessProducto);
                        break;
                    case "5":
                        ListarProductos(stockBusinessProducto);
                        break;
                    case "6":
                        continuar = false;
                        break;
                    default:
                        Console.WriteLine("Opción no válida, intente nuevamente.");
                        break;
                }
            }

            Console.WriteLine("End!");


            // Métodos por consola para el CRUD
            static void CrearProducto(StockBusinessProducto stockBusiness)
            {
                Console.WriteLine("Crear un nuevo Producto");

                // Validación de nombre no vacío y no duplicado
                string nombre;
                do
                {
                    Console.Write("Nombre: ");
                    nombre = Console.ReadLine();

                    if (string.IsNullOrEmpty(nombre))
                    {
                        Console.WriteLine("El nombre del producto no puede estar vacío. Por favor, ingrese un nombre válido.");
                        continue;
                    }

                    var productoExistente = stockBusiness.GetProductoPorNombre(nombre);
                    if (productoExistente != null)
                    {
                        Console.WriteLine("Ya existe un producto con el mismo nombre. Por favor, ingrese un nombre diferente.");
                        nombre = null;
                    }

                } while (string.IsNullOrEmpty(nombre));

                Console.WriteLine("Seleccione una Categoria: ");
                var categorias = stockBusiness.ObtenerCategorias();
                for (int i = 0; i < categorias.Count; i++)
                {
                    Console.WriteLine($"{categorias[i].CategoriaId}: {categorias[i].Nombre}");
                }
                var categoriaId = int.Parse(Console.ReadLine());

                Console.Write("¿Habilitado? (true/false): ");
                var habilitado = bool.Parse(Console.ReadLine());

                var producto = new Producto
                {
                    Nombre = nombre,
                    CategoriaId = categoriaId,
                    Habilitado = habilitado
                };

                try
                {
                    stockBusiness.AgregarProducto(producto);
                    Console.WriteLine("Producto agregado con éxito!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al agregar el producto: {ex.Message}");
                }
            }



            static void EditarProducto(StockBusinessProducto stockBusiness)
            {
                Console.WriteLine("Editar un Producto");
                Console.Write("Ingrese el nombre del Producto a editar: ");
                var nombreProducto = Console.ReadLine();

                var producto = stockBusiness.GetProductoPorNombre(nombreProducto);
                if (producto != null)
                {
                    Console.WriteLine($"Producto actual: {producto.Nombre}");

                    Console.WriteLine("Nuevo nombre (dejar vacío para no cambiar):");
                    var nuevoNombre = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevoNombre))
                    {
                        producto.Nombre = nuevoNombre;
                    }

                    Console.WriteLine($"¿Habilitado? (actual: {producto.Habilitado}) (true/false, dejar vacío para no cambiar):");
                    var habilitadoStr = Console.ReadLine();
                    if (!string.IsNullOrEmpty(habilitadoStr))
                    {
                        producto.Habilitado = bool.Parse(habilitadoStr);
                    }

                    Console.WriteLine("Seleccione una nueva Categoria (dejar vacío para no cambiar): ");
                    var categorias = stockBusiness.ObtenerCategorias();
                    for (int i = 0; i < categorias.Count; i++)
                    {
                        Console.WriteLine($"{categorias[i].CategoriaId}: {categorias[i].Nombre}");
                    }
                    var nuevaCategoriaIdStr = Console.ReadLine();
                    if (!string.IsNullOrEmpty(nuevaCategoriaIdStr))
                    {
                        var nuevaCategoriaId = int.Parse(nuevaCategoriaIdStr);
                        producto.CategoriaId = nuevaCategoriaId;
                    }

                    var resultado = stockBusiness.EditarProducto(producto);
                    if (resultado.ProductoId > 0)
                    {
                        Console.WriteLine("Producto editado con éxito!");
                    }
                    else if (string.IsNullOrEmpty(resultado.Nombre))
                    {
                        Console.WriteLine($"Error al editar el producto: ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        Console.WriteLine("Producto no encontrado.");
                    }
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }




            static void EliminarProducto(StockBusinessProducto stockBusiness)
            {
                Console.WriteLine("Eliminar un Producto");
                Console.Write("Ingrese el nombre del Producto a eliminar: ");
                var nombreProducto = Console.ReadLine();

                var producto = stockBusiness.GetProductoPorNombre(nombreProducto);
                if (producto != null)
                {
                    var resultado = stockBusiness.EliminarProducto(producto.ProductoId);
                    if (resultado != null)
                    {
                        Console.WriteLine("Producto eliminado con éxito!");
                    }
                    else
                    {
                        Console.WriteLine($"Error al eliminar el producto: {resultado}");
                    }
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }

            static void VerProductoPorNombre(StockBusinessProducto stockBusiness)
            {
                Console.WriteLine("Ver un Producto");
                Console.Write("Ingrese el nombre del Producto a ver: ");
                var nombreProducto = Console.ReadLine();

                var producto = stockBusiness.GetProductoPorNombre(nombreProducto);
                if (producto != null)
                {
                    Console.WriteLine($"Producto: {producto.Nombre}, Categoría: {producto.Categoria.Nombre}, Habilitado: {producto.Habilitado}");
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }


            static void ListarProductos(StockBusinessProducto stockBusiness)
            {
                var productos = stockBusiness.ObtenerProductosComprasYVentas();
                foreach (var producto in productos)
                {
                    Console.WriteLine($"Producto: {producto.Nombre}, Categoría: {producto.Categoria.Nombre}, Habilitado: {producto.Habilitado}");
                }
            }

        }

        // Método para obtener y configurar la cadena de conexión
        private static void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            // Configurar la instancia de Config
            var config = new Config
            {
                // Obtiene la cadena de conexión
                ConnectionString = configuration.GetConnectionString("StockConnectionString")
            };
            services.AddSingleton(config);

            // Registra los Servicios - haciendo inyección de Dependencias
            services.AddScoped<StockRepositoryProducto>();
            services.AddScoped<StockBusinessProducto>();
        }
    }
}
