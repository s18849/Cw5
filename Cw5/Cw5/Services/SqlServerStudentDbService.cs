using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public class SqlServerStudentDbService : IStudentsDbService
    {
        private const string ConString = "Data Source=db-mssql;Initial Catalog=s18849;Integrated Security=True;MultipleActiveResultSets=True;";

        public EnrollStudentResponse EnrollStudent(EnrollStudentRequest request)
        {
            throw new NotImplementedException();
        }

        public Student GetStudent(string index)
        {
            Student student = null;
            using (var con = new SqlConnection(ConString))
            using (var com = new SqlCommand())
            {

                com.Connection = con;
                con.Open();
                var tran = con.BeginTransaction();
                com.Transaction = tran;
                try
                {
                    com.CommandText = "select * from student where IndexNumber = @index";
                    com.Parameters.AddWithValue("index", index);
                  
                    var dr = com.ExecuteReader();
                    if (!dr.Read())
                    {
                        dr.Close();
                        return null;

                    }
                    else
                    {
                        student = new Student
                        {
                            IndexNumber = dr["IndexNumber"].ToString(),
                            FirstName = dr["FirstName"].ToString(),
                            LastName = dr["LastName"].ToString(),
                            BirthDate = DateTime.Parse(dr["BirthDate"].ToString()),
                            IdEnrollment = int.Parse(dr["IdEnrollment"].ToString())
                        };
                    }
                    return student;
                }
                catch (SqlException exc)
                {
                    return null;
                }


            }
        }

      

        public PromoteStudentResponse PromoteStudent(PromoteStudentRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
