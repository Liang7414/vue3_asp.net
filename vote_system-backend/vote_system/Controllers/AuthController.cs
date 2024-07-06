using Microsoft.AspNetCore.Mvc;
using vote_system.Models;


namespace YourNamespace.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private static Dictionary<string, string> userStore = new Dictionary<string, string>
        {
            { "A123456789", "password123" }
        };

        private static HashSet<string> votedUsers = new HashSet<string>();

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginModel model)
        {
            if (userStore.ContainsKey(model.IdNumber) && userStore[model.IdNumber] == model.Password)
            {
                if (votedUsers.Contains(model.IdNumber))
                {
                    return Ok(new { success = false, vote_status = true });
                }
                else
                {
                    return Ok(new { success = true, vote_status = false });
                }
            }
            else
            {
                return Unauthorized(new { success = false, error = "Invalid credentials" });
            }
        }

        [HttpPost("vote")]
        public IActionResult Vote([FromBody] VoteModel model)
        {
            if (!userStore.ContainsKey(model.IdNumber))
            {
                return Unauthorized(new { success = false, error = "Invalid user" });
            }

            if (votedUsers.Contains(model.IdNumber))
            {
                return BadRequest(new { success = false, error = "You have already voted" });
            }

            votedUsers.Add(model.IdNumber);
            return Ok(new { success = true });
        }
    }
}