using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class GetBoardBySlugQuery : IRequest<Board>
    {
        public string BoardSlug { get; set; }
    }
}