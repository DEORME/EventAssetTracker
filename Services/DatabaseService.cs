using SQLite;
using EventAssetTracker.Models;

namespace EventAssetTracker.Services
{
    public class DatabaseService : IDatabaseService
    {
        private SQLiteAsyncConnection? _connection;

        private static string DatabasePath =>
            Path.Combine(FileSystem.AppDataDirectory, "EventAssetTracker.db");

        private async Task InicializarAsync()
        {
            if (_connection is not null)
                return;

            _connection = new SQLiteAsyncConnection(DatabasePath, SQLiteOpenFlags.ReadWrite |
                                                                   SQLiteOpenFlags.Create |
                                                                   SQLiteOpenFlags.SharedCache);

            await _connection.CreateTableAsync<Activo>();
            await _connection.CreateTableAsync<HistorialEstado>();
        }

        public async Task<List<Activo>> ObtenerActivosAsync()
        {
            await InicializarAsync();
            return await _connection!.Table<Activo>().ToListAsync();
        }

        public async Task<Activo?> ObtenerActivoPorIdAsync(int id)
        {
            await InicializarAsync();
            return await _connection!.Table<Activo>()
                                     .Where(a => a.Id == id)
                                     .FirstOrDefaultAsync();
        }

        public async Task<int> GuardarActivoAsync(Activo activo)
        {
            await InicializarAsync();

            if (activo.Id == 0)
            {
                return await _connection!.InsertAsync(activo);
            }
            else
            {
                return await _connection!.UpdateAsync(activo);
            }
        }

        public async Task<int> EliminarActivoAsync(Activo activo)
        {
            await InicializarAsync();

            await _connection!.Table<HistorialEstado>()
                               .DeleteAsync(h => h.ActivoId == activo.Id);

            return await _connection!.DeleteAsync(activo);
        }

        public async Task<List<HistorialEstado>> ObtenerHistorialPorActivoAsync(int activoId)
        {
            await InicializarAsync();
            return await _connection!.Table<HistorialEstado>()
                                     .Where(h => h.ActivoId == activoId)
                                     .ToListAsync();
        }

        public async Task<int> GuardarHistorialAsync(HistorialEstado historial)
        {
            await InicializarAsync();
            return await _connection!.InsertAsync(historial);
        }

        public async Task<int> ContarActivosAsync()
        {
            await InicializarAsync();
            return await _connection!.Table<Activo>().CountAsync();
        }

        public async Task<int> ContarPorEstadoAsync(EstadoActivo estado)
        {
            await InicializarAsync();
            return await _connection!.Table<Activo>()
                                     .Where(a => a.EstadoActual == estado)
                                     .CountAsync();
        }
    }
}