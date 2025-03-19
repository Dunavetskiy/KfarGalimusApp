using KfarGalimusApp.ViewModels;

namespace KfarGalimusApp.Pages;

public partial class SignInPage : ContentPage
{
	private readonly SignInViewModel _signInViewModel;
    public SignInPage(SignInViewModel signInViewModel)
	{
		InitializeComponent();
        BindingContext = _signInViewModel = signInViewModel;

	}

}