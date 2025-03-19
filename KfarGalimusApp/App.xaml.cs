using KfarGalimusApp.Pages;
using Microsoft.Extensions.DependencyInjection;

namespace KfarGalimusApp
{
    public partial class App : Application
    {
        public App(IServiceProvider serviceProvider)
        {
            InitializeComponent();

            MainPage = serviceProvider.GetRequiredService<AppShell>();
        }
    }
}
