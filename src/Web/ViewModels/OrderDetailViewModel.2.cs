#if NETCOREAPP2_0

using System.Collections.Generic;

namespace Microsoft.eShopWeb.Web.ViewModels
{
    public class OrderDetailViewModel : OrderViewModel
    {
        public OrderDetailViewModel()
        {
            OrderItems = new List<OrderItemViewModel>();
        }

        public List<OrderItemViewModel> OrderItems { get; set; }
    }
}

#endif
