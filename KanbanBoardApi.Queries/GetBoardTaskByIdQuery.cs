using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class GetBoardTaskByIdQuery : IRequest<BoardTask>
    {
        public string BoardSlug { get; set; }

        public int TaskId { get; set; }
    }
}