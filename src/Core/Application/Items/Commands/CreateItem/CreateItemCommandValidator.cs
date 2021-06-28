namespace Application.Items.Commands.CreateItem
{
    using FluentValidation;
    using global::Common;

    public class CreateItemCommandValidator : AbstractValidator<CreateItemCommand>
    {
        private readonly IDateTime dateTime;

        public CreateItemCommandValidator(IDateTime dateTime)
        {
            this.dateTime = dateTime;

            RuleFor(p => p.Title).NotEmpty().MaximumLength(ModelConstants.Item.TitleMaxLength);
            RuleFor(p => p.Description).NotEmpty().MaximumLength(ModelConstants.Item.DescriptionMaxLength);
            RuleFor(p => p.StartingPrice).NotEmpty()
                .InclusiveBetween(ModelConstants.Item.MinStartingPrice, ModelConstants.Item.MaxStartingPrice);
            RuleFor(p => p.MinIncrease).NotEmpty()
                .InclusiveBetween(ModelConstants.Item.MinMinIncrease, ModelConstants.Item.MaxMinIncrease);

            RuleFor(p => p.StartTime).NotEmpty();
            RuleFor(p => p.EndTime).NotEmpty();

            RuleFor(m => new {m.StartTime, m.EndTime}).NotEmpty()
                .Must(x => x.EndTime.Date.ToUniversalTime() >= x.StartTime.Date.ToUniversalTime())
                .WithMessage("End time must be after start time")
                .Must(x => x.StartTime.ToUniversalTime() >= this.dateTime.Now.ToUniversalTime())
                .WithMessage("The Start time must be after the current time");

            RuleFor(p => p.SubCategoryId).NotEmpty();
        }
    }
}