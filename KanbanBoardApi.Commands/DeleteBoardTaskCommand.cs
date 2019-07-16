using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class DeleteBoardTaskCommand : IRequest<int>
    {
        public string BoardSlug { get; set; }

        public int BoardTaskId { get; set; }
    }
}