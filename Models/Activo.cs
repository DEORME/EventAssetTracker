using SQLite;

namespace EventAssetTracker.Models
{
    public class Activo
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string CodigoInventario { get; set; } = string.Empty;

        public string Ubicacion { get; set; } = string.Empty;

        public string Responsable { get; set; } = string.Empty;

        public EstadoActivo EstadoActual { get; set; } = EstadoActivo.EnUso;

        public DateTime? FechaUltimoCambio { get; set; }
    }
}