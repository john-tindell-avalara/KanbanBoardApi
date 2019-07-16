using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using KanbanBoardApi.Commands;
using KanbanBoardApi.Dto;
using KanbanBoardApi.Exceptions;
using KanbanBoardApi.HyperMedia;
using KanbanBoardApi.Queries;
using MediatR;

namespace KanbanBoardApi.Controllers
{
    [RoutePrefix("boards")]
    public class BoardColumnController : ApiController
    {
        private readonly IHyperMediaFactory hyperMediaFactory;
        private readonly IMediator mediator;

        public BoardColumnController(IHyperMediaFactory hyperMediaFactory,
            IMediator mediator)
        {
            this.hyperMediaFactory = hyperMediaFactory;
            this.mediator = mediator;
        }

        [HttpPut]
        [Route("{boardSlug}/columns/{boardColumnSlug}", Name = "BoardColumnPut")]
        public async Task<IHttpActionResult> Put(string boardSlug, string boardColumnSlug, BoardColumn boardColumn)
        {
            try
            {
                var result = await mediator.Send(new UpdateBoardColumnCommand
                {
                    BoardSlug = boardSlug,
                    BoardColumnSlug = boardColumnSlug,
                    BoardColumn = boardColumn
                });

                hyperMediaFactory.Apply(result);

                return Ok(result);
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
            catch (BoardColumnNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{boardSlug}/columns/{boardColumnSlug}", Name = "BoardColumnDelete")]
        public async Task<IHttpActionResult> Delete(string boardSlug, string boardColumnSlug)
        {
            try
            {
                await mediator.Send(new DeleteBoardColumnCommand
                {
                    BoardSlug = boardSlug,
                    BoardColumnSlug = boardColumnSlug
                });

                return Ok();
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
            catch (BoardColumnNotFoundException)
            {
                return NotFound();
            }
            catch (BoardColumnNotEmptyException)
            {
                return BadRequest("Board Tasks Are Still Assocaited To This Board Column");
            }
        }

        [HttpGet]
        [Route("{boardSlug}/columns/{boardColumnSlug}", Name = "BoardColumnGet")]
        [ResponseType(typeof (BoardColumn))]
        public async Task<IHttpActionResult> Get(string boardSlug, string boardColumnSlug)
        {
            var boardColumn =
                await mediator.Send(new GetBoardColumnBySlugQuery
                {
                    BoardSlug = boardSlug,
                    BoardColumnSlug = boardColumnSlug
                });

            if (boardColumn == null)
            {
                return NotFound();
            }

            hyperMediaFactory.Apply(boardColumn);

            return Ok(boardColumn);
        }

        [HttpGet]
        [Route("{boardSlug}/columns", Name = "BoardColumnSearch")]
        [ResponseType(typeof(BoardColumnCollection))]
        public async Task<IHttpActionResult> Search(string boardSlug)
        {
            try
            {
                var boardColumnCollection = await mediator.Send(
                    new SearchBoardColumnsQuery
                    {
                        BoardSlug = boardSlug
                    });

                hyperMediaFactory.Apply(boardColumnCollection);

                return Ok(boardColumnCollection);
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        [Route("{boardSlug}/columns")]
        [ResponseType(typeof (BoardColumn))]
        public async Task<IHttpActionResult> Post(string boardSlug, BoardColumn boardColumn)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var result =
                    await
                        mediator.Send(new CreateBoardColumnCommand
                        {
                            BoardSlug = boardSlug,
                            BoardColumn = boardColumn
                        });

                if (result == null)
                {
                    return NotFound();
                }

                hyperMediaFactory.Apply(result);

                return Created(hyperMediaFactory.GetLink(result, Link.SELF), result);
            }
            catch (CreateBoardColumnCommandSlugExistsException)
            {
                return Conflict();
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
        }
    }
}