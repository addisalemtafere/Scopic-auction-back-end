namespace Api.SwaggerExamples.Responses.Items
{
    using System;
    using Application.Common.Models;
    using Application.Items.Commands;
    using Swashbuckle.AspNetCore.Filters;

    public class SuccessfulItemCreateResponse : IExamplesProvider<Response<ItemResponseModel>>
    {
        public Response<ItemResponseModel> GetExamples()
        {
            return new(new ItemResponseModel(Guid.NewGuid()));
        }
    }
}