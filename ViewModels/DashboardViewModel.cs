using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EventAssetTracker.Models;
using EventAssetTracker.Services;

namespace EventAssetTracker.ViewModels
{
    public partial class DashboardViewModel : BaseViewModel
    {
        private readonly IDatabaseService _databaseService;

        [ObservableProperty]
        private int totalActivos;

        [ObservableProperty]
        private int totalEnUso;

        [ObservableProperty]
        private int totalEnMantenimiento;

        [ObservableProperty]
        private int totalFueraDeServicio;

        public DashboardViewModel(IDatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Bright Events Co. - Dashboard";
        }

        [RelayCommand]
        private async Task CargarResumenAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                TotalActivos = await _databaseService.ContarActivosAsync();
                TotalEnUso = await _databaseService.ContarPorEstadoAsync(EstadoActivo.EnUso);
                TotalEnMantenimiento = await _databaseService.ContarPorEstadoAsync(EstadoActivo.EnMantenimiento);
                TotalFueraDeServicio = await _databaseService.ContarPorEstadoAsync(EstadoActivo.FueraDeServicio);
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"No se pudo cargar el resumen: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task IrAGestionActivosAsync()
        {
            await Shell.Current.GoToAsync("ListaActivosPage");
        }
    }
}