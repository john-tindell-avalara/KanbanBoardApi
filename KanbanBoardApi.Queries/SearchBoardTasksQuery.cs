using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class SearchBoardTasksQuery : IRequest<BoardTaskCollection>
    {
        public string BoardSlug { get; set; }

        public string BoardColumnSlug { get; set; }
    }
}