namespace Api.SwaggerExamples.Responses.Categories
{
    using System;
    using System.Collections.Generic;
    using Application.Categories.Queries.List;
    using Application.Common.Models;
    using Swashbuckle.AspNetCore.Filters;

    public class ListCategoriesSuccessfulResponse : IExamplesProvider<MultiResponse<ListCategoriesResponseModel>>
    {
        public MultiResponse<ListCategoriesResponseModel> GetExamples()
        {
            return new(
                new List<ListCategoriesResponseModel>
                {
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Art",
                        SubCategories = new List<SubCategoriesDto>
                        {
                            new() {Id = Guid.NewGuid(), Name = "Drawings"},
                            new() {Id = Guid.NewGuid(), Name = "Photography"},
                            new() {Id = Guid.NewGuid(), Name = "Sculptures"}
                        }
                    },
                    new()
                    {
                        Id = Guid.NewGuid(),
                        Name = "Jewelry",
                        SubCategories = new List<SubCategoriesDto>
                        {
                            new() {Id = Guid.NewGuid(), Name = "Necklaces & Pendants"},
                            new() {Id = Guid.NewGuid(), Name = "Brooches & Pins"},
                            new() {Id = Guid.NewGuid(), Name = "Earrings"},
                            new() {Id = Guid.NewGuid(), Name = "Rings"}
                        }
                    }
                });
        }
    }
}