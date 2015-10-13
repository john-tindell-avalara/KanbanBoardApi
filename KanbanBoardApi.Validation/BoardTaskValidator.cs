using FluentValidation;
using KanbanBoardApi.Dto;

namespace KanbanBoardApi.Validation
{
    public class BoardTaskValidator : AbstractValidator<BoardTask>
    {
        public BoardTaskValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 100);
        }
    }
}