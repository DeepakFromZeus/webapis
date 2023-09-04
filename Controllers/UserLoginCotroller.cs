using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserLoginController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=root;Database=job_portal;";

        [HttpGet("{username}/{pasword}")]
        public IActionResult UserLogin(string username, string password)
        {
            Console.WriteLine(1);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine(2);
                    connection.Open();
                    string query = $"select * from users where email='{username}' and password ='{password}' and active = 1;";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<UserDto> dataList = new List<UserDto>();

                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                UserDto data = new UserDto
                                {
                                    Id = Convert.ToInt32(reader["id"]),
                                    First_name = reader["first_name"].ToString(),
                                    Last_name = reader["last_name"].ToString()
                                };
                                dataList.Add(data);
                            }
                            Console.WriteLine(3);
                            DataTable datatTable = new DataTable();
                            datatTable.Load(reader);
                            Console.WriteLine(datatTable);

                            // return Content(dataList);

                            return Ok(new { Status = 200, Data = dataList });

                            // return Ok("asd");
                        }
                        else
                        {
                            return Unauthorized(new { Status = 401, Message = "Invalid email or password." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, "An Error occured while fetching data");
                }
            }
        }
    }

    public class UserDto
    {
        public int Id { get; set; }
        public string First_name { get; set; }
        public string Last_name { get; set; }

    }
}

