using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using KfarGalimusApp.Models;
using KfarGalimusApp.Pages;
using System.Diagnostics;

namespace KfarGalimusApp.ViewModels
{
    public partial class SignUpViewModel : BaseViewModel
    {
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private readonly FirebaseClient _firebaseDatabase;

        public SignUpViewModel(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _firebaseDatabase = new FirebaseClient("https://kfargalimusapp-default-rtdb.firebaseio.com/");
        }

        [ObservableProperty]
        private SignUpModel _signUpModel = new();

        [ObservableProperty]
        private string _errorMessage;

        [RelayCommand]
        private async Task SignUp()
        {
            try
            {
                IsBusy = true;
                ErrorMessage = string.Empty;

                // Проверка, что все поля заполнены
                if (string.IsNullOrWhiteSpace(SignUpModel.Email) ||
                    string.IsNullOrWhiteSpace(SignUpModel.Password) ||
                    string.IsNullOrWhiteSpace(SignUpModel.UserName))
                {
                    ErrorMessage = "All the feilds mast be filled.";
                    return;
                }

                if (SignUpModel.Password.Length < 6)
                {
                    await Shell.Current.DisplayAlert("Error", "Password must be at least 6 characters long.", "OK");
                    return;
                }

                if (SignUpModel.UserName.Length > 15)
                {
                    await Shell.Current.DisplayAlert("Error", "Username must be no more than 15 characters long.", "OK");
                    return;
                }


                var result = await _firebaseAuthClient.CreateUserWithEmailAndPasswordAsync(SignUpModel.Email, SignUpModel.Password);
                var user = result?.User?.Info ?? throw new Exception("Error signUp_1");

                Debug.WriteLine("50");
                var newUser = new UserModel
                {
                    UserId = user.Uid,
                    Email = SignUpModel.Email,
                    UserName = SignUpModel.UserName,
                };

                Debug.WriteLine("Saving user to Firebase...");
                await _firebaseDatabase.Child("users").Child(user.Uid).PutAsync(newUser);
                Debug.WriteLine("User saved successfully!");

                // save on local
                Preferences.Set("UserId", user.Uid);
                Preferences.Set("UserName", SignUpModel.UserName);
                Preferences.Set("Email", SignUpModel.Email);

                try
                {
                    await Shell.Current.GoToAsync(nameof(SignInPage));
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Navigation error: {ex.Message}");
                    ErrorMessage = "Navigation failed. Please try again.";
                }

            }
            catch (FirebaseAuthException ex)
            {
                ErrorMessage = $"Error Firebase: {ex.Reason} | {ex.Message}";
                Console.WriteLine($"FirebaseAuthException: {ex.Reason} | {ex.Message}");
                Console.WriteLine($"StackTrace: {ex.StackTrace}");
                Debug.WriteLine($"Firebase Error: {ex.Reason} | {ex.Message}");
            }
            catch (Exception ex)
            {
                ErrorMessage = $"Error signUp: {ex.Message}";
                Console.WriteLine($"Error signUp_3: {ex}");
            }
            finally
            {
                IsBusy = false;
            }
        }


        [RelayCommand]
        private async Task NavigateSignIn()
        {
            await Shell.Current.GoToAsync($"//{nameof(SignInPage)}", true);
            Debug.WriteLine("91");
        }
    }
}
