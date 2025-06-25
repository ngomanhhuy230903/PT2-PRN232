using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Micho.API.Models;
using Micho.API.ViewModels;

namespace Micho.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportsController : ControllerBase
    {
        private readonly MichoOrderingSystemContext _context;

        public ReportsController(MichoOrderingSystemContext context)
        {
            _context = context;
        }

        [HttpGet("sales")]
        public async Task<ActionResult<SalesReportViewModel>> GetSalesReport(
            [FromQuery] DateTime startDate, [FromQuery] DateTime endDate)
        {
            // Thêm một ngày vào endDate để bao gồm tất cả các đơn hàng trong ngày đó
            var inclusiveEndDate = endDate.AddDays(1);

            var completedOrders = await _context.Orders
                .Where(o => o.Status == "Đã hoàn thành" &&
                            o.OrderDate >= startDate &&
                            o.OrderDate < inclusiveEndDate)
                .Include(o => o.Customer)
                .Include(o => o.OrderDetails)
                .OrderByDescending(o => o.OrderDate)
                .ToListAsync();

            var reportItems = completedOrders.Select(o => new SalesReportItemViewModel
            {
                OrderId = o.OrderId,
                OrderDate = o.OrderDate,
                CustomerName = o.Customer?.Name ?? "N/A",
                OrderTotal = o.OrderDetails.Sum(od => od.TotalAmount),
                Status = o.Status
            }).ToList();

            var report = new SalesReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                TotalRevenue = reportItems.Sum(i => i.OrderTotal),
                TotalOrders = reportItems.Count,
                TotalItemsSold = completedOrders.SelectMany(o => o.OrderDetails).Sum(od => od.Quantity),
                Orders = reportItems
            };

            return Ok(report);
        }
        [HttpGet("analytics")]
        public async Task<ActionResult<AnalyticsViewModel>> GetAnalytics(
    [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var ordersQuery = _context.Orders.AsQueryable();

            if (startDate.HasValue && endDate.HasValue)
            {
                var inclusiveEndDate = endDate.Value.AddDays(1);
                ordersQuery = ordersQuery.Where(o => o.OrderDate >= startDate.Value && o.OrderDate < inclusiveEndDate);
            }

            // 1. Thống kê sản phẩm bán chạy
            var bestSellingProducts = await _context.OrderDetailIceCreams
                .Where(odi => ordersQuery.Any(o => o.OrderId == odi.OrderId)) // Lọc theo ngày nếu có
                .GroupBy(odi => new { odi.IceCreamId, odi.IceCream.Name })
                .Select(g => new BestSellingProductViewModel
                {
                    IceCreamId = g.Key.IceCreamId,
                    IceCreamName = g.Key.Name,
                    // Sum quantity from the related OrderDetail
                    TotalQuantitySold = g.Sum(x => x.OrderDetail.Quantity)
                })
                .OrderByDescending(p => p.TotalQuantitySold)
                .Take(10) // Lấy top 10 sản phẩm
                .ToListAsync();

            // 2. Thống kê giờ cao điểm
            var peakHoursData = await ordersQuery
                .GroupBy(o => o.OrderDate.Hour)
                .Select(g => new { Hour = g.Key, Count = g.Count() })
                .ToListAsync();

            var peakOrderHours = new List<PeakHourViewModel>();
            for (int i = 0; i < 24; i++)
            {
                peakOrderHours.Add(new PeakHourViewModel
                {
                    Hour = i,
                    OrderCount = peakHoursData.FirstOrDefault(p => p.Hour == i)?.Count ?? 0
                });
            }

            var viewModel = new AnalyticsViewModel
            {
                BestSellingProducts = bestSellingProducts,
                PeakOrderHours = peakOrderHours
            };

            return Ok(viewModel);
        }
    }
}