#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints
{
    public class GetByIdCatalogItemRequest : BaseRequest
    {
        public GetByIdCatalogItemRequest(int catalogItemId)
        {
            CatalogItemId = catalogItemId;
        }

        public int CatalogItemId { get; private set; }
    }
}

#endif
