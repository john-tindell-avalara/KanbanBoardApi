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
    public class GetBoardBySlugQueryHandler : IRequestHandler<GetBoardBySlugQuery, Board>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public GetBoardBySlugQueryHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<Board> Handle(GetBoardBySlugQuery request, CancellationToken cancellationToken)
        {
            var board = await dataContext.Set<BoardEntity>().FirstOrDefaultAsync(x => x.Slug == request.BoardSlug, cancellationToken);

            if (board == null)
            {
                return null;
            }

            return mappingService.Map<Board>(board);
        }
    }
}