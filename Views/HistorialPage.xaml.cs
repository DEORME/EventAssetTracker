using EventAssetTracker.ViewModels;

namespace EventAssetTracker.Views
{
    public partial class HistorialPage : ContentPage
    {
        public HistorialPage(HistorialViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}