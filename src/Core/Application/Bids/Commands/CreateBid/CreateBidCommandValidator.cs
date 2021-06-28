namespace Application.Bids.Commands.CreateBid
{
    using FluentValidation;
    using global::Common;

    public class CreateBidCommandValidator : AbstractValidator<CreateBidCommand>
    {
        public CreateBidCommandValidator()
        {
            RuleFor(p => p.Amount).NotEmpty()
                .InclusiveBetween(ModelConstants.Bid.MinAmount, ModelConstants.Bid.MaxAmount);
            RuleFor(p => p.ItemId).NotEmpty();
            RuleFor(p => p.UserId).NotEmpty();
        }
    }
}