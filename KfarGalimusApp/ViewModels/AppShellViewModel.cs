using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using KfarGalimusApp.Pages;
using System.Diagnostics;

namespace KfarGalimusApp.ViewModels
{
    public partial class AppShellViewModel : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

        public AppShellViewModel(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient ?? throw new ArgumentNullException(nameof(firebaseAuthClient), "FirebaseAuthClient cannot be null.");
        }


        [RelayCommand]
        async Task Logout()
        {
            bool answer = await Shell.Current.DisplayAlert(
                "Logout", "Are you sure you want to Logout?", "Yes", "No");

            if (answer)
            {
                try
                {
                    if (_firebaseAuthClient != null)
                    {
                        _firebaseAuthClient.SignOut();
                    }
                    else
                    {
                        Debug.WriteLine($"Firebase Error: 35");
                        Console.WriteLine("FirebaseAuthClient is null");
                    }

                    Preferences.Remove("UserId");
                    Preferences.Remove("UserName");
                    Preferences.Remove("Email");

                    if (Shell.Current != null)
                    {
                        await Shell.Current.GoToAsync(nameof(OnboardingPage));
                    }
                    else
                    {
                        Console.WriteLine("Shell.Current is null");
                        Debug.WriteLine($"Shell.Current is null");
                    }
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.Message);

                    Console.WriteLine($"Ошибка при выходе: {ex.Message}");
                }
            }
        }

    }
}
