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
    public class GetBoardColumnBySlugQueryHandler : IRequestHandler<GetBoardColumnBySlugQuery, BoardColumn>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public GetBoardColumnBySlugQueryHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardColumn> Handle(GetBoardColumnBySlugQuery request, CancellationToken cancellationToken)
        {
            var boardColumn =
                await dataContext.Set<BoardEntity>()
                    .Where(x => x.Slug == request.BoardSlug)
                    .Select(x => x.Columns.FirstOrDefault(y => y.Slug == request.BoardColumnSlug))
                    .FirstOrDefaultAsync(cancellationToken);

            if (boardColumn == null)
            {
                return null;
            }

            return mappingService.Map<BoardColumn>(boardColumn);
        }
    }
}