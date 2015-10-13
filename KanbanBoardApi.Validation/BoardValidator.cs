using FluentValidation;
using KanbanBoardApi.Dto;

namespace KanbanBoardApi.Validation
{
    public class BoardValidator : AbstractValidator<Board>
    {
        public BoardValidator()
        {
            RuleFor(x => x.Name).NotEmpty().Length(1, 100);
            RuleFor(x => x.Slug).Length(1, 100);
        }
    }
}