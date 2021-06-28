namespace Application.Items.Commands
{
    using System;

    public class ItemResponseModel
    {
        public ItemResponseModel(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}