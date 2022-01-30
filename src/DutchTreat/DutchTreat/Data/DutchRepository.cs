using DutchTreat.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchRepository : IDutchRepository
    {
        private readonly DutchContext _context;
        private readonly ILogger<DutchRepository> _logger;

        public DutchRepository(DutchContext context, ILogger<DutchRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void AddEntity(object model)
        {
            _context.Add(model);
        }

        public IEnumerable<Order> GetAllOrders(bool includeItems)
        {
            if (includeItems)
            {
                return _context.Orders
                            .Include(o => o.Items)
                            .ThenInclude(item => item.Product)
                            .ToList();
            }
            else
            {
                return _context.Orders.ToList();
            }
        }

        public IEnumerable<Order> GetAllOrdersByUser(string username, bool includeItems)
        {
            if (includeItems)
            {
                return _context.Orders
                            .Where(order=>order.User.UserName==username)
                            .Include(o => o.Items)
                            .ThenInclude(item => item.Product)
                            .ToList();
            }
            else
            {
                return _context.Orders
                               .Where(order => order.User.UserName == username)
                               .ToList();
            }
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                _logger.LogInformation("Get all products was called");
                return _context.Products.OrderBy(p => p.Title).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to get all products:{ex}");
                return null;
            }

        }

        public Order GetOrderById(int id, string username)
        {
            return _context.Orders
                            .Where(order => order.Id == id && order.User.UserName == username)
                            .Include(order => order.Items)
                            .ThenInclude(item => item.Product)
                            .FirstOrDefault();
        }

        public IEnumerable<Product> GetProductsByCategory(string category)
        {
            return _context.Products.Where(p => p.Category == category).ToList();
        }

        public bool SaveAll()
        {
            return _context.SaveChanges() > 0;
        }
    }
}
