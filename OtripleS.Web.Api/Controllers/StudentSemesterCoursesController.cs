﻿// ---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
// ---------------------------------------------------------------

using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using OtripleS.Web.Api.Services.StudentSemesterCourses;
using RESTFulSense.Controllers;

namespace OtripleS.Web.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentSemesterCoursesController : RESTFulController
    {
        private IStudentSemesterCourseService studentSemesterCourseService;

        public StudentSemesterCoursesController(IStudentSemesterCourseService studentSemesterCourseService) =>
            this.studentSemesterCourseService = studentSemesterCourseService;

        [HttpPost]
        public async ValueTask<ActionResult<StudentSemesterCourse>>
        PostStudentSemesterCourseAsync(StudentSemesterCourse studentSemesterCourse)
        {
            try
            {
                StudentSemesterCourse persistedStudentSemesterCourse =
                    await this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(studentSemesterCourse);

                return Ok(persistedStudentSemesterCourse);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
                when (studentSemesterCourseValidationException.InnerException is AlreadyExistsStudentSemesterCourseException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return Conflict(innerMessage);
            }
            catch (StudentSemesterCourseValidationException studentSemesterCourseValidationException)
            {
                string innerMessage = GetInnerMessage(studentSemesterCourseValidationException);

                return BadRequest(innerMessage);
            }
            catch (StudentSemesterCourseDependencyException studentSemesterCourseDependencyException)
            {
                return Problem(studentSemesterCourseDependencyException.Message);
            }
            catch (StudentSemesterCourseServiceException studentSemesterCourseServiceException)
            {
                return Problem(studentSemesterCourseServiceException.Message);
            }
        }

        private static string GetInnerMessage(Exception exception) =>
            exception.InnerException.Message;
    }
}
