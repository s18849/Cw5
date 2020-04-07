using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/enrollments")]
    [ApiController]
    public class EnrollmentsController : ControllerBase
    {

        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18849;Integrated Security=True;MultipleActiveResultSets=True;";
        [HttpPost]
        public IActionResult EnrollStudent(EnrollStudentRequest request)
        {
            EnrollStudentResponse response;
            using(var con = new SqlConnection(ConString))
            using(var com = new SqlCommand())
            {
                
                    com.Connection = con;
                    con.Open();
                    var tran = con.BeginTransaction();
                    com.Transaction = tran;
                try
                {

                    com.CommandText = "select IdStudy from studies where name=@name";
                    com.Parameters.AddWithValue("name", request.Studies);

                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        tran.Rollback();
                        return BadRequest("Nie ma takich studiow");
                    }

                    int idStudies = (int)dr["IdStudy"];
                    com.CommandText = "select IdEnrollment from Enrollment where semester = 1 and idstudy =@idstudies and StartDate = (select max(StartDate) from Enrollment where semester =1 and idstudy = @idstudies)";
                    com.Parameters.AddWithValue("idstudies", idStudies);
                    dr.Close();
                    dr = com.ExecuteReader();
                    int idEnrollment;
                    if (!dr.Read())
                    {
                        idEnrollment = 0;
                    }
                    else
                    {
                        idEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                    }
                    if (idEnrollment == 0)
                    {
                        com.CommandText = "Insert into Enrollment(IdEnrollment, Semester, IdStudy, StartDate)"
                                  + "values ((select count(*)+1 from Enrollment), 1, @idstudiesADD, getdate())";
                        com.Parameters.AddWithValue("idstudiesADD", idStudies);
                        dr.Close();
                        dr = com.ExecuteReader();

                        com.CommandText = "select Idenrollment from Enrollment where semester = 1 and idstudy =@idstudies and StartDate =(select max(StartDate) from Enrollment where semester = 1 and idstudy = @idstudies)";
                        com.Parameters.AddWithValue("idstudies3", idStudies);
                        dr.Close();
                        dr = com.ExecuteReader();
                        if (dr.Read())
                        {
                            idEnrollment = int.Parse(dr["IdEnrollment"].ToString());
                        }

                    }
                    com.CommandText = "select * from Student where IndexNumber =@indexnumber";
                    com.Parameters.AddWithValue("indexnumber", request.IndexNumber);
                    dr.Close();
                    dr = com.ExecuteReader();
                    if (dr.HasRows)
                    {
                        tran.Rollback();
                        return BadRequest("student istnieje");
                    }
                    com.CommandText = "INSERT INTO Student(IndexNumber, FirstName, LastName, BirthDate,IdEnrollment)"
                           + "values(@id,@firstname, @lastname, @birthdate, @idenrollment)";
                    com.Parameters.AddWithValue("id", request.IndexNumber);
                    com.Parameters.AddWithValue("firstname", request.FirstName);
                    com.Parameters.AddWithValue("lastname", request.LastName);
                    com.Parameters.AddWithValue("birthdate", request.Birthdate);
                    com.Parameters.AddWithValue("idenrollment", idEnrollment);
                    dr.Close();
                    dr = com.ExecuteReader();

                    response = new EnrollStudentResponse()
                    {
                        LastName = request.LastName,
                        Semester = 1,
                        StartDate = DateTime.Now
                    };
                    tran.Commit();
                    return Ok(response);
                }
                catch(SqlException exc)
                {
                    tran.Rollback();
                    return BadRequest();
                }

            }


        }

        [HttpPost("promotions")]
        public IActionResult PromoteStudent(PromoteStudentRequest request)
        {
            PromoteStudentResponse response;
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {
                    com.CommandText = "exec PromoteStudents @studies, @semester ";
                    com.Parameters.AddWithValue("studies", request.Studies);
                    com.Parameters.AddWithValue("semester", request.Semester);
                    var dr = com.ExecuteReader();

                    response = new PromoteStudentResponse()
                    {
                        Name = request.Studies,
                        Semester = request.Semester + 1

                    };
                    return Ok(response);
                }catch(SqlException exc)
                {
                    return BadRequest("nie dziala");
                }
                   

            }
        }
    }
}
