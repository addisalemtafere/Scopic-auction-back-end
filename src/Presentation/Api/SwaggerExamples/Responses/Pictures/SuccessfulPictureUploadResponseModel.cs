namespace Api.SwaggerExamples.Responses.Pictures
{
    using System;
    using System.Collections.Generic;
    using Application.Common.Models;
    using Application.Pictures;
    using Swashbuckle.AspNetCore.Filters;

    public class SuccessfulPictureUploadResponseModel : IExamplesProvider<MultiResponse<PictureResponseModel>>
    {
        public MultiResponse<PictureResponseModel> GetExamples()
        {
            return new(new HashSet<PictureResponseModel>
            {
                new()
                {
                    Id = Guid.NewGuid(),
                    Url = "image url (i.e https://google.com)"
                },
                new()
                {
                    Id = Guid.NewGuid(),
                    Url = "Some random url"
                }
            });
        }
    }
}