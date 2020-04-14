using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cw5.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Cw5.Controllers
{
    [Route("api/students")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetStudents()
        {
            var list = new List<Student>();
            list.Add(new Student { IndexNumber = "s11115", FirstName = "Paweł", LastName = "Kowalski" });
            list.Add(new Student { IndexNumber = "s23235", FirstName = "Adam", LastName = "Malinowski" });
            
            return Ok();
        }
        [HttpGet("{id}")]
        public IActionResult GetStudent(string index)
        {
           
            return Ok(new Student { IndexNumber = "s44445", FirstName = "Jan", LastName = "Szabla" });
        }

    }
}