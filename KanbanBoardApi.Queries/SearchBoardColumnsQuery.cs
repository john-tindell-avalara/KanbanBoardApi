using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class SearchBoardColumnsQuery : IRequest<BoardColumnCollection>
    {
        public string BoardSlug { get; set; }
    }
}