﻿using System;
using System.Threading;
using System.Web.Http.Results;
using KanbanBoardApi.Commands;
using KanbanBoardApi.Controllers;
using KanbanBoardApi.Dto;
using KanbanBoardApi.Exceptions;
using KanbanBoardApi.HyperMedia;
using KanbanBoardApi.Queries;
using MediatR;
using Moq;
using Xunit;

namespace KanbanBoardApi.UnitTests.Controllers
{
    public class BoardTaskControllerTests
    {
        private BoardTaskController controller;
        private Mock<IHyperMediaFactory> mockHyperMediaFactory;
        private Mock<IMediator> mockMediator;

        private void SetupController()
        {
            mockHyperMediaFactory = new Mock<IHyperMediaFactory>();
            mockMediator = new Mock<IMediator>();
            controller = new BoardTaskController(
                mockHyperMediaFactory.Object,
                mockMediator.Object);
        }

        public class Post : BoardTaskControllerTests
        {
            [Fact]
            public async void GivenABoardSlugAndTaskWhenBoardAndColumnExistsThenReturnOkResult()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTask());
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                var createdNegotiatedContentResult =
                    await controller.Post(boardSlug, boardTask) as CreatedNegotiatedContentResult<BoardTask>;

                // Assert
                Assert.NotNull(createdNegotiatedContentResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskWhenBoardExistsThenCreateBoardTaskCommandCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                await controller.Post(boardSlug, boardTask);

                // Assert
                mockMediator.Verify(
                    x =>
                        x.Send(
                            It.Is<CreateBoardTaskCommand>(y => y.BoardTask == boardTask && y.BoardSlug == boardSlug),
                            It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async void GivenABoardSlugSlugAndTaskWhenBoardExistsThenHyperMediaSet()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                mockMediator.Setup(
                        x => x.Send(It.IsAny<CreateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTask());
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                await controller.Post(boardSlug, boardTask);

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<BoardTask>()), Times.Once);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskWhenBoardColumnIsInvalidThenReturnInvalidModelState()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                controller.ModelState.AddModelError("error", "error");

                // Act
                var invalidModelStateResult = await controller.Post(boardSlug, boardTask) as InvalidModelStateResult;

                // Assert
                Assert.NotNull(invalidModelStateResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskWhenBoardDoesNotExistThenNotFound()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardNotFoundException>();

                // Act
                var notFoundResult = await controller.Post(boardSlug, boardTask) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskWhenBoardColumnDoesNotExistThenBadRequestReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                var boardTask = new BoardTask();
                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardColumnNotFoundException>();

                // Act
                var badRequestErrorMessageResult =
                    await controller.Post(boardSlug, boardTask) as BadRequestErrorMessageResult;

                // Assert
                Assert.NotNull(badRequestErrorMessageResult);
                Assert.Equal("Board Column Not Found", badRequestErrorMessageResult.Message);
            }
        }

        public class Get : BoardTaskControllerTests
        {
            [Fact]
            public async void GivenAnIdWhenBoardExistsThenBoardIsReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                mockMediator.Setup(
                    x => x.Send(It.IsAny<GetBoardTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTask());

                // Act
                var okNegotiatedContentResult =
                    await controller.Get(boardSlug, taskId) as OkNegotiatedContentResult<BoardTask>;

                // Assert
                Assert.NotNull(okNegotiatedContentResult);
                Assert.NotNull(okNegotiatedContentResult.Content);
            }


            [Fact]
            public async void GivenAnIdWhenBoardExistsThenGetBoardBySlugQueryCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                // Act
                await controller.Get(boardSlug, taskId);

                // Assert
                mockMediator.Verify(
                    x =>
                        x.Send(
                            It.Is<GetBoardTaskByIdQuery>(y => y.TaskId == taskId && y.BoardSlug == boardSlug),
                            It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async void GiveAnIdWhenBoardExistsThenHypermediaSet()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                mockMediator.Setup(
                    x => x.Send(It.IsAny<GetBoardTaskByIdQuery>(),
                    It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTask());

                // Act
                await controller.Get(boardSlug, taskId);

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<object>()), Times.Once);
            }

            [Fact]
            public async void GivenAnIdWhenBoardDoesNotExistsThenNotFoundReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                // Act
                var notFoundResult = await controller.Get(boardSlug, taskId) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenAnIdWhenBoardTaskDoesNotExistsThenNotFoundReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 2;
                mockMediator.Setup(
                    x => x.Send(It.IsAny<GetBoardTaskByIdQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(null, default(TimeSpan));

                // Act
                var notFoundResult = await controller.Get(boardSlug, taskId) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }
        }

        public class Search : BoardTaskControllerTests
        {
            [Fact]
            public async void GiveDefaultValuesWhenBoardTasksExistABoardTaskCollectionIsReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";

                // Act
                var okNegotiatedContentResult =
                    await controller.Search(boardSlug) as OkNegotiatedContentResult<BoardTaskCollection>;

                // Assert
                Assert.NotNull(okNegotiatedContentResult);
            }

            [Fact]
            public async void GiveDefaultValuesWhenBoardDoesNotExistABoardTaskCollectionIsReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                mockMediator.Setup(
                    x => x.Send(It.IsAny<SearchBoardTasksQuery>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardNotFoundException>();

                // Act
                var notFoundResult = await controller.Search(boardSlug) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenDefaultvaluesWhenBoardTaskssExistsThenSearchBoardTasksQueryCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const string boardColumnSlug = "board-column-name";

                mockMediator.Setup(
                    x => x.Send(It.IsAny<SearchBoardTasksQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTaskCollection());

                // Act
                await controller.Search(boardSlug, boardColumnSlug);

                // Assert
                mockMediator.Verify(
                    x =>
                        x.Send(
                            It.Is<SearchBoardTasksQuery>(
                                y => y.BoardSlug == boardSlug && y.BoardColumnSlug == boardColumnSlug),
                            It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async void GivenDefaultvaluesWhenBoardTaskssExistsThenHypermediaSet()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";

                mockMediator.Setup(
                    x => x.Send(It.IsAny<SearchBoardTasksQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardTaskCollection());

                // Act
                await controller.Search(boardSlug);

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<object>()), Times.Once);
            }
        }

        public class Put : BoardTaskControllerTests
        {
            [Fact]
            public async void GivenABoardSlugTaskIdAndTaskWhenAllAreValidThenOkResultReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                var boardTask = new BoardTask
                {
                    Id = taskId,
                    Name = "Updated Name"
                };

                // Act
                var okNegotiatedContentResult =
                    await controller.Put(boardSlug, taskId, boardTask) as OkNegotiatedContentResult<BoardTask>;

                // Assert
                Assert.NotNull(okNegotiatedContentResult);
            }

            [Fact]
            public async void GivenABoardSlugTaskIdAndTaskWhenAllAreValidThenUpdateBoardTaskCommandCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                var boardTask = new BoardTask
                {
                    Id = taskId,
                    Name = "Updated Name"
                };

                // Act
                await controller.Put(boardSlug, taskId, boardTask);

                // Assert
                mockMediator.Verify(
                    x => x.Send(It.Is<UpdateBoardTaskCommand>(
                        y => y.BoardSlug == boardSlug && y.BoardTask == boardTask
                        ), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async void GivenABoardSlugTaskIdAndTaskWhenAllAreValidThenHypermediaSet()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                var boardTask = new BoardTask
                {
                    Id = taskId,
                    Name = "Updated Name"
                };

                // Act
                await controller.Put(boardSlug, taskId, boardTask);

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<object>()), Times.Once);
            }

            [Fact]
            public async void GivenABoardSlugTaskIdAndTaskWhenDoesNotExistThenNotFoundResultReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                var boardTask = new BoardTask
                {
                    Id = taskId,
                    Name = "Updated Name"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<UpdateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardTaskNotFoundException>();

                // Act
                var notFoundResult = await controller.Put(boardSlug, taskId, boardTask) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenABoardSlugTaskIdAndTaskWhenBoardColumnDoesNotExistThenBadRequestReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                var boardTask = new BoardTask
                {
                    Id = taskId,
                    Name = "Updated Name"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<UpdateBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardColumnNotFoundException>();

                // Act
                var badRequestErrorMessageResult =
                    await controller.Put(boardSlug, taskId, boardTask) as BadRequestErrorMessageResult;

                // Assert
                Assert.NotNull(badRequestErrorMessageResult);
                Assert.Equal("Board Column Not Found", badRequestErrorMessageResult.Message);
            }
        }

        public class Delete : BoardTaskControllerTests
        {
            [Fact]
            public async void GivenABoardSlugAndTaskIdWhenBoardDoesNotExistsThenNotFoundReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                mockMediator.Setup(
                    x => x.Send(It.IsAny<DeleteBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardNotFoundException>();

                // Act
                var notFoundResult = await controller.Delete(boardSlug, taskId) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskIdWhenBoardTaskDoesNotExistsThenNotFoundReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;
                mockMediator.Setup(
                    x => x.Send(It.IsAny<DeleteBoardTaskCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<BoardTaskNotFoundException>();

                // Act
                var notFoundResult = await controller.Delete(boardSlug, taskId) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskIdWhenBoardSlugAndTaskIdFoundThenOkReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                // Act
                var okResult = await controller.Delete(boardSlug, taskId) as OkResult;

                // Assert
                Assert.NotNull(okResult);
            }

            [Fact]
            public async void GivenABoardSlugAndTaskIdWhenBoardSlugAndTaskIdFoundThenDeleteBoardTaskCommandCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "board-name";
                const int taskId = 1;

                // Act
                await controller.Delete(boardSlug, taskId);

                // Assert
                mockMediator.Verify(x => x.Send(
                    It.Is<DeleteBoardTaskCommand>(y => y.BoardSlug == boardSlug && y.BoardTaskId == taskId), It.IsAny<CancellationToken>()), Times.Once);
            }
        }
    }
}