namespace Application.Items.Commands.DeleteItem
{
    using System;
    using MediatR;

    public class DeleteItemCommand : IRequest
    {
        public DeleteItemCommand(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}