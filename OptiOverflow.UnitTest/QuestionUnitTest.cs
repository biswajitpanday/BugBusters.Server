using System.Net;
using BugBusters.Server.Core.Dtos;
using BugBusters.Server.Core.Entities;
using BugBusters.Server.Core.Interfaces.Common;
using BugBusters.Server.Core.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Moq;
using Newtonsoft.Json;
using OptiOverflow.Api.Controllers;
using OptiOverflow.Core.Entities;

namespace BugBusters.Server.UnitTest;

[TestFixture]
public class QuestionUnitTest
{
    private readonly Mock<IQuestionService> _questionServiceMock;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock;
    private readonly QuestionController _questionControllerMock;

    public QuestionUnitTest()
    {
        _questionServiceMock = new Mock<IQuestionService>();
        _currentUserServiceMock = new Mock<ICurrentUserService>();
        _questionControllerMock = new QuestionController(_questionServiceMock.Object, _currentUserServiceMock.Object);

    }

    [Test]
    public async Task GetQuestions_ReturnsOk_WhenQuestionsFound()
    {
        // Arrange
        var questions = new List<Question>
        {
            new Question { Id = Guid.NewGuid(), Title = "Question 1", CreatedBy = new ApplicationUser {Id = Guid.NewGuid(), Email = "admin001@example.com" }},
            new Question { Id = Guid.NewGuid(), Title = "Question 2", CreatedBy = new ApplicationUser {Id = Guid.NewGuid(), Email = "admin001@example.com" } }
        };

        _questionServiceMock.Setup(x => x.GetAll(It.IsAny<PagedRequest>()))!.ReturnsAsync(new PagedResponse<List<QuestionResponseDto>>
        {
            Items = questions.Select(x => new QuestionResponseDto { Id = x.Id, Title = x.Title, Body = x.Body, CreatedBy = new ProfileResponseDto { Id = x.CreatedById, Email = x.CreatedBy.Email } }).ToList(),
            ItemCount = questions.Count
        });

        // Act
        var response = await _questionControllerMock.Get(new PagedRequest { Page = 1, PageSize = 10 });

        // Assert
        Assert.NotNull(response);
        Assert.That((response as OkObjectResult)!.StatusCode, Is.EqualTo(200));

        // ~~~ToDo: Finish this Assert
        // Assert.That(content, Is.EqualTo(JsonConvert.SerializeObject(new PagedResponse<List<QuestionResponseDto>>
        // {
        //     Items = questions.Select(x => new QuestionResponseDto { Id = x.Id, Title = x.Title }).ToList(),
        //     ItemCount = itemCount
        // })));
    }


    // ~~~ToDo: Fix the below test (StatusCode is not present)
    [Test]
    public async Task GetAllQuestions_ReturnsNotFound_WhenNoQuestionsFound()
    {
        // Arrange
        _questionServiceMock.Setup(x => x.GetAll(It.IsAny<PagedRequest>()))!.ReturnsAsync((PagedResponse<List<QuestionResponseDto>>?)null);

        // Act
        var response = await _questionControllerMock.Get(new PagedRequest { Page = 1, PageSize = 10 });

        // Assert
        var statusCode = (response as ObjectResult)!.StatusCode;
        Assert.That(statusCode, Is.EqualTo(404));
    }

}