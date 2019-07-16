using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Commands.Services;
using KanbanBoardApi.Domain;
using KanbanBoardApi.Dto;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Exceptions;
using KanbanBoardApi.Mapping;
using MediatR;

namespace KanbanBoardApi.Commands.Handlers
{
    public class CreateBoardColumnCommandHandler : IRequestHandler<CreateBoardColumnCommand, BoardColumn>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;
        private readonly ISlugService slugService;

        public CreateBoardColumnCommandHandler(IDataContext dataContext, IMappingService mappingService,
            ISlugService slugService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
            this.slugService = slugService;
        }

        public async Task<BoardColumn> Handle(CreateBoardColumnCommand request, CancellationToken cancellationToken)
        {
            var boardColumn = mappingService.Map<BoardColumnEntity>(request.BoardColumn);
            boardColumn.Slug = slugService.Slugify(boardColumn.Name);

            if (await dataContext.Set<BoardColumnEntity>().AnyAsync(x => x.Slug == boardColumn.Slug, cancellationToken))
            {
                throw new CreateBoardColumnCommandSlugExistsException();
            }

            var board = dataContext.Set<BoardEntity>().FirstOrDefault(x => x.Slug == request.BoardSlug);
            if (board == null)
            {
                throw new BoardNotFoundException();
            }

            dataContext.Set<BoardColumnEntity>().Add(boardColumn);
            board.Columns.Add(boardColumn);

            await dataContext.SaveChangesAsync();
            return mappingService.Map<BoardColumn>(boardColumn);
        }
    }
}