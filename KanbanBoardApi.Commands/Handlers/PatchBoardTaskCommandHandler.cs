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
    public class PatchBoardTaskCommandHandler : IRequestHandler<PatchBoardTaskCommand, BoardTask>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public PatchBoardTaskCommandHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardTask> Handle(PatchBoardTaskCommand request, CancellationToken cancellationToken)
        {
            var boardTaskEntity = await dataContext.Set<BoardTaskEntity>()
                .Include(x => x.BoardColumnEntity)
                .FirstOrDefaultAsync(x => x.Id == request.BoardTaskId, cancellationToken);

            if (boardTaskEntity == null)
            {
                throw new BoardTaskNotFoundException();
            }

            var boardTask = new BoardTask();
            mappingService.Map(boardTaskEntity, boardTask);
            request.BoardTask.Patch(boardTask);
            mappingService.Map(boardTask, boardTaskEntity);

            if (!string.IsNullOrEmpty(boardTask.BoardColumnSlug))
            {
                var boardColumnEntity = await dataContext.Set<BoardColumnEntity>()
                    .Where(x => x.Slug == boardTask.BoardColumnSlug && x.BoardEntity.Slug == request.BoardSlug)
                    .FirstOrDefaultAsync(cancellationToken);

                if (boardColumnEntity == null)
                {
                    throw new BoardColumnNotFoundException();
                }

                boardTaskEntity.BoardColumnEntity = boardColumnEntity;
            }


            dataContext.SetModified(boardTaskEntity);

            await dataContext.SaveChangesAsync();

            return mappingService.Map<BoardTask>(boardTaskEntity);
        }
    }
}