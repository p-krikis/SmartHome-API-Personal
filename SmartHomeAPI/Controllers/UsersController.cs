using Microsoft.AspNetCore.Mvc;
using SmartHomeAPI.Models;
using SmartHomeAPI.Services;

namespace SmartHomeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly Users_Service _usersService; //creates service interface

        public UsersController(Users_Service usersService)
        {
            _usersService = usersService; //assign service variable to interface value - same for all controllers
        }

        [HttpPost("UserSignup")]
        public async Task<IActionResult> UserSignup([FromBody] UserModels userModels)
        {
            string response = await _usersService.SignupUser(userModels);
            if (response == null)
            {
                return StatusCode(406, "Email already exists");
            }
            else
            {
                return StatusCode(201, response);
            }
        }
        [HttpPost("UserLogin")]
        public async Task<IActionResult> UserLogin([FromBody] LoginInfo loginInfo)
        {
            var loginCheck = await _usersService.LoginUser(loginInfo.LoginEmail, loginInfo.LoginPassword);
            if (loginCheck != null)
            {
                return Ok(loginCheck);
            }
            else
            {
                return Unauthorized();
            }
        }
        [HttpPut("UserUpdate/{id}")]
        public async Task<IActionResult> UserUpdate(string id, [FromBody]UserModels userModels)
        {
            await _usersService.UpdateUser(id, userModels);
            return Ok($"User with ID: {id} has been updated");
        }
        [HttpDelete("UserDelete/{id}")]
        public async Task<IActionResult> UserDeletion(string id)
        {
            await _usersService.DeleteUser(id);
            return Ok($"Deleted account with ID:{id}");
        }
        [HttpDelete("DEV_DELETE_DB_COLLECTION")]
        public async Task<IActionResult> ClearTable()
        {
            await _usersService.DEV_CLEAR_COLLECTION();
            return Ok();
        }
        [HttpPost("DEV_POPULATE_USER/{userID}/{roomAmount}/{deviceAmount}")]
        public async Task<IActionResult> PopulateUser(string userID, int roomAmount, int deviceAmount)
        {
            var response = await _usersService.PopulateUser(userID, roomAmount, deviceAmount);
            if (response != null)
            {
                return StatusCode(201, "populated user");
            }
            else
            {
                return BadRequest("User already populated. Cannot re-populate");
            }
        }
    }
}
