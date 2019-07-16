using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Domain;
using KanbanBoardApi.Dto;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Exceptions;
using KanbanBoardApi.Mapping;
using MediatR;

namespace KanbanBoardApi.Commands.Handlers
{
    public class UpdateBoardColumnCommandHandler  : IRequestHandler<UpdateBoardColumnCommand, BoardColumn>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public UpdateBoardColumnCommandHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardColumn> Handle(UpdateBoardColumnCommand request, CancellationToken cancellationToken)
        {
            if (!await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardNotFoundException();
            }

            var boardColumnEntity = await dataContext.Set<BoardColumnEntity>()
                .FirstOrDefaultAsync(x => x.Slug == request.BoardColumnSlug && x.BoardEntity.Slug == request.BoardSlug, cancellationToken);

            if (boardColumnEntity == null)
            {
                throw new BoardColumnNotFoundException();
            }

            mappingService.Map(request.BoardColumn, boardColumnEntity);


            dataContext.SetModified(boardColumnEntity);
            await dataContext.SaveChangesAsync();

            return mappingService.Map<BoardColumn>(boardColumnEntity);
        }
    }
}