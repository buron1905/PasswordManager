using PasswordManager.WebAPI.Helpers.Attributes;

namespace PasswordManager.WebAPI.Controllers
{
    [Authorize]
    public class SettingsController : ApiControllerBase
    {

        public SettingsController()
        {
        }

    }
}
