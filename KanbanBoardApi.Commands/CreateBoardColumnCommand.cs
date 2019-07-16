using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class CreateBoardColumnCommand : IRequest<BoardColumn>
    {
        public string BoardSlug { get; set; }

        public BoardColumn BoardColumn { get; set; }
    }
}