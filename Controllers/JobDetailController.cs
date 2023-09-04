using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class JobDetailController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=root;Database=job_portal;";

        [HttpGet("{id}")]
        public IActionResult GetJobDetails(int id)
        {
            Console.WriteLine(1);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                try
                {
                    Console.WriteLine(2);
                    connection.Open();
                    string query = $"select j.*, group_concat(distinct r.name ) as role_names, group_concat(distinct t.time_slot) as time_slots,  group_concat(distinct i.title) as instruction_titles,  group_concat(distinct i.instruction) as instructions from job_openings j join roles_master r on find_in_set(r.id, j.role_id)  join time_slots t on t.job_id = j.id  left join instructions i on i.job_id = j.id where j.id = {id} Group by j.id;";
                    MySqlCommand command = new MySqlCommand(query, connection);

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        List<DataDto1> dataList = new List<DataDto1>();

                        while (reader.Read())
                        {
                            DataDto1 data = new DataDto1
                            {
                                Id = Convert.ToInt32(reader["id"]),
                                Title = reader["title"].ToString(),
                                Location = reader["location"].ToString(),
                                Date_from = reader["date_from"].ToString(),
                                Date_to = reader["date_to"].ToString(),
                                Role_id = reader["role_id"].ToString(),
                                Role_names = reader["role_names"].ToString(),
                                Offers = reader["offers"].ToString(),
                                Expires_on = reader["expires_on"].ToString(),
                                Applicants = reader["applicants"].ToString(),
                                Venue = reader["venue"].ToString(),
                                Time_slots = reader["time_slots"].ToString(),
                                Instruction_titles = reader["instruction_titles"].ToString(),
                                Instructions = reader["instructions"].ToString(),

                                Created_by = reader["created_by"].ToString(),
                                Created_at = reader["created_at"].ToString(),
                                Updated_at = reader["updated_at"].ToString()
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

    public class DataDto1
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Date_from { get; set; }
        public string Date_to { get; set; }
        public string Role_id { get; set; }
        public string Role_names { get; set; }
        public string Offers { get; set; }
        public string Expires_on { get; set; }
        public string Applicants { get; set; }
        public string Venue { get; set; }
        public string Time_slots { get; set; }
        public string Instruction_titles { get; set; }
        public string Instructions { get; set; }
        public string Created_by { get; set; }
        public string Created_at { get; set; }
        public string Updated_at { get; set; }


    }
}

