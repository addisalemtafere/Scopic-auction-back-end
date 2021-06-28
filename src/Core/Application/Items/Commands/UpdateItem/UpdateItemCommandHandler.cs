namespace Application.Items.Commands.UpdateItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using Common.Exceptions;
    using Common.Interfaces;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pictures.Commands.UpdatePicture;

    public class UpdateItemCommandHandler : IRequestHandler<UpdateItemCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IMediator mediator;

        public UpdateItemCommandHandler(
            IAuctionSystemDbContext context,
            ICurrentUserService currentUserService,
            IMediator mediator)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.mediator = mediator;
        }

        public async Task<Unit> Handle(UpdateItemCommand request, CancellationToken cancellationToken)
        {
            var item = await context
                .Items
                .SingleOrDefaultAsync(i => i.Id == request.Id, cancellationToken);

            if (item == null
                || item.UserId != currentUserService.UserId)
                throw new NotFoundException(nameof(item));

            if (!await context
                .SubCategories
                .AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
                throw new BadRequestException(ExceptionMessages.Item.SubCategoryDoesNotExist);

            item.Title = request.Title;
            item.Description = request.Description;
            item.StartingPrice = request.StartingPrice;
            item.MinIncrease = request.MinIncrease;
            item.StartTime = request.StartTime.ToUniversalTime();
            item.EndTime = request.EndTime.ToUniversalTime();
            item.SubCategoryId = request.SubCategoryId;

            context.Items.Update(item);
            await context.SaveChangesAsync(cancellationToken);
            await mediator.Send(new UpdatePictureCommand
                {
                    ItemId = item.Id, PicturesToAdd = request.PicturesToAdd, PicturesToRemove = request.PicturesToRemove
                },
                cancellationToken);

            return Unit.Value;
        }
    }
}