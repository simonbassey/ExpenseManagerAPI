using System;
using ExpenseMgr.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ExpenseMgr.Domain;
using ExpenseMgr.Domain.Models;
using System.Net;
using System.Linq;

namespace ExpenseMgr.API.Controllers
{
    [Route("api/users")]
    public class UserController : Controller
    {
        private readonly IUserService userService;
        public UserController(IUserService userService_)
        {
            userService = userService_;
        }

        [HttpPost("create")]
        public async Task<IActionResult> Create([FromBody] NewUserModel user)
        {
            try
            {
                if (user == null)
                    return BadRequest("invalid request payload");
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);
                var userAccount = await userService.GetUser(user.Email); ;
                if (userAccount != null)
                    return BadRequest("User already exist");
                var result = await userService.CreateUserAsync(new User
                {
                    Email = user.Email,
                    UserName = string.IsNullOrEmpty(user.UserName) ? user.Email : user.UserName
                });
                return result == null ? StatusCode(204, "") : StatusCode((int)HttpStatusCode.OK, result);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var users = (await userService.GetUsers()).ToList();
                return Ok(users);
            }
            catch (Exception exception)
            {
                return StatusCode((int)HttpStatusCode.InternalServerError, exception);
            }
        }
    }
}
