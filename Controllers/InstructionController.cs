using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class InstructionController : ControllerBase
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
                    string query = $"SELECT * FROM instructions where job_ID + {id};";


                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<InstructionDto> dataList = new List<InstructionDto>();

                        while (reader.Read())
                        {
                            InstructionDto data = new InstructionDto
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Title = reader["title"].ToString(),
                                Instructions = reader["instruction"].ToString(),
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

    public class InstructionDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Instructions { get; set; }
    }
}

