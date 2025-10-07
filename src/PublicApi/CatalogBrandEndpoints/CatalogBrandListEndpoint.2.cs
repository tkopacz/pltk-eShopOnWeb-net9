#if NETCOREAPP2_0

using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using FastEndpoints;
using Microsoft.eShopWeb.ApplicationCore.Entities;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;

namespace Microsoft.eShopWeb.PublicApi.CatalogBrandEndpoints
{
    /// <summary>
    /// List Catalog Brands
    /// </summary>
    public class CatalogBrandListEndpoint : EndpointWithoutRequest<ListCatalogBrandsResponse>
    {
        private readonly IRepository<CatalogBrand> _catalogBrandRepository;
        private readonly IMapper _mapper;

        public CatalogBrandListEndpoint(IRepository<CatalogBrand> catalogBrandRepository, IMapper mapper)
        {
            _catalogBrandRepository = catalogBrandRepository;
            _mapper = mapper;
        }

        public override void Configure()
        {
            Get("api/catalog-brands");
            AllowAnonymous();
            Description(d =>
                d.Produces<ListCatalogBrandsResponse>()
                 .WithTags("CatalogBrandEndpoints"));
        }

        public override async Task<ListCatalogBrandsResponse> ExecuteAsync(CancellationToken ct)
        {
            var response = new ListCatalogBrandsResponse();

            var items = await _catalogBrandRepository.ListAsync(ct);

            response.CatalogBrands.AddRange(items.Select(_mapper.Map<CatalogBrandDto>));

            return response;
        }
    }
}

#endif
