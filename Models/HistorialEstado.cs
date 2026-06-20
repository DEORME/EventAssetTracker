using SQLite;

namespace EventAssetTracker.Models
{
    public class HistorialEstado
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [Indexed]
        public int ActivoId { get; set; }

        public EstadoActivo EstadoAnterior { get; set; }

        public EstadoActivo EstadoNuevo { get; set; }

        public DateTime FechaCambio { get; set; }
    }
}