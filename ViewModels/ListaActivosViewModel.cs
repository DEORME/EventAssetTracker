using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAssetTracker.Models;
using EventAssetTracker.Services;

namespace EventAssetTracker.ViewModels
{
    public partial class ListaActivosViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        public ObservableCollection<Activo> Activos { get; } = new();

        [ObservableProperty]
        private Activo? activoSeleccionado;

        public ListaActivosViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Gestión de Activos";
        }

        [RelayCommand]
        private async Task CargarActivosAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var lista = await _databaseService.ObtenerActivosAsync();

                Activos.Clear();
                foreach (var activo in lista)
                    Activos.Add(activo);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudieron cargar los activos: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NuevoActivoAsync()
        {
            await Shell.Current.GoToAsync("ActivoFormPage");
        }

        [RelayCommand]
        private async Task EditarActivoAsync(Activo activo)
        {
            if (activo is null)
                return;

            await Shell.Current.GoToAsync($"ActivoFormPage?activoId={activo.Id}");
        }

        [RelayCommand]
        private async Task EliminarActivoAsync(Activo activo)
        {
            if (activo is null)
                return;

            bool confirmar = await Shell.Current.DisplayAlert(
                "Confirmar",
                $"¿Seguro que deseas eliminar '{activo.Nombre}'?",
                "Sí, eliminar",
                "Cancelar");

            if (!confirmar)
                return;

            try
            {
                IsBusy = true;
                await _databaseService.EliminarActivoAsync(activo);
                Activos.Remove(activo);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo eliminar: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task CambiarEstadoAsync(Activo activo)
        {
            if (activo is null)
                return;

            await Shell.Current.GoToAsync($"CambiarEstadoPage?activoId={activo.Id}");
        }

        [RelayCommand]
        private async Task VerHistorialAsync(Activo activo)
        {
            if (activo is null)
                return;

            await Shell.Current.GoToAsync($"HistorialPage?activoId={activo.Id}");
        }
    }
}