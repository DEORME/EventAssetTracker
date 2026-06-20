namespace EventAssetTracker
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute("ListaActivosPage", typeof(Views.ListaActivosPage));
            Routing.RegisterRoute("ActivoFormPage", typeof(Views.ActivoFormPage));
            Routing.RegisterRoute("CambiarEstadoPage", typeof(Views.CambiarEstadoPage));
            Routing.RegisterRoute("HistorialPage", typeof(Views.HistorialPage));
        }
    }
}
