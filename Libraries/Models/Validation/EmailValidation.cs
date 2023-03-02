namespace Models.Validation
{
    // based on DataAnnotations email validation
    public static class EmailValidation
    {
        public static bool IsValid(string value)
        {
            if (value == null)
                return true;

            if ((value.Contains('\r') || value.Contains('\n')))
                return false;

            // only return true if there is only 1 '@' character
            // and it is neither the first nor the last character
            int index = value.IndexOf('@');

            return
                index > 0 &&
                index != value.Length - 1 &&
                index == value.LastIndexOf('@');
        }
    }
}
