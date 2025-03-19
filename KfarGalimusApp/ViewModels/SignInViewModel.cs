using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using KfarGalimusApp.Models;
using KfarGalimusApp.Pages;

namespace KfarGalimusApp.ViewModels
{
    public partial class SignInViewModel : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

        [ObservableProperty]
        private SignInModel _signInModel = new();

        [ObservableProperty]
        private string _errorMessage;

        public SignInViewModel(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
        }

        [RelayCommand]
        private async Task SignIn()
        {
            try
            {
                IsBusy = true;
                ClearErrorMessage();

                var result = await _firebaseAuthClient.SignInWithEmailAndPasswordAsync(
                    SignInModel.Email, SignInModel.Password);

                if (!string.IsNullOrWhiteSpace(result?.User?.Info?.Email))
                {
                    Preferences.Set("UserId", result.User.Info.Uid);
                    Preferences.Set("UserName", result.User.Info.DisplayName ?? "User");

                    await Shell.Current.GoToAsync($"//{nameof(MaOmerPage)}", true);
                }
                else
                {
                    ErrorMessage = "Invalid login credentials.";
                    await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}", true);
                }
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Ошибка входа: {ex.Message}";
                Console.WriteLine($"Ошибка входа: {ex.Message}");
                await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}", true);
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task NavigateSignUp()
        {
            await Shell.Current.GoToAsync($"//{nameof(SignUpPage)}", true);
        }
    }
}
