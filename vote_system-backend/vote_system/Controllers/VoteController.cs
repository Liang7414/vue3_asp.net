using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using vote_system.Models;

namespace vote_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public VoteController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(VoteModel user)
        {
            // 获取数据库连接字符串
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "UPDATE user SET vote_choice = @VoteChoice, vote_status = @VoteStatus WHERE idNumber = @IdNumber";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdNumber", user.IdNumber ?? string.Empty); // 处理可能的null值
                    cmd.Parameters.AddWithValue("@VoteChoice", user.VoteChoice);
                    cmd.Parameters.AddWithValue("@VoteStatus", true);

                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        return Ok(new { success = true });
                    }
                    else
                    {
                        return BadRequest(new { success = false});
                    }
                }
            }
        }
    }
}