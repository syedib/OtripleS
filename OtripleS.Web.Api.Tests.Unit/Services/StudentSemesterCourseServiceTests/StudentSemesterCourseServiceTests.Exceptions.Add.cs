﻿//---------------------------------------------------------------
// Copyright (c) Coalition of the Good-Hearted Engineers
// FREE TO USE AS LONG AS SOFTWARE FUNDS ARE DONATED TO THE POOR
//----------------------------------------------------------------

using Microsoft.EntityFrameworkCore;
using Moq;
using OtripleS.Web.Api.Models.StudentSemesterCourses;
using OtripleS.Web.Api.Models.StudentSemesterCourses.Exceptions;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace OtripleS.Web.Api.Tests.Unit.Services.StudentSemesterCourseServiceTests
{
    public partial class StudentSemesterCourseServiceTests
    {
        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenSqlExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = inputStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.UpdatedDate = inputStudentSemesterCourse.CreatedDate;
            var sqlException = GetSqlException();

            var expectedStudentSemesterCourseDependencyException =
                new StudentSemesterCourseDependencyException(sqlException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ThrowsAsync(sqlException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(() =>
                createStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogCritical(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowDependencyExceptionOnCreateWhenDbExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = inputStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.UpdatedDate = inputStudentSemesterCourse.CreatedDate;
            var databaseUpdateException = new DbUpdateException();

            var expectedStudentSemesterCourseDependencyException =
                new StudentSemesterCourseDependencyException(databaseUpdateException);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ThrowsAsync(databaseUpdateException);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseDependencyException>(() =>
                createStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseDependencyException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }

        [Fact]
        public async Task ShouldThrowServiceExceptionOnCreateWhenExceptionOccursAndLogItAsync()
        {
            // given
            DateTimeOffset dateTime = GetRandomDateTime();
            StudentSemesterCourse randomStudentSemesterCourse = CreateRandomStudentSemesterCourse(dateTime);
            StudentSemesterCourse inputStudentSemesterCourse = randomStudentSemesterCourse;
            inputStudentSemesterCourse.UpdatedBy = inputStudentSemesterCourse.CreatedBy;
            inputStudentSemesterCourse.UpdatedDate = inputStudentSemesterCourse.CreatedDate;
            var exception = new Exception();

            var expectedStudentSemesterCourseServiceException =
                new StudentSemesterCourseServiceException(exception);

            this.dateTimeBrokerMock.Setup(broker =>
                broker.GetCurrentDateTime())
                    .Returns(dateTime);

            this.storageBrokerMock.Setup(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse))
                    .ThrowsAsync(exception);

            // when
            ValueTask<StudentSemesterCourse> createStudentSemesterCourseTask =
                this.studentSemesterCourseService.CreateStudentSemesterCourseAsync(inputStudentSemesterCourse);

            // then
            await Assert.ThrowsAsync<StudentSemesterCourseServiceException>(() =>
                createStudentSemesterCourseTask.AsTask());

            this.loggingBrokerMock.Verify(broker =>
                broker.LogError(It.Is(SameExceptionAs(expectedStudentSemesterCourseServiceException))),
                    Times.Once);

            this.storageBrokerMock.Verify(broker =>
                broker.InsertStudentSemesterCourseAsync(inputStudentSemesterCourse),
                    Times.Once);

            this.dateTimeBrokerMock.Verify(broker =>
                broker.GetCurrentDateTime(),
                    Times.Once);

            this.dateTimeBrokerMock.VerifyNoOtherCalls();
            this.loggingBrokerMock.VerifyNoOtherCalls();
            this.storageBrokerMock.VerifyNoOtherCalls();
        }
    }
}
