using Firebase.Auth;

namespace KfarGalimusApp.Controls;

public partial class FlyoutHeaderControl : StackLayout
{
    private readonly FirebaseAuthClient _firebaseAuthClient;

    public FlyoutHeaderControl(FirebaseAuthClient firebaseAuthClient)
    {
        InitializeComponent();
        _firebaseAuthClient = firebaseAuthClient;

        // Вызов LoadUserInfo() после того, как клиент будет инициализирован
        LoadUserInfo();
    }

    private void LoadUserInfo()
    {
        try
        {
            var user = _firebaseAuthClient.User;

            // Проверяем, что объект user не равен null
            if (user != null && user.Info != null)
            {
                lblUserName.Text = user.Info.DisplayName ?? "No Name";
                lblUserEmail.Text = user.Info.Email ?? "No Email";
            }
            else
            {
                lblUserName.Text = "No User";
                lblUserEmail.Text = "No Email";
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error: {ex.Message}");
            lblUserName.Text = "Error loading user info";
            lblUserEmail.Text = "Error loading email";
        }
    }
}
