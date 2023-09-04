using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]


    public class UserRegistrationController : ControllerBase
    {
        private readonly string _connectionString = "Server=localhost;Uid=root;Pwd=root;Database=job_portal;";

        [HttpPost]
        public IActionResult GetJobInstructions([FromBody] UserRegistration userRegistration)
        {
            Console.WriteLine(1);
            using (MySqlConnection connection = new MySqlConnection(_connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        Console.WriteLine(2);
                        connection.Open();
                        string query = "Insert into users (email,password,first_name,last_name,phone_no,portfolio_url,preferred_roles_id) values (@email,'123',@firstName,@lastName,@phnNumber,@url,@prefered);";

                        MySqlCommand command = new MySqlCommand(query, connection, transaction);

                        command.Parameters.AddWithValue("@email", userRegistration.email);
                        command.Parameters.AddWithValue("@firstName", userRegistration.firstName);
                        command.Parameters.AddWithValue("@lastName", userRegistration.lastName);
                        command.Parameters.AddWithValue("@url", userRegistration.url);
                        command.Parameters.AddWithValue("@phnNumber", userRegistration.phnNumber);
                        command.Parameters.AddWithValue("@prefered", userRegistration.prefered);

                        int rowsAffected = command.ExecuteNonQuery();

                        int userId = (int)command.LastInsertedId;
                        //                    

                        string userProfileInsertQuery = " Insert into qualification (user_id,degree,stream,college,percentage,year_of_passing,college_location) values (@UserId,@degree,@stream,@college,@percentage,@yearOfPassing,@location);";
                        MySqlCommand userProfileInsertCommand = new MySqlCommand(userProfileInsertQuery, connection, transaction);
                        // Assuming the 'users' table has an auto-incrementing ID
                        userProfileInsertCommand.Parameters.AddWithValue("@UserId", userId);
                        userProfileInsertCommand.Parameters.AddWithValue("@degree", userRegistration.degree);
                        userProfileInsertCommand.Parameters.AddWithValue("@stream", userRegistration.stream);
                        userProfileInsertCommand.Parameters.AddWithValue("@college", userRegistration.college);
                        userProfileInsertCommand.Parameters.AddWithValue("@percentage", userRegistration.percentage);
                        userProfileInsertCommand.Parameters.AddWithValue("@yearOfPassing", userRegistration.yearOfPassing);
                        userProfileInsertCommand.Parameters.AddWithValue("@location", userRegistration.location);

                        userProfileInsertCommand.ExecuteNonQuery();

                        string userProfessionaltQuery = " Insert into professional_qualification (user_id,years_of_experience,current_ctc,expected_ctc,technologies_expertise_id,other_technology_expertise,familiar_technologies_id,notice_period,notice_period_end, notie_period_in_months,previous_test,previous_role_id) values (@UserId,@yearsOfExp,@currentCTC,@expectedCTC,@expertTech,'',@famTech,@notice,@noticeEndDate,@noticeMonths,@test,@appliedRole);";

                        MySqlCommand userProfessionaltCommand = new MySqlCommand(userProfessionaltQuery, connection, transaction);
                        // Assuming the 'users' table has an auto-incrementing ID
                        userProfessionaltCommand.Parameters.AddWithValue("@UserId", userId);
                        userProfessionaltCommand.Parameters.AddWithValue("@yearsOfExp", userRegistration.yearsOfExp);
                        userProfessionaltCommand.Parameters.AddWithValue("@currentCTC", userRegistration.currentCTC);
                        userProfessionaltCommand.Parameters.AddWithValue("@expectedCTC", userRegistration.expectedCTC);
                        userProfessionaltCommand.Parameters.AddWithValue("@expertTech", userRegistration.expertTech);
                        userProfessionaltCommand.Parameters.AddWithValue("@famTech", userRegistration.famTech);
                        userProfessionaltCommand.Parameters.AddWithValue("@notice", userRegistration.notice);
                        userProfessionaltCommand.Parameters.AddWithValue("@noticeEndDate", userRegistration.noticeEndDate);
                        userProfessionaltCommand.Parameters.AddWithValue("@noticeMonths", userRegistration.noticeMonths);
                        userProfessionaltCommand.Parameters.AddWithValue("@test", userRegistration.test);
                        userProfessionaltCommand.Parameters.AddWithValue("@appliedRole", userRegistration.appliedRole);

                        userProfessionaltCommand.ExecuteNonQuery();

                        transaction.Commit();
                        return Ok("User registered successfully.");

                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                        return StatusCode(500, ex.Message);
                    }
                }
            }
        }
    }

    public class UserRegistration
    {

        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phnNumber { get; set; }
        public string url { get; set; }
        public string prefered { get; set; }
        public string percentage { get; set; }
        public string college { get; set; }
        public string degree { get; set; }
        public string stream { get; set; }
        public string location { get; set; }
        public string yearOfPassing { get; set; }
        public string applicantType { get; set; }
        public string yearsOfExp { get; set; }
        public string expectedCTC { get; set; }
        public string currentCTC { get; set; }
        public string expertTech { get; set; }
        public string famTech { get; set; }
        public string notice { get; set; }
        public string noticeEndDate { get; set; }
        public string noticeMonths { get; set; }
        public string test { get; set; }
        public string appliedRole { get; set; }




    }
}

