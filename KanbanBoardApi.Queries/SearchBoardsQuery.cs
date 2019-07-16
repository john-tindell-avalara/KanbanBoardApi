using KanbanBoardApi.Dto;
using MediatR;

namespace KanbanBoardApi.Queries
{
    public class SearchBoardsQuery : IRequest<BoardCollection>
    {
    }
}