//using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models;
using System.Data;
using PasswordManager.WebAPI.Helpers;
using PasswordManager.WebAPI.Helpers.Attributes;

namespace PasswordManager.WebAPI.Controllers
{
    public class PasswordsController : ApiControllerBase
    {
        public PasswordsController()
        {
        }

        [HttpGet, Authorize]
        public async Task<IEnumerable<string>> Get()
        {
            //var passwordsResult = _mapper.Map<IEnumerable<PasswordDTO>>(passwords);

            return new string[] { "pass1", "pass2" };
        }


    }
}
