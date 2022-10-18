namespace Models.Validation
{
    public class ModelsValidation
    {
        public const int MaxEmailLength                 = 256;
        public const int MaxPasswordNameLength          = 256;
        public const int MaxUserNameLength              = 512;
        public const int MaxPasswordLength              = 1024;
        public const int MaxPasswordDescriptionLength   = 512;
        
        public const string MaxEmailLengthErrorMessage                  = "Email can't be longer than {1} characters";
        public const string MaxPasswordNameLengthErrorMessage           = "Password name can't be longer than {1} characters";
        public const string MaxUserNameLengthErrorMessage               = "User name can't be longer than {1} characters";
        public const string MaxPasswordLengthErrorMessage               = "Password can't be longer than {1} characters";
        public const string MaxPasswordDescriptionLengthErrorMessage    = "Password description can't be longer than {1} characters";
        public const string PasswordsDoNotMatchErrorMessage             = "Passwords do not match";

        public const string RequiredErrorMessage                        = "{0} is required.";
    }
}
