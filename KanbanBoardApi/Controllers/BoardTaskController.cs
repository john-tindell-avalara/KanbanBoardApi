﻿using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using System.Web.Http.OData;
using KanbanBoardApi.Commands;
using KanbanBoardApi.Dto;
using KanbanBoardApi.Exceptions;
using KanbanBoardApi.HyperMedia;
using KanbanBoardApi.Queries;
using MediatR;

namespace KanbanBoardApi.Controllers
{
    [RoutePrefix("boards")]
    public class BoardTaskController : ApiController
    {
        private readonly IHyperMediaFactory hyperMediaFactory;
        private readonly IMediator mediator;

        public BoardTaskController(IHyperMediaFactory hyperMediaFactory,
            IMediator mediator)
        {
            this.hyperMediaFactory = hyperMediaFactory;
            this.mediator = mediator;
        }

        [HttpPost]
        [Route("{boardSlug}/tasks", Name = "BoardTaskPost")]
        [ResponseType(typeof(BoardTask))]
        public async Task<IHttpActionResult> Post(string boardSlug, BoardTask boardTask)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                var result = await mediator.Send(new CreateBoardTaskCommand
                {
                    BoardSlug = boardSlug,
                    BoardTask = boardTask
                });

                hyperMediaFactory.Apply(result);

                return Created(hyperMediaFactory.GetLink(result, Link.SELF), result);
            }
            catch (BoardColumnNotFoundException)
            {
                return BadRequest("Board Column Not Found");
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPut]
        [Route("{boardSlug}/tasks/{taskId:int}", Name = "BoardTaskPut")]
        [ResponseType(typeof(BoardTask))]
        public async Task<IHttpActionResult> Put(string boardSlug, int taskId, BoardTask boardTask)
        {
            try
            {
                var result = await mediator.Send(new UpdateBoardTaskCommand
                {
                    BoardSlug = boardSlug,
                    BoardTask = boardTask
                });

                hyperMediaFactory.Apply(result);

                return Ok(result);
            }
            catch (BoardTaskNotFoundException)
            {
                return NotFound();
            }
            catch (BoardColumnNotFoundException)
            {
                return BadRequest("Board Column Not Found");
            }
        }

        [HttpPatch]
        [Route("{boardSlug}/tasks/{taskId:int}", Name = "BoardTaskPatch")]
        [ResponseType(typeof(BoardTask))]
        public async Task<IHttpActionResult> Patch(string boardSlug, int taskId, Delta<BoardTask> boardTask)
        {
            try
            {
                var result = await mediator.Send(new PatchBoardTaskCommand
                {
                    BoardSlug = boardSlug,
                    BoardTaskId = taskId,
                    BoardTask = boardTask
                });

                hyperMediaFactory.Apply(result);

                return Ok(result);
            }
            catch (BoardTaskNotFoundException)
            {
                return NotFound();
            }
            catch (BoardColumnNotFoundException)
            {
                return BadRequest("Board Column Not Found");
            }
        }


        [HttpGet]
        [Route("{boardSlug}/tasks/{taskId:int}", Name = "BoardTaskGet")]
        [ResponseType(typeof(BoardTask))]
        public async Task<IHttpActionResult> Get(string boardSlug, int taskId)
        {
            var boardTask = await mediator.Send(new GetBoardTaskByIdQuery
            {
                BoardSlug = boardSlug,
                TaskId = taskId
            });

            if (boardTask == null)
            {
                return NotFound();
            }

            hyperMediaFactory.Apply(boardTask);

            return Ok(boardTask);
        }

        [HttpGet]
        [Route("{boardSlug}/tasks", Name = "BoardTasksSearch")]
        [Route("{boardSlug}/columns/{boardColumnSlug}/tasks", Name = "BoardTaskByBoardColumnSearch")]
        [ResponseType(typeof(BoardTask))]
        public async Task<IHttpActionResult> Search(string boardSlug, string boardColumnSlug = "")
        {
            try
            {
                var result = await mediator.Send(new SearchBoardTasksQuery
                {
                    BoardSlug = boardSlug,
                    BoardColumnSlug = boardColumnSlug
                });

                hyperMediaFactory.Apply(result);

                return Ok(result);
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpDelete]
        [Route("{boardSlug}/tasks/{taskId:int}", Name = "BoardTaskDelete")]
        public async Task<IHttpActionResult> Delete(string boardSlug, int taskId)
        {
            try
            {
                await mediator.Send(new DeleteBoardTaskCommand
                {
                    BoardSlug = boardSlug,
                    BoardTaskId = taskId
                });

                return Ok();
            }
            catch (BoardNotFoundException)
            {
                return NotFound();
            }
            catch (BoardTaskNotFoundException)
            {
                return NotFound();
            }
        }
    }
}