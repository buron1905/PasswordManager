﻿using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace PasswordManager.MAUI.Helpers
{
    // Code from https://www.codeproject.com/Articles/1274851/Xamarin-Forms-Validations-Made-Simple
    public static class ValidationHelper
    {
        public static bool ValidateForm(object model, Page page)
        {
            HideValidationFields(model, page);
            var errors = new List<ValidationResult>();
            var context = new ValidationContext(model);
            bool isValid = Validator.TryValidateObject(model, context, errors, true);
            if (!isValid)
            {
                ShowValidationFields(errors, model, page);
            }
            return errors.Count() == 0;
        }
        private static void HideValidationFields
            (object model, Page page, string validationLabelSuffix = "Error")
        {
            if (model == null) { return; }
            var properties = GetValidatablePropertyNames(model);
            foreach (var propertyName in properties)
            {
                var errorControlName =
                $"{propertyName}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);
                if (control != null)
                {
                    control.IsVisible = false;
                }
            }
        }

        private static void ShowValidationFields
        (List<ValidationResult> errors,
        object model, Page page, string validationLabelSuffix = "Error")
        {
            if (model == null) { return; }
            foreach (var error in errors)
            {
                var memberName = $"{error.MemberNames.FirstOrDefault()}";
                var errorControlName = $"{memberName}{validationLabelSuffix}";
                var control = page.FindByName<Label>(errorControlName);
                if (control != null)
                {
                    control.Text = $"{error.ErrorMessage}{Environment.NewLine}";
                    control.IsVisible = true;
                }
            }
        }

        private static IEnumerable<string> GetValidatablePropertyNames(object model)
        {
            var validatableProperties = new List<string>();
            var properties = GetValidatableProperties(model);
            foreach (var propertyInfo in properties)
            {
                var errorControlName = $"{propertyInfo.Name}";
                validatableProperties.Add(errorControlName);
            }
            return validatableProperties;
        }

        private static List<PropertyInfo> GetValidatableProperties(object model)
        {
            var properties = model.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(prop => prop.CanRead
                && prop.GetCustomAttributes(typeof(ValidationAttribute), true).Any()
                && prop.GetIndexParameters().Length == 0).ToList();
            return properties;
        }
    }
}
