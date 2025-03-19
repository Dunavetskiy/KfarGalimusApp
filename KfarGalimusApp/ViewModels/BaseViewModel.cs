using CommunityToolkit.Mvvm.ComponentModel;
using System.Xml;

namespace KfarGalimusApp.ViewModels
{
    public abstract class BaseViewModel : ObservableObject
    {
        private bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }


        private string _errorMessage;
        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public void ClearErrorMessage()
        {
            ErrorMessage = string.Empty;
        }
    }
}
