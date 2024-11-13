using swp_be.Models;
using swp_be.Data;
using swp_be.data.Repositories;
using Microsoft.EntityFrameworkCore;

namespace swp_be.Data.Repositories
{
    public class OrderRepository : GenericRepository<Order>
    {
        public OrderRepository(ApplicationDBContext context) : base(context)
        {
        }

        public override void Create(Order entity)
        {
            _context.Orders.Add(entity);
        }

        public Order? GetOrderByID(int id)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.Promotion)
                .Include(o => o.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Koi)
                .Include(o => o.OrderDetails).ThenInclude(od => od.ConsignmentKoi).ThenInclude(ck => ck.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Batch)
                .FirstOrDefault(o => o.OrderID == id);
        }

        public List<Order> GetOrdersByUserID(int userID)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.Promotion)
                .Include(o => o.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Koi)
                .Include(o => o.OrderDetails).ThenInclude(od => od.ConsignmentKoi).ThenInclude(ck => ck.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Batch)
                .Where(o => o.CustomerID == userID)
                .ToList();
        }

        public List<Order> GetOrdersByStaffID(int staffID)
        {
            return _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Staff)
                .Include(o => o.Promotion)
                .Include(o => o.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Koi)
                .Include(o => o.OrderDetails).ThenInclude(od => od.ConsignmentKoi).ThenInclude(ck => ck.Consignment)
                .Include(o => o.OrderDetails).ThenInclude(od => od.Batch)
                .Where(o => o.StaffID == staffID)
                .ToList();
        }
    }
}