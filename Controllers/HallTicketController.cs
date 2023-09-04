using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class HallTicketController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=root;Database=job_portal;";

        [HttpGet]
        public IActionResult GetHallTicket()
        {
            Console.WriteLine(1);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine(2);
                    connection.Open();
                    string query = "Select j.title,j.date_from, j.venue,a.time_slot,i.instruction from job_openings j,applicants a, instructions i  where j.id = a.job_id and i.job_id = j.id and i.title = 'Things to remember';";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<HallTicketDto> dataList = new List<HallTicketDto>();

                        while (reader.Read())
                        {
                            HallTicketDto data = new HallTicketDto
                            {
                                Date_from = reader["date_from"].ToString(),
                                Time_slot = reader["time_slot"].ToString(),
                                Instruction = reader["instruction"].ToString(),
                                Venue = reader["venue"].ToString(),

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
                    return StatusCode(500, "An Error occured while fetching data");
                }
            }
        }
    }

    public class HallTicketDto
    {
        public string Title { get; set; }
        public string Date_from { get; set; }
        public string Time_slot { get; set; }
        public string Instruction { get; set; }
        public string Venue { get; set; }

    }
}

