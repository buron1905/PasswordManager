namespace Models.Validation
{
    public class ModelsValidation
    {
        public const int MaxEmailLength = 256;
        public const int MaxPasswordNameLength = 256;
        public const int MaxUserNameLength = 512;
        public const int MaxPasswordLength = 1024;
        public const int MaxURLLength = 2048;
        public const int MaxNotesLength = 100000;

        public const string MaxEmailLengthErrorMessage = "Email can't be longer than {1} characters.";
        public const string MaxPasswordNameLengthErrorMessage = "Password name can't be longer than {1} characters.";
        public const string MaxUserNameLengthErrorMessage = "User name can't be longer than {1} characters.";
        public const string MaxPasswordLengthErrorMessage = "Password can't be longer than {1} characters.";
        public const string MaxNotesLengthErrorMessage = "Password notes can't be longer than {1} characters.";
        public const string MaxURLLengthErrorMessage = "URL can't be longer than {1} characters.";
        public const string PasswordsDoNotMatchErrorMessage = "Passwords do not match.";

        public const string RequiredEmailAddressErrorMessage = "The Email Address field is required.";
        public const string RequiredConfirmPasswordErrorMessage = "The Confirm Password field is required.";
        public const string RequiredPasswordNameErrorMessage = "The Password Name field is required.";
        public const string InvalidEmailAddressFormatErrorMessage = "The Email Address field is not a valid e-mail address.";

        //public string RequiredErrorMessage(string fieldName) => $"The {fieldName} field is required.";
        //public const string RequiredErrorMessage = "{0} is required.";

    }
}
