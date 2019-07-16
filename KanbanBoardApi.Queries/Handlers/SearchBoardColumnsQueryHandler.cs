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

namespace KanbanBoardApi.Queries.Handlers
{
    public class SearchBoardColumnsQueryHandler : IRequestHandler<SearchBoardColumnsQuery, BoardColumnCollection>
    {
        private readonly IDataContext dataContext;
        private readonly IMappingService mappingService;

        public SearchBoardColumnsQueryHandler(IDataContext dataContext, IMappingService mappingService)
        {
            this.dataContext = dataContext;
            this.mappingService = mappingService;
        }

        public async Task<BoardColumnCollection> Handle(SearchBoardColumnsQuery request, CancellationToken cancellationToken)
        {
            if (!await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardNotFoundException();
            }

            var boardColumnEntities = await dataContext.Set<BoardColumnEntity>().Where(x => x.BoardEntity.Slug == request.BoardSlug).ToListAsync(cancellationToken);

            return new BoardColumnCollection
            {
                Items = boardColumnEntities.Select(x => mappingService.Map<BoardColumn>(x)).ToList()
            };
        }
    }
}