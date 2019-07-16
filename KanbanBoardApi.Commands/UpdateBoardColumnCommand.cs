using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class UpdateBoardColumnCommand : IRequest<BoardColumn>
    {
        public string BoardSlug { get; set; }

        public BoardColumn BoardColumn { get; set; }
        public string BoardColumnSlug { get; set; }
    }
}