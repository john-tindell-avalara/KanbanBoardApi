using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class UpdateBoardTaskCommand : IRequest<BoardTask>
    {
        public string BoardSlug { get; set; }

        public BoardTask BoardTask { get; set; }
    }
}