#if NETCOREAPP2_0

namespace Microsoft.eShopWeb.PublicApi.CatalogItemEndpoints
{
    public class ListPagedCatalogItemRequest : BaseRequest
    {
        public ListPagedCatalogItemRequest(int? pageSize, int? pageIndex, int? catalogBrandId, int? catalogTypeId)
        {
            PageSize = pageSize ?? 0;
            PageIndex = pageIndex ?? 0;
            CatalogBrandId = catalogBrandId;
            CatalogTypeId = catalogTypeId;
        }

        public int PageSize { get; private set; }

        public int PageIndex { get; private set; }

        public int? CatalogBrandId { get; private set; }

        public int? CatalogTypeId { get; private set; }
    }
}

#endif
