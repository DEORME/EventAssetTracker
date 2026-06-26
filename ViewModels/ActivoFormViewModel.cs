using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAssetTracker.Models;
using EventAssetTracker.Services;

namespace EventAssetTracker.ViewModels
{
    [QueryProperty(nameof(ActivoId), "activoId")]
    public partial class ActivoFormViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        private Activo? _activoOriginal;

        [ObservableProperty]
        private int activoId;

        [ObservableProperty]
        private string nombre = string.Empty;

        [ObservableProperty]
        private string codigoInventario = string.Empty;

        [ObservableProperty]
        private string ubicacion = string.Empty;

        [ObservableProperty]
        private string responsable = string.Empty;

        [ObservableProperty]
        private bool esEdicion;

        public ActivoFormViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        // Se dispara automáticamente cuando Shell asigna ActivoId (gracias a [QueryProperty])
        async partial void OnActivoIdChanged(int value)
        {
            if (value > 0)
            {
                await CargarActivoAsync(value);
            }
            else
            {
                // Modo creación
                EsEdicion = false;
                Title = "Nuevo Activo";
                LimpiarFormulario();
            }
        }

        private async Task CargarActivoAsync(int id)
        {
            try
            {
                IsBusy = true;

                var activo = await _databaseService.ObtenerActivoPorIdAsync(id);
                if (activo is null)
                {
                    await Shell.Current.DisplayAlert("Error", "No se encontró el activo.", "OK");
                    return;
                }

                _activoOriginal = activo;
                EsEdicion = true;
                Title = "Editar Activo";

                Nombre = activo.Nombre;
                CodigoInventario = activo.CodigoInventario;
                Ubicacion = activo.Ubicacion;
                Responsable = activo.Responsable;
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

        private void LimpiarFormulario()
        {
            _activoOriginal = null;
            Nombre = string.Empty;
            CodigoInventario = string.Empty;
            Ubicacion = string.Empty;
            Responsable = string.Empty;
        }

        private bool ValidarCampos(out string mensajeError)
        {
            if (string.IsNullOrWhiteSpace(Nombre))
            {
                mensajeError = "El nombre del activo es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(CodigoInventario))
            {
                mensajeError = "El código de inventario es obligatorio.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Ubicacion))
            {
                mensajeError = "La ubicación es obligatoria.";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Responsable))
            {
                mensajeError = "El responsable es obligatorio.";
                return false;
            }

            mensajeError = string.Empty;
            return true;
        }

        [RelayCommand]
        private async Task GuardarAsync()
        {
            if (IsBusy)
                return;

            if (!ValidarCampos(out string mensajeError))    
            {
            await Shell.Current.DisplayAlert("Datos incompletos", mensajeError, "OK");
            return;
            }

            var activos = await _databaseService.ObtenerActivosAsync();
            bool codigoDuplicado = activos.Any(a =>
            a.CodigoInventario.Trim().ToLower() == CodigoInventario.Trim().ToLower() &&
            a.Id != (_activoOriginal?.Id ?? 0));

            if (codigoDuplicado)
            {       
            await Shell.Current.DisplayAlert(
            "Código duplicado",
            $"Ya existe un activo con el código '{CodigoInventario}'. Por favor usa un código diferente.",
            "OK");
            return;
            }

            try
            {
                IsBusy = true;

                Activo activo;

                if (EsEdicion && _activoOriginal is not null)
                {
                    activo = _activoOriginal;
                    activo.Nombre = Nombre;
                    activo.CodigoInventario = CodigoInventario;
                    activo.Ubicacion = Ubicacion;
                    activo.Responsable = Responsable;
                }
                else
                {
                    activo = new Activo
                    {
                        Nombre = Nombre,
                        CodigoInventario = CodigoInventario,
                        Ubicacion = Ubicacion,
                        Responsable = Responsable,
                        EstadoActual = EstadoActivo.EnUso,
                        FechaUltimoCambio = null
                    };
                }

                await _databaseService.GuardarActivoAsync(activo);

                await Shell.Current.DisplayAlert("Éxito", "El activo se guardó correctamente.", "OK");
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo guardar: {ex.Message}", "OK");
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