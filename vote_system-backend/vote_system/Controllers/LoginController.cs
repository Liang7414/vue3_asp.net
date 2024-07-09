using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using vote_system.Models;

namespace vote_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public LoginController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult Post(LoginModel user)
        {
            // 检查 user.IdNumber 和 user.Password 是否为 null
            if (string.IsNullOrEmpty(user.IdNumber) || string.IsNullOrEmpty(user.Password))
            {
                return BadRequest("IdNumber 和 Password 不能为空");
            }

            // 获取数据库连接字符串
            string? connectionString = _configuration.GetConnectionString("DefaultConnection");

            using (MySqlConnection conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                string query = "use sql_test;SELECT * FROM user WHERE idNumber = @IdNumber AND password_ = @Password";

                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@IdNumber", user.IdNumber);
                    cmd.Parameters.AddWithValue("@Password", user.Password);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            bool voteStatus = Convert.ToBoolean(reader["vote_status"]);
                            if (!voteStatus)
                            {
                                return Ok(new { success = true });
                            }
                            else
                            {
                                return Ok(new { success = false });
                            }
                        }
                        else
                        {
                            return Unauthorized();
                        }
                    }
                }
            }
        }
    }
}