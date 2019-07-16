using System.Data.Entity;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Domain;
using KanbanBoardApi.Dto;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Mapping;
using MediatR;

namespace KanbanBoardApi.Queries.Handlers
{
    public class SearchBoardTasksQueryHandler : IRequestHandler<SearchBoardTasksQuery, BoardTaskCollection>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public SearchBoardTasksQueryHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardTaskCollection> Handle(SearchBoardTasksQuery request, CancellationToken cancellationToken)
        {
            var linqQuery = dataContext.Set<BoardTaskEntity>()
                .Include(x => x.BoardColumnEntity)
                .Where(x => x.BoardColumnEntity.BoardEntity.Slug == request.BoardSlug);

            if (!string.IsNullOrWhiteSpace(request.BoardColumnSlug))
            {
                linqQuery = linqQuery.Where(x => x.BoardColumnEntity.Slug == request.BoardColumnSlug);
            }

            var boardTasks = await linqQuery.ToListAsync(cancellationToken);

            return new BoardTaskCollection
            {
                Items = boardTasks.Select(x => mappingService.Map<BoardTask>(x)).ToList()
            };
        }
    }
}