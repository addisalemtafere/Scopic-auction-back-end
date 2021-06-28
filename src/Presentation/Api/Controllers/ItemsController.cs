namespace Api.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Common;
    using Application.Common.Models;
    using Application.Items.Commands;
    using Application.Items.Commands.CreateItem;
    using Application.Items.Commands.DeleteItem;
    using Application.Items.Commands.UpdateItem;
    using Application.Items.Queries.Details;
    using Application.Items.Queries.List;
    using AutoMapper;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Models;
    using SwaggerExamples;
    using Swashbuckle.AspNetCore.Annotations;

    public class ItemsController : BaseController
    {
        private const int CachingTimeInMinutes = 10;

        private readonly IMapper mapper;

        public ItemsController(IMapper mapper)
        {
            this.mapper = mapper;
        }

        /// <summary>
        /// Retrieves all items (max 24 per request)
        /// </summary>
        [HttpGet]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.ItemConstants.SuccessfulGetRequestMessage,
            typeof(PagedResponse<ListItemsResponseModel>))]
        [Cached(CachingTimeInMinutes)]
        public async Task<IActionResult> Get([FromQuery] PaginationQuery paginationQuery,
            [FromQuery] ItemsFilter filters)
        {
            var paginationFilter = mapper.Map<PaginationFilter>(paginationQuery);
            var model = mapper.Map<ListItemsQuery>(paginationFilter);
            model.Filters = mapper.Map<ListAllItemsQueryFilter>(filters);
            var result = await Mediator.Send(model);
            return Ok(result);
        }

        /// <summary>
        /// Retrieves item with given id
        /// </summary>
        [HttpGet("{id}")]
        [SwaggerResponse(
            StatusCodes.Status200OK,
            SwaggerDocumentation.ItemConstants.SuccessfulGetRequestWithIdDescriptionMessage,
            typeof(Response<ItemDetailsResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        public async Task<IActionResult> Get(Guid id)
        {
            var result = await Mediator.Send(new GetItemDetailsQuery(id));
            return Ok(result);
        }

        /// <summary>
        /// Creates item
        /// </summary>
        [Authorize]
        [HttpPost]
        [SwaggerResponse(
            StatusCodes.Status201Created,
            SwaggerDocumentation.ItemConstants.SuccessfulPostRequestDescriptionMessage,
            typeof(Response<ItemResponseModel>))]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Post([FromForm] CreateItemCommand model)
        {
            var result = await Mediator.Send(model);
            return CreatedAtAction(nameof(Post), result);
        }

        /// <summary>
        /// Updates item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        [Authorize]
        [HttpPut("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.ItemConstants.SuccessfulPutRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status400BadRequest,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(BadRequestErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.ItemConstants.NotFoundDescriptionMessage,
            typeof(NotFoundErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Put(Guid id, [FromForm] UpdateItemCommand model)
        {
            if (id != model.Id) return BadRequest();

            await Mediator.Send(model);
            return NoContent();
        }

        /// <summary>
        /// Deletes item
        /// </summary>
        /// <param name="id"></param>
        [Authorize]
        [HttpDelete("{id}")]
        [SwaggerResponse(
            StatusCodes.Status204NoContent,
            SwaggerDocumentation.ItemConstants.SuccessfulDeleteRequestDescriptionMessage)]
        [SwaggerResponse(
            StatusCodes.Status404NotFound,
            SwaggerDocumentation.ItemConstants.BadRequestDescriptionMessage,
            typeof(NotFoundErrorModel))]
        [SwaggerResponse(
            StatusCodes.Status401Unauthorized,
            SwaggerDocumentation.UnauthorizedDescriptionMessage)]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteItemCommand(id));
            return NoContent();
        }
    }
}