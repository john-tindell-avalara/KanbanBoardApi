
using System.Web.Http.OData;
using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class PatchBoardTaskCommand : IRequest<BoardTask>
    {
        public string BoardSlug { get; set; }

        public Delta<BoardTask> BoardTask { get; set; }
        public int BoardTaskId { get; set; }
    }
}