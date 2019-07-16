using System.Data.Entity;
using System.Linq;
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
    public class UpdateBoardTaskCommandHandler : IRequestHandler<UpdateBoardTaskCommand, BoardTask>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public UpdateBoardTaskCommandHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardTask> Handle(UpdateBoardTaskCommand request, CancellationToken cancellationToken)
        {
            var boardTaskEntity = await dataContext.Set<BoardTaskEntity>().FirstOrDefaultAsync(x => x.Id == request.BoardTask.Id, cancellationToken);

            if (boardTaskEntity == null)
            {
                throw new BoardTaskNotFoundException();
            }

            var boardColumnEntity = await dataContext.Set<BoardColumnEntity>()
                .Where(x => x.Slug == request.BoardTask.BoardColumnSlug && x.BoardEntity.Slug == request.BoardSlug)
                .FirstOrDefaultAsync(cancellationToken);

            if (boardColumnEntity == null)
            {
                throw new BoardColumnNotFoundException();
            }

            mappingService.Map(request.BoardTask, boardTaskEntity);

            boardTaskEntity.BoardColumnEntity = boardColumnEntity;

            dataContext.SetModified(boardTaskEntity);

            await dataContext.SaveChangesAsync();

            return mappingService.Map<BoardTask>(boardTaskEntity);
        }
    }
}