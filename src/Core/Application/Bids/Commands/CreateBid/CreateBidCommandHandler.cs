namespace Application.Bids.Commands.CreateBid
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using AutoMapper;
    using Common.Exceptions;
    using Common.Interfaces;
    using Domain.Entities;
    using global::Common;
    using MediatR;
    using Microsoft.EntityFrameworkCore;

    public class CreateBidCommandHandler : IRequestHandler<CreateBidCommand>
    {
        private readonly IAuctionSystemDbContext context;
        private readonly ICurrentUserService currentUserService;
        private readonly IDateTime dateTime;
        private readonly IMapper mapper;

        public CreateBidCommandHandler(IAuctionSystemDbContext context,
            ICurrentUserService currentUserService,
            IDateTime dateTime,
            IMapper mapper)
        {
            this.context = context;
            this.currentUserService = currentUserService;
            this.dateTime = dateTime;
            this.mapper = mapper;
        }

        public async Task<Unit> Handle(CreateBidCommand request, CancellationToken cancellationToken)
        {
            await CheckWhetherItemIsEligibleForBidding(request, cancellationToken);

            var bid = mapper.Map<Bid>(request);
            await context.Bids.AddAsync(bid, cancellationToken);
            await context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }

        private async Task CheckWhetherItemIsEligibleForBidding(CreateBidCommand request,
            CancellationToken cancellationToken)
        {
            var item = await context
                .Items
                .Select(i => new
                {
                    i.Id,
                    i.StartingPrice,
                    i.StartTime,
                    i.EndTime,
                    HighestBidAmount = i.Bids
                        .Select(b => b.Amount)
                        .OrderByDescending(amount => amount)
                        .FirstOrDefault()
                })
                .Where(i => i.Id == request.ItemId)
                .SingleOrDefaultAsync(cancellationToken);

            if (item == null) throw new NotFoundException(nameof(Item));

            if (request.UserId != currentUserService.UserId) throw new NotFoundException(nameof(Item));

            if (item.StartTime >= dateTime.UtcNow)
                //Bid hasn't started yet.
                throw new BadRequestException(
                    string.Format(ExceptionMessages.Bid.BiddingNotStartedYet, request.ItemId));

            if (item.EndTime <= dateTime.UtcNow)
                // Bidding has ended
                throw new BadRequestException(
                    string.Format(ExceptionMessages.Bid.BiddingHasEnded, request.ItemId));

            if (request.Amount <= item.HighestBidAmount
                || request.Amount <= item.StartingPrice)
                throw new BadRequestException(ExceptionMessages.Bid.InvalidBidAmount);
        }
    }
}