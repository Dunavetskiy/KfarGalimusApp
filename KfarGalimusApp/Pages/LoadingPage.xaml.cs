using KfarGalimusApp.ViewModels;

namespace KfarGalimusApp.Pages;

public partial class LoadingPage : ContentPage
{
	private readonly LoadingPageViewModel _loadingPageViewModel;

    public LoadingPage(LoadingPageViewModel loadingPageViewModel)
	{
		InitializeComponent();
		BindingContext = _loadingPageViewModel = loadingPageViewModel;
	}
}