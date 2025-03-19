using Firebase.Auth;
using Firebase.Auth.Providers;
using KfarGalimusApp.Pages;
using KfarGalimusApp.ViewModels;
using Microsoft.Extensions.Logging;
using Firebase.Database;

namespace KfarGalimusApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()

                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                    fonts.AddFont("Syne-VariableFont_wght.ttf", "Syne-VariableFont");
                    fonts.AddFont("NotoSansHebrew-Regular.ttf", "NotoSansHebrew-Regular");
                    fonts.AddFont("Wavetosh.ttf", "Wavetosh");

                });

#if DEBUG
            builder.Logging.AddDebug();
#endif

            builder.Services.AddSingleton<FirebaseAuthClient>(sp =>
                new FirebaseAuthClient(new FirebaseAuthConfig()
                {
                    ApiKey = "AIzaSyAsVeIbTS8zzrOZOyIo1g002164qToCW3U",
                    AuthDomain = "kfargalimusapp.firebaseapp.com",
                    Providers = new FirebaseAuthProvider[] { new EmailProvider() }
                })
            );

            builder.Services.AddSingleton(new FirebaseClient("https://kfargalimusapp-default-rtdb.firebaseio.com/"));

            //ViewModels
            builder.Services.AddSingleton<MaOmerViewModel>();
            builder.Services.AddSingleton<SignInViewModel>();
            builder.Services.AddSingleton<SignUpViewModel>();
            builder.Services.AddSingleton<LoadingPageViewModel>();
            builder.Services.AddSingleton<AppShellViewModel>();
            builder.Services.AddSingleton<ProfileViewModel>();

            //pages
            builder.Services.AddSingleton<ProfilePage>();
            builder.Services.AddSingleton<AppShell>();
            builder.Services.AddSingleton<MaOmerPage>();
            builder.Services.AddSingleton<LoadingPage>();
            builder.Services.AddSingleton<OnboardingPage>();
            builder.Services.AddSingleton<SignUpPage>();
            builder.Services.AddSingleton<SignInPage>();
            builder.Services.AddSingleton<CreatePostPage>();
            builder.Services.AddSingleton<AppShellViewModel>();


            return builder.Build();
        }
    }
}
