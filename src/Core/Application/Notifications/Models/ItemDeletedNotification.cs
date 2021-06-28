namespace Application.Notifications.Models
{
    using System;
    using MediatR;

    public class ItemDeletedNotification : INotification
    {
        public ItemDeletedNotification(Guid itemId)
        {
            ItemId = itemId;
        }

        public Guid ItemId { get; set; }
    }
}