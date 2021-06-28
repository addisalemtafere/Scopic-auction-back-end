namespace Application.Common.Models
{
    using System;
    using System.Collections.Generic;

    public class PagedResponse<T>
    {
        public PagedResponse()
        {
            PageNumber = 1;
            PageSize = AppConstants.PageSize;
        }

        public PagedResponse(IEnumerable<T> data, int totalDataCountInDatabase)
        {
            Data = data;
            TotalPages = (int) Math.Ceiling(totalDataCountInDatabase / (double) AppConstants.PageSize);
        }

        public int TotalPages { get; set; }

        public int PageNumber { get; set; }

        public int? PageSize { get; set; }

        public int? NextPage { get; set; }

        public int? PreviousPage { get; set; }

        public IEnumerable<T> Data { get; set; }

        public int TotalDataCount { get; set; }
    }
}