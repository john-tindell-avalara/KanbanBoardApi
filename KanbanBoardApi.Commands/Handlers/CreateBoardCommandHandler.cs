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
    public class CreateBoardCommandHandler : IRequestHandler<CreateBoardCommand, Board>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;
        private readonly ISlugService slugService;

        public CreateBoardCommandHandler(IDataContext dataContext, IMappingService mappingService,
            ISlugService slugService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
            this.slugService = slugService;
        }

        public async Task<Board> Handle(CreateBoardCommand request, CancellationToken cancellationToken)
        {
            var board = mappingService.Map<BoardEntity>(request.Board);
            board.Slug = slugService.Slugify(board.Name);

            if (await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == board.Slug, cancellationToken))
            {
                throw new CreateBoardCommandSlugExistsException();
            }

            dataContext.Set<BoardEntity>().Add(board);
            await dataContext.SaveChangesAsync();
            return mappingService.Map<Board>(board);
        }
    }
}