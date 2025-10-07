#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints
{
    public class DeleteCatalogItemRequest : BaseRequest
    {
        public DeleteCatalogItemRequest(int catalogItemId)
        {
            CatalogItemId = catalogItemId;
        }

        public int CatalogItemId { get; private set; }
    }
}

#endif
