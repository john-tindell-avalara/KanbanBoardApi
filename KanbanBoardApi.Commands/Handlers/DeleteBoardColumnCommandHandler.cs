using System.Data.Entity;
using System.Threading;
using System.Threading.Tasks;
using KanbanBoardApi.Domain;
using KanbanBoardApi.EntityFramework;
using KanbanBoardApi.Exceptions;
using MediatR;

namespace KanbanBoardApi.Commands.Handlers
{
    public class DeleteBoardColumnCommandHandler : IRequestHandler<DeleteBoardColumnCommand, string>
    {
        private readonly IDataContext dataContext;

        public DeleteBoardColumnCommandHandler(IDataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<string> Handle(DeleteBoardColumnCommand request, CancellationToken cancellationToken)
        {
            if (!await dataContext.Set<BoardEntity>().AnyAsync(x => x.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardNotFoundException();
            }

            var boardColumnEntity = await dataContext.Set<BoardColumnEntity>()
                .FirstOrDefaultAsync(x => x.Slug == request.BoardColumnSlug && x.BoardEntity.Slug == request.BoardSlug,
                    cancellationToken);

            if (boardColumnEntity == null)
            {
                throw new BoardColumnNotFoundException();
            }

            if (
                await
                    dataContext.Set<BoardTaskEntity>()
                        .AnyAsync(
                            x =>
                                x.BoardColumnEntity.Slug == request.BoardColumnSlug &&
                                x.BoardColumnEntity.BoardEntity.Slug == request.BoardSlug, cancellationToken))
            {
                throw new BoardColumnNotEmptyException();
            }

            dataContext.Delete(boardColumnEntity);
            await dataContext.SaveChangesAsync();

            return request.BoardColumnSlug;
        }
    }
}