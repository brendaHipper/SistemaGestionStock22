using Stock.Core.Entidades;
using Stock.Core.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Stock.Core.DataEF
{
    // Esta clase en la capa de datos que representa el Repositorio de Datos donde se
    // implementan los métodos de las Operaciones contra la Base de Datos.
    public class StockRepositoryProducto
    {
        // db es una instancia, es el objeto del Contexto (Clase StockContext).
        // Este contexto representa una sesión con la base de datos y se utiliza para consultar y guardar datos.

        // Aplica Inyeccion de Dependencia a traves del Constructor
        private readonly Config _config;
        public StockRepositoryProducto(Config config) 
        {
            _config = config;
        }

        public List<Producto> ObtenerTodosLosProductos()
        {
            var resultado = new List<Producto>();

            // CONSULTA CON LINQ
            using (var db = new StockContext(_config))
            {
                resultado = db.Productos.ToList();
            }

            return resultado;
        }


        public List<Producto> ObtenerProductosComprasYVentas()
        {
            using (var db = new StockContext(_config))
            {
                return db.Productos
                    .Include(p => p.Categoria)
                    .Include(p => p.Compras)
                    .Include(p => p.Ventas)
                    .ToList();
            }
        }

        public Producto ObtenerProductoPorNombre(string nombre)
        {
            using (var db = new StockContext(_config))
            {
                //return db.Productos.FirstOrDefault(p => p.Nombre == nombre);
                return db.Productos.Include(p => p.Categoria).FirstOrDefault(p => p.Nombre == nombre);
            }
        }

        // Agregar Producto (CRUD)
        public Producto AgregarProducto(Producto producto)
        {
            var resultado = new Producto();
            using (var db = new StockContext(_config))
            {
                if (db.Productos.Any(p => p.Nombre == producto.Nombre))
                {
                    Console.WriteLine("Ya existe un producto con el mismo nombre.");
                    return resultado;
                }
                else if(producto.Nombre == null)
                {
                    Console.WriteLine("Ingrese nombre del Producto");
                    return resultado;
                }
                else
                {
                    Console.WriteLine("Producto agregado con éxito!");
                }


                db.Productos.Add(producto);
                db.SaveChanges();
            }
            return resultado;
        }

        // Obtener categoria
        public List<Categoria> ObtenerCategorias()
        {
            using (var db = new StockContext(_config))
            {
                return db.Categorias.ToList();
            }
        }

        // Método para Editar un Producto (CRUD)
        public Producto EditarProducto(Producto producto)
        {
            var resultado = new Producto();
            using (var db = new StockContext(_config))
            {
                if (db.Productos.Any(p => p.ProductoId != producto.ProductoId && p.Nombre == producto.Nombre))
                {
                    Console.WriteLine("Ya existe un producto con el mismo nombre.");
                    return resultado;
                }

                var productoExistente = db.Productos.FirstOrDefault(p => p.ProductoId == producto.ProductoId);
                if (productoExistente != null)
                {
                    productoExistente.Nombre = producto.Nombre;
                    productoExistente.CategoriaId = producto.CategoriaId;
                    productoExistente.Habilitado = producto.Habilitado;

                    db.SaveChanges();
                    resultado = productoExistente;
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }
            return resultado;
        }


        // Elimina el Producto con sus compras y ventas asociadas (CRUD)
        public Producto EliminarProducto(int ProductoId)
        {
            var resultado = new Producto();

            using (var db = new StockContext(_config))
            {
                //var producto = db.Productos.FirstOrDefault(p => p.ProductoId == ProductoId);
                var producto = db.Productos
                                 .Include(p => p.Compras)
                                 .Include(p => p.Ventas)
                                 .Where(p => p.ProductoId == ProductoId).FirstOrDefault();

                if (producto != null)
                {
                    db.Compras.RemoveRange(producto.Compras);
                    db.Ventas.RemoveRange(producto.Ventas);
                    db.Productos.Remove(producto);
                    db.SaveChanges();
                }
                else
                {
                    Console.WriteLine("Producto no encontrado.");
                }
            }
            return resultado;
        }

    }
}
