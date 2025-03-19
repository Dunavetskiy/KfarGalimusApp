using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Auth;
using KfarGalimusApp.Models;

namespace KfarGalimusApp.ViewModels
{
    public partial class ProfileViewModel : BaseViewModel
    {
        private readonly FirebaseClient _firebaseDatabase;
        private readonly FirebaseAuthClient _firebaseAuthClient;
        private string _userId;

        public ProfileViewModel(FirebaseAuthClient firebaseAuthClient)
        {
            _firebaseAuthClient = firebaseAuthClient;
            _firebaseDatabase = new FirebaseClient("https://kfargalimusapp-default-rtdb.firebaseio.com/");
            _userId = Preferences.Get("UserId", string.Empty);

            // Инициализация списка ролей
            Roles = new List<string> { "Teacher", "Student" };

            // Загрузка данных пользователя
            LoadUserDataAsync();
        }

        [ObservableProperty]
        private string _userName;

        [ObservableProperty]
        private string _email;

        [ObservableProperty]
        private string _avatarUrl = "avatardefault.png";

        [ObservableProperty]
        private bool _isLoading;

        [ObservableProperty]
        private List<string> _roles; // Список ролей

        [ObservableProperty]
        private string _selectedRole;

        [ObservableProperty]
        private string _interestsInput;

        private async void LoadUserDataAsync()
        {
            await LoadUserData();
        }

        public async Task LoadUserData()
        {
            if (string.IsNullOrEmpty(_userId))
            {
                Console.WriteLine("Ошибка: UserId отсутствует в Preferences.");
                return;
            }

            Console.WriteLine($"🔹 Загрузка данных пользователя с ID: {_userId}");

            try
            {
                IsLoading = true;

                var user = await _firebaseDatabase.Child("users")
                    .Child(_userId)
                    .OnceSingleAsync<UserModel>();

                if (user != null)
                {
                    Console.WriteLine($"✅ Данные загружены: {user.UserName}, {user.Email}");
                    UserName = user.UserName ?? string.Empty; // Обработка возможного null
                    Email = user.Email ?? string.Empty; // Обработка возможного null
                    SelectedRole = user.Role ?? "Student"; // Обработка возможного null
                    InterestsInput = string.Join(", ", user.Interests ?? new List<string>()); // Обработка возможного null
                }
                else
                {
                    Console.WriteLine("⚠️ Ошибка: пользователь не найден в базе.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Ошибка загрузки профиля: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        [RelayCommand]
        private async Task ChangeAvatar()
        {
            try
            {
                var result = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Choose a new avatar",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null)
                {
                    Console.WriteLine("File picking was canceled.");
                    return;
                }

                var stream = await result.OpenReadAsync();
                string imageUrl = await UploadImageToFirebase(stream, result.FileName);

                if (!string.IsNullOrEmpty(imageUrl))
                {
                    AvatarUrl = imageUrl;

                    await _firebaseDatabase.Child("users").Child(_userId)
                        .Child("AvatarUrl").PutAsync(imageUrl);
                }
                else
                {
                    Console.WriteLine("Ошибка загрузки изображения: URL пустой");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error changing avatar: {ex.Message}");
            }
        }

        private async Task<string> UploadImageToFirebase(Stream imageStream, string fileName)
        {
            try
            {
                var storage = new Firebase.Storage.FirebaseStorage("your-app.appspot.com");
                var imageUrl = await storage.Child("avatars").Child(fileName).PutAsync(imageStream);
                return imageUrl;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка загрузки изображения: {ex.Message}");
                return string.Empty;
            }
        }

        [RelayCommand]
        private async Task SaveProfile()
        {
            try
            {
                IsBusy = true;

                // Проверка, что InterestsInput не пуст
                if (string.IsNullOrWhiteSpace(InterestsInput))
                {
                    await Shell.Current.DisplayAlert("Error", "Interests cannot be empty.", "OK");
                    return;
                }

                // Проверка, что SelectedRole не пуст
                if (string.IsNullOrWhiteSpace(SelectedRole))
                {
                    await Shell.Current.DisplayAlert("Error", "Role cannot be empty.", "OK");
                    return;
                }

                // Обновление данных пользователя
                var updatedUser = new UserModel
                {
                    UserId = _userId,
                    UserName = UserName,
                    Email = Email,
                    Role = SelectedRole,
                    Interests = InterestsInput.Split(',').Select(i => i.Trim()).ToList()
                };

                // Сохранение в Firebase
                await _firebaseDatabase.Child("users").Child(_userId).PutAsync(updatedUser);

                // Обновление локальных данных
                Preferences.Set("Role", SelectedRole);
                Preferences.Set("Interests", InterestsInput);

                await Shell.Current.DisplayAlert("Success", "Profile updated successfully!", "OK");
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error", $"Failed to update profile: {ex.Message}", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}