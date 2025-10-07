#if NETCOREAPP2_0

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FastEndpoints;
using Microsoft.AspNetCore.Http;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.ApplicationCore.Specifications;

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints
{
    /// <summary>
    /// List Catalog Items (paged)
    /// </summary>
    public class CatalogItemListPagedEndpoint : Endpoint<ListPagedCatalogItemRequest, ListPagedCatalogItemResponse>
    {
        private readonly IRepository<CatalogItem> _itemRepository;
        private readonly IUriComposer _uriComposer;
        private readonly IMapper _mapper;

        public CatalogItemListPagedEndpoint(IRepository<CatalogItem> itemRepository, IUriComposer uriComposer, IMapper mapper)
        {
            _itemRepository = itemRepository;
            _uriComposer = uriComposer;
            _mapper = mapper;
        }

        public override void Configure()
        {
            Get("api/catalog-items");
            AllowAnonymous();
            Description(d =>
                d.Produces<ListPagedCatalogItemResponse>()
                 .WithTags("CatalogItemEndpoints"));
        }

        public override async Task<ListPagedCatalogItemResponse> ExecuteAsync(ListPagedCatalogItemRequest request, CancellationToken ct)
        {
            await Task.Delay(1000, ct);

            var response = new ListPagedCatalogItemResponse(request.CorrelationId());

            var filterSpec = new CatalogFilterSpecification(request.CatalogBrandId, request.CatalogTypeId);
            var totalItems = await _itemRepository.CountAsync(filterSpec, ct);

            var pagedSpec = new CatalogFilterPaginatedSpecification(
                request.PageIndex * request.PageSize,
                request.PageSize,
                request.CatalogBrandId,
                request.CatalogTypeId);

            var items = await _itemRepository.ListAsync(pagedSpec, ct);

            response.CatalogItems.AddRange(items.Select(_mapper.Map<CatalogItemDto>));
            foreach (var item in response.CatalogItems)
            {
                item.PictureUri = _uriComposer.ComposePicUri(item.PictureUri);
            }

            if (request.PageSize > 0)
            {
                response.PageCount = (int)Math.Ceiling((decimal)totalItems / request.PageSize);
            }
            else
            {
                response.PageCount = totalItems > 0 ? 1 : 0;
            }

            return response;
        }
    }
}

#endif
