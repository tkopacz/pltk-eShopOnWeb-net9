#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Exceptions;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints
{
    /// <summary>
    /// Creates a new Catalog Item
    /// </summary>
    public class CreateCatalogItemEndpoint : Endpoint<CreateCatalogItemRequest, CreateCatalogItemResponse>
    {
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IUriComposer _uriComposer;

        public CreateCatalogItemEndpoint(IRepository<CatalogItem> itemRepository, IUriComposer uriComposer)
        {
            _itemRepository = itemRepository;
            _uriComposer = uriComposer;
        }

        public override void Configure()
        {
            Post("api/catalog-items");
            Roles(BlazorShared.Authorization.Constants.Roles.PRODUCT_MANAGERS);
            AuthSchemes(JwtBearerDefaults.AuthenticationScheme);
            Description(d =>
                d.Produces<CreateCatalogItemResponse>()
                 .WithTags("CatalogItemEndpoints"));
        }

        public override async Task HandleAsync(CreateCatalogItemRequest request, CancellationToken ct)
        {
            var response = new CreateCatalogItemResponse(request.CorrelationId());

            var catalogItemNameSpecification = new CatalogItemNameSpecification(request.Name);
            var existingCatalogItem = await _itemRepository.CountAsync(catalogItemNameSpecification, ct);
            if (existingCatalogItem > 0)
            {
                throw new DuplicateException("A catalogItem with name " + request.Name + " already exists");
            }

            var newItem = new CatalogItem(request.CatalogTypeId, request.CatalogBrandId, request.Description, request.Name, request.Price, request.PictureUri);
            newItem = await _itemRepository.AddAsync(newItem, ct);

            if (newItem.Id != 0)
            {
                newItem.UpdatePictureUri("eCatalog-item-default.png");
                await _itemRepository.UpdateAsync(newItem, ct);
            }

            var dto = new CatalogItemDto
            {
                Id = newItem.Id,
                CatalogBrandId = newItem.CatalogBrandId,
                CatalogTypeId = newItem.CatalogTypeId,
                Description = newItem.Description,
                Name = newItem.Name,
                PictureUri = _uriComposer.ComposePicUri(newItem.PictureUri),
                Price = newItem.Price
            };
            response.CatalogItem = dto;

            await SendCreatedAtAsync<CatalogItemGetByIdEndpoint>(new { CatalogItemId = dto.Id }, response, cancellation: ct);
        }
    }
}

#endif
