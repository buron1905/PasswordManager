using MAUIModelsLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasswordManager.Services
{
    public class ActiveUserService      //normální heslo v ActiveUserService s vlastnosti heslo
    {
        public User User { get; set; }
        public string Password { get; set; }

        private static ActiveUserService instance;
        private ActiveUserService()
        {

        }

        public static ActiveUserService Instance
        {
            get 
            { 
                if(instance == null)
                {
                    instance = new ActiveUserService();
                }

                return instance;
            }
        }

        public bool IsActive
        { 
            get
            {
                return User != null;
            }
        }

        public void Logout()
        {
            User = null;
            Password = null;
        }

    }
}
