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
    public class CreateBoardTaskCommandHandler : IRequestHandler<CreateBoardTaskCommand, BoardTask>
    {
        private IDataContext dataContext;
        private IMappingService mappingService;

        public CreateBoardTaskCommandHandler(IDataContext dataContext, IMappingService  mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardTask> Handle(CreateBoardTaskCommand request, CancellationToken cancellationToken)
        {
            var boardTask = mappingService.Map<BoardTaskEntity>(request.BoardTask);

            if (!await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardNotFoundException();
            }

            var boardColumn =
                await dataContext.Set<BoardEntity>()
                    .Where(x => x.Slug == request.BoardSlug)
                    .Select(x => x.Columns.FirstOrDefault(y => y.Slug == request.BoardTask.BoardColumnSlug))
                    .FirstOrDefaultAsync(cancellationToken);

            if (boardColumn == null)
            {
                throw new BoardColumnNotFoundException();
            }

            boardTask.BoardColumnEntity = boardColumn;
            dataContext.Set<BoardTaskEntity>().Add(boardTask);

            await dataContext.SaveChangesAsync();
            return mappingService.Map<BoardTask>(boardTask);
        }
    }
}