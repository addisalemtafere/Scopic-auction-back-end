namespace Application.Common.Models
{
    using System.Collections.Generic;

    public class MultiResponse<T>
    {
        public MultiResponse(IEnumerable<T> data)
        {
            Data = data;
        }

        public IEnumerable<T> Data { get; }
    }
}