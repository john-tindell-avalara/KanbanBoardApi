using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Domain;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Exceptions;
using MediatR;

namespace KanbanBoardApi.Commands.Handlers
{
    public class DeleteBoardTaskCommandHandler : IRequestHandler<DeleteBoardTaskCommand, int>
    {
        private readonly IDataContext dataContext;

        public DeleteBoardTaskCommandHandler(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<int> Handle(DeleteBoardTaskCommand request, CancellationToken cancellationToken)
        {
            if (!await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardNotFoundException();
            }

            var boardTaskEntity = await dataContext.Set<BoardTaskEntity>()
                .FirstOrDefaultAsync(x => x.Id == request.BoardTaskId, cancellationToken);

            if (boardTaskEntity == null)
            {
                throw new BoardTaskNotFoundException();
            }

            dataContext.Delete(boardTaskEntity);
            await dataContext.SaveChangesAsync();

            return request.BoardTaskId;
        }
    }
}