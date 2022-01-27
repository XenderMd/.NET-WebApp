using DutchTreat.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace DutchTreat.Data
{
    public class DutchSeeder
    {
        private readonly DutchContext _context;
        private readonly IWebHostEnvironment _env;
        private readonly UserManager<StoreUser> _userManager;

        public DutchSeeder(DutchContext context, IWebHostEnvironment env, UserManager<StoreUser> userManager)
        {
            _context = context;
            _env = env;
            _userManager = userManager;
        }

        public async Task SeedAsync()
        {
            _context.Database.EnsureCreated();

            StoreUser user = await _userManager.FindByEmailAsync("denis.stavilamd@gmail.com");
            if (user == null)
            {
                user = new StoreUser()
                {
                    FirstName = "Denis",
                    LastName = "Stavila",
                    Email = "denis.stavilamd@gmail.com",
                    UserName = "denis.stavilamd@gmail.com"
                };

                var result = await _userManager.CreateAsync(user, "P@ssw0rd!");
                if (result != IdentityResult.Success)
                {
                    throw new InvalidOperationException("Could not create new user in seeder !");
                }
            }

            if (!_context.Products.Any())
            {
                //Need to create sample data
                var filePath = Path.Combine(_env.ContentRootPath, "Data/art.json");
                var json = File.ReadAllText(filePath);
                var products = JsonSerializer.Deserialize<IEnumerable<Product>>(json);
                _context.Products.AddRange(products);

                var order = new Order()
                {
                    OrderDate = DateTime.Today,
                    OrderNumber = "10000",
                    Items = new List<OrderItem>()
                    {
                        new OrderItem()
                        {
                            Product = products.First(),
                            Quantity = 5,
                            UnitPrice = products.First().Price
                        }
                    },
                    User = user
                };

                _context.Orders.Add(order);
                _context.SaveChanges();
            }
        }
    }
}
