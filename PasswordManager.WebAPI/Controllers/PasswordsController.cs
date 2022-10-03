using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PasswordManager.WebAPI.Controllers
{
    public class PasswordsController : ApiControllerBase
    {
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "pass1", "pass2" };
        }
    }
}
