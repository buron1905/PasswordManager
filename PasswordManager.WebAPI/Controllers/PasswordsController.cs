using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PasswordManager.WebAPI.Models.Passwords;
using System.Data;

namespace PasswordManager.WebAPI.Controllers
{
    public class PasswordsController : ApiControllerBase
    {
        private IMapper _mapper;

        public PasswordsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [HttpGet, Authorize]
        public async Task<IEnumerable<string>> Get()
        {
            //var passwordsResult = _mapper.Map<IEnumerable<PasswordDTO>>(passwords);

            return new string[] { "pass1", "pass2" };
        }


    }
}
