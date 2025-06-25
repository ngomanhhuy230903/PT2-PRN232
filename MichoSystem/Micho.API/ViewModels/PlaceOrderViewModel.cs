namespace Micho.API.ViewModels
{
    public class PlaceOrderViewModel
    {
        public string CustomerName { get; set; }
        public string CustomerAddress { get; set; }
        public string CustomerContact { get; set; }
        public List<CartItemViewModel> Items { get; set; }
    }

    public class CartItemViewModel
    {
        public int IceCreamId { get; set; }
        public int Quantity { get; set; }
    }
}