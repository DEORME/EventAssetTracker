using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAssetTracker.Models;
using EventAssetTracker.Services;

namespace EventAssetTracker.ViewModels
{
    [QueryProperty(nameof(ActivoId), "activoId")]
    public partial class HistorialViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private int activoId;

        [ObservableProperty]
        private string nombreActivo = string.Empty;

        public ObservableCollection<HistorialEstado> Historial { get; } = new();

        public HistorialViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Historial de Estados";
        }

        async partial void OnActivoIdChanged(int value)
        {
            if (value > 0)
            {
                await CargarHistorialAsync(value);
            }
        }

        [RelayCommand]
        private async Task CargarHistorialAsync(int id)
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var activo = await _databaseService.ObtenerActivoPorIdAsync(id);
                NombreActivo = activo?.Nombre ?? string.Empty;

                var movimientos = await _databaseService.ObtenerHistorialPorActivoAsync(id);

                Historial.Clear();
                foreach (var item in movimientos.OrderByDescending(h => h.FechaCambio))
                    Historial.Add(item);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el historial: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}