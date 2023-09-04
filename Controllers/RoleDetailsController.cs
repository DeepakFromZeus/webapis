using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class RoleDetailsController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=root;Database=job_portal;";

        [HttpGet("{id}")]
        public IActionResult GetJobInstructions(int id)
        {
            Console.WriteLine(1);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine(2);
                    connection.Open();
                    string query = $"SELECT j.*, r.* FROM job_openings j JOIN roles_master r ON FIND_IN_SET(r.id, j.role_id) > 0 where j.id={id};";


                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<RoleDetailDto> dataList = new List<RoleDetailDto>();

                        while (reader.Read())
                        {
                            RoleDetailDto data = new RoleDetailDto
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Rolename = reader["name"].ToString(),
                                Description = reader["description"].ToString(),
                                Package = reader["gross_compensation_package"].ToString(),
                                Requirement = reader["requirements"].ToString(),
                            };
                            dataList.Add(data);
                        }
                        Console.WriteLine(3);
                        DataTable datatTable = new DataTable();
                        datatTable.Load(reader);
                        Console.WriteLine(datatTable);



                        return Ok(dataList);

                        // return Ok("asd");
                    }

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return StatusCode(500, ex.Message);
                }
            }
        }
    }

    public class RoleDetailDto
    {
        public int Id { get; set; }
        public string Rolename { get; set; }
        public string Package { get; set; }
        public string Description { get; set; }
        public string Requirement { get; set; }
    }
}

