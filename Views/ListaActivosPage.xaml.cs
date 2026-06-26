using EventAssetTracker.ViewModels;

namespace EventAssetTracker.Views
{
    public partial class ListaActivosPage : ContentPage
    {
        public ListaActivosPage(ListaActivosViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            if (BindingContext is ListaActivosViewModel vm)
                await vm.CargarActivosCommand.ExecuteAsync(null);
        }
    }
}