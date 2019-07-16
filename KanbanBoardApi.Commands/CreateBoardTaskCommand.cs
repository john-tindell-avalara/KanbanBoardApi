using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class CreateBoardTaskCommand : IRequest<BoardTask>
    {
        public string BoardSlug { get; set; }

        public BoardTask BoardTask { get; set; }
    }
}