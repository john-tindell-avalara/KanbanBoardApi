using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Commands
{
    public class DeleteBoardColumnCommand : IRequest<string>
    {
        public string BoardSlug { get; set; }

        public string BoardColumnSlug { get; set; }
    }
}