using C971_Ogden.Pages;

namespace C971_Ogden
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(TermDetails), typeof(TermDetails));
        }
    }
}
