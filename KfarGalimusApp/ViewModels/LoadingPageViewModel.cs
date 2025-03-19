using Firebase.Auth;
using KfarGalimusApp.Pages;

namespace KfarGalimusApp.ViewModels
{
    public partial class LoadingPageViewModel : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;

        public LoadingPageViewModel(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
            CheckUserLoginDetails();
        }

        private async Task CheckUserLoginDetails()
        {
            try
            {
                await Task.Delay(200); 

                if (_firebaseAuthClient?.User == null || string.IsNullOrWhiteSpace(_firebaseAuthClient.User.Info?.Email))
                {
                    await NavigateToOnboarding();
                }
                else
                {
                    await Shell.Current.GoToAsync($"//{nameof(MaOmerPage)}");
                }
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to check login: {ex.Message}", "OK");
            }
        }

        private async Task NavigateToOnboarding()
        {
            await MainThread.InvokeOnMainThreadAsync(async () =>
            {
                await Shell.Current.GoToAsync($"//{nameof(OnboardingPage)}");
            });
        }
    }
}
