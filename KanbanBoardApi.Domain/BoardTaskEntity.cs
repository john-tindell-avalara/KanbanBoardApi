namespace KanbanBoardApi.Domain
{
    public class BoardTaskEntity : EntityBase
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public BoardColumnEntity BoardColumnEntity { get; set; }
    }
}