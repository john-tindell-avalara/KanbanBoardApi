
using System.Web.Http.OData;
using KanbanBoardApi.Dto;

namespace KanbanBoardApi.Commands
{
    public class PatchBoardTaskCommand : ICommand
    {
        public string BoardSlug { get; set; }

        public Delta<BoardTask> BoardTask { get; set; }
        public int BoardTaskId { get; set; }
    }
}