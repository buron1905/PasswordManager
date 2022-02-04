using System;
using Microsoft.Maui.Controls;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using PasswordManager.Services;
using MAUIModelsLib;
using System.Collections.ObjectModel;

namespace PasswordManager.ViewModels
{
    public class PasswordDetailViewModel : BindableObject
    {
        public Action DisplayInvalidLoginPrompt;

        public PasswordDetailViewModel()
        {

        }
    }
}
