using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FluentValidation.Attributes;
using KanbanBoardApi.Dto.Validators;

namespace KanbanBoardApi.Dto
{
    public class Board : IHyperMediaItem
    {
        public string Slug { get; set; }

        public string Name { get; set; }

        public IList<Link> Links { get; set; }
    }
}