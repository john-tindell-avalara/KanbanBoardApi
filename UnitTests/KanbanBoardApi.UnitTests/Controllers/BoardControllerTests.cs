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
    public class BoardControllerTests
    {
        private BoardController controller;
        private Mock<IHyperMediaFactory> mockHyperMediaFactory;
        private Mock<IMediator> mockMediator;

        private void SetupController()
        {
            mockHyperMediaFactory = new Mock<IHyperMediaFactory>();
            mockMediator = new Mock<IMediator>();
            controller = new BoardController(
                mockHyperMediaFactory.Object,
                mockMediator.Object);
        }

        public class Post : BoardControllerTests
        {
            [Fact]
            public async void GivenABoardWhenDataIsValidThenCreatedOkResultReturned()
            {
                // Arrange
                SetupController();

                var board = new Board
                {
                    Name = "new board"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Board());
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                var createdNegotiatedContentResult =
                    await controller.Post(board) as CreatedNegotiatedContentResult<Board>;

                // Assert
                Assert.NotNull(createdNegotiatedContentResult);
            }

            [Fact]
            public async void GivenABoardWhenDataIsValidThenHyperMediaSet()
            {
                // Arrange
                SetupController();

                var board = new Board
                {
                    Name = "new board"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Board());
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                var createdNegotiatedContentResult =
                    await controller.Post(board) as CreatedNegotiatedContentResult<Board>;

                // Assert
                Assert.NotNull(createdNegotiatedContentResult);
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<Board>()), Times.Once);
            }

            [Fact]
            public async void GivenABoardWhenDataIsValidThenCreateBoardCommandCalled()
            {
                // Arrange
                SetupController();

                var board = new Board
                {
                    Name = "new board"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Board());
                mockHyperMediaFactory.Setup(x => x.GetLink(It.IsAny<IHyperMediaItem>(), It.IsAny<string>()))
                    .Returns("http://fake-url/");

                // Act
                var createdNegotiatedContentResult =
                    await controller.Post(board) as CreatedNegotiatedContentResult<Board>;

                // Assert
                Assert.NotNull(createdNegotiatedContentResult);
                mockMediator.Verify(
                    x => x.Send(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>()),
                    Times.Once);
            }


            [Fact]
            public async void GivenABoardWhenBoardSlugAlreadyExistsThenReturnReturnsConflict()
            {
                // Arrange
                SetupController();

                var board = new Board
                {
                    Name = "new board"
                };

                mockMediator.Setup(
                    x => x.Send(It.IsAny<CreateBoardCommand>(), It.IsAny<CancellationToken>()))
                    .Throws<CreateBoardCommandSlugExistsException>();

                // Act
                var conflictResult = await controller.Post(board) as ConflictResult;

                // Act
                Assert.NotNull(conflictResult);
            }

            [Fact]
            public async void GivenABoardWhenDataIsNotValidThenInvalidModelStateResultReturned()
            {
                // Arrange
                SetupController();

                var board = new Board();

                // force a validation error
                controller.ModelState.AddModelError("error", "error");

                // Act
                var invalidModelStateResult = await controller.Post(board) as InvalidModelStateResult;

                // Assert
                Assert.NotNull(invalidModelStateResult);
            }
        }

        public class Get : BoardControllerTests
        {
            [Fact]
            public async void GivenABoardSlugWhenBoardExistsThenBoardIsReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "test-slug";
                mockMediator.Setup(
                    x => x.Send(It.IsAny<GetBoardBySlugQuery>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Board());

                // Act
                var okNegotiatedContentResult = await controller.Get(boardSlug) as OkNegotiatedContentResult<Board>;

                // Assert
                Assert.NotNull(okNegotiatedContentResult);
                Assert.NotNull(okNegotiatedContentResult.Content);
            }

            [Fact]
            public async void GivenASlugWhenBoardExistsThenGetBoardBySlugQueryCalled()
            {
                // Arrange
                SetupController();
                const string boardSlug = "test-slug";

                // Act
                await controller.Get(boardSlug);

                // Assert
                mockMediator.Verify(
                    x =>
                        x.Send(
                            It.Is<GetBoardBySlugQuery>(y => y.BoardSlug == boardSlug),
                            It.IsAny<CancellationToken>()),
                    Times.Once);
            }

            [Fact]
            public async void GiveASlugWhenBoardExistsThenHypermediaSet()
            {
                // Arrange
                SetupController();
                const string boardSlug = "test-slug";
                mockMediator.Setup(
                    x => x.Send(It.IsAny<GetBoardBySlugQuery>(),
                        It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new Board());

                // Act
                await controller.Get(boardSlug);

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<object>()), Times.Once);
            }

            [Fact]
            public async void GivenASlugWhenBoardDoesNotExistsThenNotFoundReturned()
            {
                // Arrange
                SetupController();
                const string boardSlug = "test-slug";

                // Act
                var notFoundResult = await controller.Get(boardSlug) as NotFoundResult;

                // Assert
                Assert.NotNull(notFoundResult);
            }
        }

        public class Search : BoardControllerTests
        {
            [Fact]
            public async void GiveDefaultValuesWhenBoardsExistABoardCollectionIsReturned()
            {
                // Arrange
                SetupController();

                // Act
                var okNegotiatedContentResult = await controller.Search() as OkNegotiatedContentResult<BoardCollection>;

                // Assert
                Assert.NotNull(okNegotiatedContentResult);
            }

            [Fact]
            public async void GivenDefaultvaluesWhenBoardsExistsThenSearchBoardsQueryCsalled()
            {
                // Arrange
                SetupController();
                mockMediator.Setup(
                    x => x.Send(It.IsAny<SearchBoardsQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardCollection());

                // Act
                await controller.Search();

                // Assert
                mockMediator.Verify(
                    x => x.Send(It.IsAny<SearchBoardsQuery>(), It.IsAny<CancellationToken>()), Times.Once);
            }

            [Fact]
            public async void GivenDefaultvaluesWhenBoardsExistsThenHypermediaSet()
            {
                // Arrange
                SetupController();
                mockMediator.Setup(
                    x => x.Send(It.IsAny<SearchBoardsQuery>(), It.IsAny<CancellationToken>()))
                    .ReturnsAsync(new BoardCollection());

                // Act
                await controller.Search();

                // Assert
                mockHyperMediaFactory.Verify(x => x.Apply(It.IsAny<object>()), Times.Once);
            }
        }
    }
}