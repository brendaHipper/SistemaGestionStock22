namespace Stock.Core.Configuration
{
    public class Config
    {
        // La clase config solo va tener la propiedad que va setear y obtener el 
        // ConecctionString. Esto debo pasarlo por configuración en el archivo json
        public string ConnectionString { get; set; }
    }
}
