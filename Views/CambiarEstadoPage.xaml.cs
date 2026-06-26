using EventAssetTracker.ViewModels;

namespace EventAssetTracker.Views
{
    public partial class CambiarEstadoPage : ContentPage
    {
        public CambiarEstadoPage(CambiarEstadoViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}