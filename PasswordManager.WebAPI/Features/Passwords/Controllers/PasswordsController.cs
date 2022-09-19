using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace PasswordManager.WebAPI.Features.Passwords.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class PasswordsController : ControllerBase
    {
        [HttpGet, Authorize]
        public IEnumerable<string> Get()
        {
            return new string[] { "pass1", "pass2" };
        }
    }
}
