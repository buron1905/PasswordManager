using MvvmHelpers;
using PasswordManager.MAUI.Helpers;

namespace PasswordManager.MAUI.ViewModels
{
    public abstract class BaseWithValidationViewModel<T> : BaseViewModel
    {
        public Page Page { get; set; }

        public BaseWithValidationViewModel()
        {
            //Model = new T();
        }

        T _model;
        public T Model
        {
            get => _model;
            set => SetProperty(ref _model, value);
        }

        public bool IsFormValid()
        {
            return ValidationHelper.IsFormValid(Model, Page);
        }
    }
}
