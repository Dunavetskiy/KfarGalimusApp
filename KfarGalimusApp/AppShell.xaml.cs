using KfarGalimusApp.Handlers;
using KfarGalimusApp.Pages;
using KfarGalimusApp.ViewModels;

namespace KfarGalimusApp
{
    public partial class AppShell : Shell
    {
        private readonly AppShellViewModel _appShellViewModel;

        public AppShell(AppShellViewModel appShellViewModel)
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(MaOmerPage), typeof(MaOmerPage));
            Routing.RegisterRoute(nameof(LoadingPage), typeof(LoadingPage));
            Routing.RegisterRoute(nameof(SignUpPage), typeof(SignUpPage));
            Routing.RegisterRoute(nameof(SignInPage), typeof(SignInPage));
            Routing.RegisterRoute(nameof(OnboardingPage), typeof(OnboardingPage));
            Routing.RegisterRoute(nameof(CreatePostPage), typeof(CreatePostPage));
            Routing.RegisterRoute(nameof(ProfilePage), typeof(ProfilePage));
            BindingContext = _appShellViewModel = appShellViewModel;

        }
    }
}
