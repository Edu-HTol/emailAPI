using MailingApi.Interfaces;
using MailingApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MailingApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private IUserAuthenticationService _userService;
        public UserController(IUserAuthenticationService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Authenticates user
        /// </summary>
        /// <param name="model">login and password</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public IActionResult Authenticate([FromBody] BusinessModelUser model)
        {
            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
                return BadRequest();

            return Ok(user);
        }
        /// <summary>
        /// Register new user
        /// </summary>
        /// <param name="model">login and password</param>
        /// <returns></returns>
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [AllowAnonymous]
        [HttpPost]
        [Route("register")]
        public IActionResult Register([FromBody] BusinessModelUser model)
        {
            if(string.IsNullOrEmpty(model.Username) || string.IsNullOrEmpty(model.Password))
                return BadRequest();
            var user = _userService.Register(model.Username, model.Password);
            if (user == false)
                return BadRequest();

            return Ok(user);
        }

    }
}
