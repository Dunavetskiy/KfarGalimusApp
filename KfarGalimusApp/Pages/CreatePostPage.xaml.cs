using Firebase.Database;
using Firebase.Database.Query;
using KfarGalimusApp.Models;

namespace KfarGalimusApp.Pages;

public partial class CreatePostPage : ContentPage
{
    private readonly FirebaseClient _firebaseClient;
    private string _userName = "Anonim";
    private string _userId;

    public List<string> CategoryList { get; set; }

    public CreatePostPage(FirebaseClient firebaseClient)
    {
        InitializeComponent();
        _firebaseClient = firebaseClient;

        // Заполняем список категорий
        CategoryList = Enum.GetNames(typeof(PostModel.PostCategory)).ToList();
        BindingContext = this;  // Устанавливаем BindingContext для привязки данных

        Task.Run(async () => await LoadUserData());
    }

    private async Task LoadUserData()
    {
        _userId = Preferences.Get("UserId", string.Empty);
        if (string.IsNullOrEmpty(_userId))
        {
            await DisplayAlert("Error", "User has been not found", "OK");
            return;
        }

        var user = await _firebaseClient.Child("users").Child(_userId).OnceSingleAsync<UserModel>();

        if (user != null)
        {
            _userName = user.UserName;
        }
    }

    public async void OnSavePostClicked(object sender, EventArgs e)
    {
        // Validate user data
        if (string.IsNullOrEmpty(_userName) || string.IsNullOrEmpty(_userId))
        {
            await DisplayAlert("Error", "User data is not loaded. Please try again.", "OK");
            return;
        }

        // Validate category selection
        if (categoryPicker.SelectedItem is not string selectedCategoryString || !Enum.TryParse(typeof(PostModel.PostCategory), selectedCategoryString, out var category))
        {
            await DisplayAlert("Error", "Invalid or no category selected.", "OK");
            return;
        }

        // Create the post model
        var post = new PostModel
        {
            Title = TitleEntry.Text,
            Content = ContentEntry.Text,
            Date = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
            UserName = _userName,
            UserId = _userId,
            Category = (PostModel.PostCategory)category
        };

        // Try to save the post
        try
        {
            await _firebaseClient.Child("PostModel").PostAsync(post);
        }
        catch (Exception ex)
        {
            await DisplayAlert("Error", "Failed to save post: " + ex.Message, "OK");
            return;
        }

        // Clear input fields
        TitleEntry.Text = string.Empty;
        ContentEntry.Text = string.Empty;

        // Navigate back
        await Navigation.PopAsync();
    }
}
