using Micho.API.Models;
using Micho.API.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Micho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly MichoOrderingSystemContext _context;

        public OrdersController(MichoOrderingSystemContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderViewModel orderViewModel)
        {
            if (orderViewModel == null || !orderViewModel.Items.Any(i => i.Quantity > 0))
            {
                return BadRequest("Invalid order data.");
            }

            var selectedItems = orderViewModel.Items.Where(i => i.Quantity > 0).ToList();

            var customer = await _context.Customers
                .FirstOrDefaultAsync(c => c.Contact == orderViewModel.CustomerContact);

            if (customer == null)
            {
                customer = new Customer
                {
                    Name = orderViewModel.CustomerName,
                    Address = orderViewModel.CustomerAddress,
                    Contact = orderViewModel.CustomerContact
                };
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }

            var order = new Models.Order
            {
                Status = "Đang xử lý",
                OrderDate = DateTime.Now,
                CustomerId = customer.CustomerId
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            foreach (var item in selectedItems)
            {
                var iceCream = await _context.IceCreams.FindAsync(item.IceCreamId);
                if (iceCream == null) continue;

                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    Quantity = item.Quantity,
                    TotalAmount = item.Quantity * iceCream.Price
                };
                _context.OrderDetails.Add(orderDetail);
                await _context.SaveChangesAsync();

                var orderDetailIceCream = new OrderDetailIceCream
                {
                    OrderDetailId = orderDetail.OrderDetailId,
                    OrderId = order.OrderId,
                    IceCreamId = item.IceCreamId
                };
                _context.OrderDetailIceCreams.Add(orderDetailIceCream);
            }

            await _context.SaveChangesAsync();

            return Ok(new { orderId = order.OrderId });
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<InvoiceViewModel>> GetOrderDetails(int id)
        {
            var order = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                    .ThenInclude(od => od.OrderDetailIceCreams)
                        .ThenInclude(odi => odi.IceCream)
                .FirstOrDefaultAsync(o => o.OrderId == id);

            if (order == null)
            {
                return NotFound();
            }

            var invoiceItems = new List<InvoiceItemViewModel>();
            foreach (var detail in order.OrderDetails)
            {
                // Lấy thông tin kem từ bảng liên kết đầu tiên tìm thấy
                var iceCreamInfo = detail.OrderDetailIceCreams.FirstOrDefault()?.IceCream;
                if (iceCreamInfo != null)
                {
                    invoiceItems.Add(new InvoiceItemViewModel
                    {
                        IceCreamName = iceCreamInfo.Name,
                        Quantity = detail.Quantity,
                        UnitPrice = detail.TotalAmount / detail.Quantity, // Tính lại đơn giá
                        TotalAmount = detail.TotalAmount
                    });
                }
            }

            var invoice = new InvoiceViewModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                OrderStatus = order.Status,
                CustomerName = order.Customer.Name,
                CustomerAddress = order.Customer.Address,
                CustomerContact = order.Customer.Contact,
                Items = invoiceItems,
                GrandTotal = order.OrderDetails.Sum(od => od.TotalAmount)
            };

            return Ok(invoice);
        }
    }
}