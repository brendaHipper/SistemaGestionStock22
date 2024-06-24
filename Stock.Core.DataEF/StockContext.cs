using Microsoft.EntityFrameworkCore;
using Stock.Core.Configuration;
using Stock.Core.Entidades;

namespace Stock.Core.DataEF
{
    public class StockContext: DbContext
    {
        // Constructor de StockContext inicializa la configuración necesaria para tomar el string de conexión 
        private readonly Config _config;
        public StockContext(Config config)
        {
            _config = config;
        }

        // DbContext: Es un objeto que Mapea los objetos con las Tablas en la BD
        // DbSet representa la colección de todas las entidades del contexto
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Compra> Compras { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }

        // Método de configuración que llama a la cadena de conexión
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_config.ConnectionString);
        }

        public StockContext(DbContextOptions<StockContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Configura la relación uno a muchos entre Producto y Categoria
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.ListaProductos)
                .HasForeignKey(p => p.CategoriaId);

            // Configura la relación uno a muchos entre Producto y Compra
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Producto)
                .WithMany(p => p.Compras)
                .HasForeignKey(c => c.ProductoId);

            // AGREGO LAS RELACIONES CON USUARIO
            // Configura la relación uno a muchos entre Usuario y Compra
            modelBuilder.Entity<Compra>()
                .HasOne(c => c.Usuario)
                .WithMany(u => u.Compras)  
                .HasForeignKey(c => c.UsuarioId);

            // Configura la relación uno a muchos entre Usuario y Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Usuario)
                .WithMany(u => u.Ventas) 
                .HasForeignKey(v => v.UsuarioId);

            // Configura la relación uno a muchos entre Producto y Venta
            modelBuilder.Entity<Venta>()
                .HasOne(v => v.Producto)
                .WithMany(p => p.Ventas)
                .HasForeignKey(v => v.ProductoId);

            base.OnModelCreating(modelBuilder);
        }

    }
}
