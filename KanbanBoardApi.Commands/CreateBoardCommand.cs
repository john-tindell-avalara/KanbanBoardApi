using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class CreateBoardCommand : IRequest<Board>
    {
        public Board Board { get; set; }
    }
}