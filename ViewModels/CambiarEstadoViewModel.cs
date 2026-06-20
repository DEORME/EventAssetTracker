using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAssetTracker.Models;
using EventAssetTracker.Services;

namespace EventAssetTracker.ViewModels
{
    [QueryProperty(nameof(ActivoId), "activoId")]
    public partial class CambiarEstadoViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        private Activo? _activo;

        [ObservableProperty]
        private int activoId;

        [ObservableProperty]
        private string nombreActivo = string.Empty;

        [ObservableProperty]
        private EstadoActivo estadoActual;

        [ObservableProperty]
        private EstadoActivo estadoSeleccionado;

        public List<EstadoActivo> EstadosDisponibles { get; } =
            Enum.GetValues(typeof(EstadoActivo)).Cast<EstadoActivo>().ToList();

        public CambiarEstadoViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Cambiar Estado";
        }

        async partial void OnActivoIdChanged(int value)
        {
            if (value > 0)
            {
                await CargarActivoAsync(value);
            }
        }

        private async Task CargarActivoAsync(int id)
        {
            try
            {
                IsBusy = true;

                _activo = await _databaseService.ObtenerActivoPorIdAsync(id);
                if (_activo is null)
                {
                    await Shell.Current.DisplayAlert("Error", "No se encontró el activo.", "OK");
                    return;
                }

                NombreActivo = _activo.Nombre;
                EstadoActual = _activo.EstadoActual;
                EstadoSeleccionado = _activo.EstadoActual;
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el activo: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task ConfirmarCambioAsync()
        {
            if (IsBusy || _activo is null)
                return;

            if (EstadoSeleccionado == EstadoActual)
            {
                await Shell.Current.DisplayAlert("Aviso", "Selecciona un estado diferente al actual.", "OK");
                return;
            }

            try
            {
                IsBusy = true;

                var estadoAnterior = _activo.EstadoActual;
                var fechaCambio = DateTime.Now;

                _activo.EstadoActual = EstadoSeleccionado;
                _activo.FechaUltimoCambio = fechaCambio;

                await _databaseService.GuardarActivoAsync(_activo);

                var registroHistorial = new HistorialEstado
                {
                    ActivoId = _activo.Id,
                    EstadoAnterior = estadoAnterior,
                    EstadoNuevo = EstadoSeleccionado,
                    FechaCambio = fechaCambio
                };

                await _databaseService.GuardarHistorialAsync(registroHistorial);

                await Shell.Current.DisplayAlert("Éxito", "El estado se actualizó correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo cambiar el estado: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CancelarAsync()
        {
            await Shell.Current.GoToAsync("..");
        }
    }
}