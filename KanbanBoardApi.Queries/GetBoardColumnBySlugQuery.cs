using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class GetBoardColumnBySlugQuery : IRequest<BoardColumn>
    {
        public string BoardSlug { get; set; }

        public string BoardColumnSlug { get; set; }
    }
}