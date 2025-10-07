#if NETCOREAPP2_0

using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.eShopWeb.Web.Services;
using Microsoft.eShopWeb.Web.ViewModels;

namespace Microsoft.eShopWeb.Web.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ICatalogViewModelService _catalogViewModelService;

        public IndexModel(ICatalogViewModelService catalogViewModelService)
        {
            _catalogViewModelService = catalogViewModelService;
            CatalogModel = new CatalogIndexViewModel();
        }

        public CatalogIndexViewModel CatalogModel { get; set; }

        public async Task OnGet(CatalogIndexViewModel catalogModel, int? pageId)
        {
            var pageIndex = pageId.HasValue ? pageId.Value : 0;
            CatalogModel = await _catalogViewModelService.GetCatalogItems(pageIndex, Constants.ITEMS_PER_PAGE, catalogModel.BrandFilterApplied, catalogModel.TypesFilterApplied);
        }
    }
}

#endif
