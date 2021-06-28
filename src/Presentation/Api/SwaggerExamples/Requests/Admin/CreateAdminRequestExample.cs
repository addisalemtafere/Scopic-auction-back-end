namespace Api.SwaggerExamples.Requests.Admin
{
    using Application.Admin.Commands.CreateAdmin;
    using Swashbuckle.AspNetCore.Filters;

    public class CreateAdminRequestExample : IExamplesProvider<CreateAdminCommand>
    {
        public CreateAdminCommand GetExamples()
        {
            return new() {Email = "test1@test.com", Role = "Administrator"};
        }
    }
}