﻿using Cw5.DTOs.Requests;
using Cw5.DTOs.Responses;
using Cw5.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cw5.Services
{
    public interface IStudentsDbService
    {
        EnrollStudentResponse EnrollStudent(EnrollStudentRequest request);
        PromoteStudentResponse PromoteStudent(PromoteStudentRequest request);

        public Student GetStudent(string indexNumber);


    }
}
