using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Domain;
using KanbanBoardApi.Dto;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Mapping;
using MediatR;

namespace KanbanBoardApi.Queries.Handlers
{
    public class GetBoardTaskByIdQueryHandler : IRequestHandler<GetBoardTaskByIdQuery, BoardTask>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public GetBoardTaskByIdQueryHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardTask> Handle(GetBoardTaskByIdQuery request, CancellationToken cancellationToken)
        {
            var boardTaskEntity = await dataContext.Set<BoardTaskEntity>().Include(x => x.BoardColumnEntity)
                .FirstOrDefaultAsync(x => x.Id == request.TaskId, cancellationToken);

            if (boardTaskEntity == null)
            {
                return null;
            }

            return mappingService.Map<BoardTask>(boardTaskEntity);
        }
    }
}