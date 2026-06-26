using EventAssetTracker.ViewModels;

namespace EventAssetTracker.Views
{
    public partial class ActivoFormPage : ContentPage
    {
        public ActivoFormPage(ActivoFormViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}