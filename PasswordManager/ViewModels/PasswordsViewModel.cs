using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PasswordManager.ViewModels
{
    public class PasswordsViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public PasswordsViewModel()
        {
        }

    }
}
