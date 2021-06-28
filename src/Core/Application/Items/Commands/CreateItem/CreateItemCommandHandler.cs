namespace Application.Items.Commands.CreateItem
{
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using Common.Models;
    using Domain.Entities;
    using MediatR;
    using Microsoft.EntityFrameworkCore;
    using Pictures.Commands.CreatePicture;

    public class CreateItemCommandHandler : IRequestHandler<CreateItemCommand, Response<ItemResponseModel>>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly IMapper mapper;
        private readonly IMediator mediator;
        private readonly ICurrentUserService userService;

        public CreateItemCommandHandler(IAuctionSystemDbContext context,
            IMapper mapper,
            IMediator mediator,
            ICurrentUserService userService)
        {
            this.context = context;
            this.mapper = mapper;
            this.mediator = mediator;
            this.userService = userService;
        }


        public async Task<Response<ItemResponseModel>> Handle(CreateItemCommand request,
            CancellationToken cancellationToken)
        {
            if (userService.UserId == null
                || !await context.SubCategories.AnyAsync(c => c.Id == request.SubCategoryId, cancellationToken))
                throw new BadRequestException(ExceptionMessages.Item.CreateItemErrorMessage);

            var item = mapper.Map<Item>(request);
            item.UserId = userService.UserId;
            item.StartTime = item.StartTime.ToUniversalTime();
            item.EndTime = item.EndTime.ToUniversalTime();

            await context.Items.AddAsync(item, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            await mediator.Send(new CreatePictureCommand {ItemId = item.Id, Pictures = request.Pictures},
                cancellationToken);

            return new Response<ItemResponseModel>(new ItemResponseModel(item.Id));
        }
    }
}