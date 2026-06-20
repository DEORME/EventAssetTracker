using EventAssetTracker.Models;

namespace EventAssetTracker.Services
{
    public interface IDatabaseService
    {
        Task<List<Activo>> ObtenerActivosAsync();
        Task<Activo?> ObtenerActivoPorIdAsync(int id);
        Task<int> GuardarActivoAsync(Activo activo); 
        Task<int> EliminarActivoAsync(Activo activo);

        Task<List<HistorialEstado>> ObtenerHistorialPorActivoAsync(int activoId);
        Task<int> GuardarHistorialAsync(HistorialEstado historial);

        Task<int> ContarActivosAsync();
        Task<int> ContarPorEstadoAsync(EstadoActivo estado);
    }
}