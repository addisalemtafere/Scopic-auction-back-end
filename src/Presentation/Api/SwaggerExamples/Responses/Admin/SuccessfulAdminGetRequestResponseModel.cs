namespace Api.SwaggerExamples.Responses.Admin
{
    using System;
    using System.Collections.Generic;
    using Application.Admin.Queries.List;
    using Application.Common.Models;
    using Swashbuckle.AspNetCore.Filters;

    public class SuccessfulAdminGetRequestResponseModel : IExamplesProvider<PagedResponse<ListAllUsersResponseModel>>
    {
        public PagedResponse<ListAllUsersResponseModel> GetExamples()
        {
            return new(new List<ListAllUsersResponseModel>
            {
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "admin@admin.com",
                    FullName = "Admin Admin",
                    CurrentRoles = new List<string>
                        {"User, Administrator"},
                    NonCurrentRoles = new List<string>()
                },
                new()
                {
                    Id = Guid.NewGuid().ToString(),
                    Email = "normal@normal.com",
                    FullName = "Normal User",
                    CurrentRoles = new List<string> {"User"},
                    NonCurrentRoles = new List<string> {"Administrator"}
                }
            }, 2);
        }
    }
}