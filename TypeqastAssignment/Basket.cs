using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace TypeqastAssignment
{
    public class Basket
    {
        private IRepository<IDiscount> DiscountRepository;
        public List<int> PurchasedItems { get; }
        private IRepository<Product> ProductRepository;

        public Basket( IRepository<Product> productRepository, IRepository<IDiscount> discountRepository)
        {
            ProductRepository = productRepository;
            DiscountRepository = discountRepository;
            PurchasedItems = new List<int>();
        }

        public void AddItem(int id)
        {
            if(ProductRepository.GetAll().Where(x => x.Id == id).Any())
            {
                PurchasedItems.Add(id);
            }
        }

        public List<IDiscount> GetOrderedDisounts()
        {
            return DiscountRepository.GetAll().OrderByDescending(d => d.CalculateDiscount()).ToList();
        }

        public Dictionary<int,double> GetDiscountedItems(List<IDiscount> usedDiscounts, List<int> purchasedItems)
        {
            var sortedDiscounts = GetOrderedDisounts();
            var discountedItems = new Dictionary<int, double>();
            foreach (var discount in sortedDiscounts)
            {
                var result = discount.ApplyToBasket(purchasedItems);
                while (result != null)
                {
                    usedDiscounts.Add(discount);
                    foreach (var r in result)
                    {
                        var productPrice = ProductRepository.GetAll().Where(p => p.Id == r.Item1).Select(p => p.Price).FirstOrDefault();
                        discountedItems[r.Item1] = discountedItems.ContainsKey(r.Item1) ? discountedItems[r.Item1] + (r.Item2 * productPrice) : r.Item2 * productPrice;
                    }
                    result = discount.ApplyToBasket(purchasedItems);
                }
            }
            return discountedItems;
        }

        public string GetBill(List<IDiscount> usedDiscounts, Dictionary<int, double> boughtItems, double total)
        {
            var query = PurchasedItems.Join(ProductRepository.GetAll(), y => y, y2 => y2.Id, (i1, i2) => new { id = i1, name = i2.Name })
                .GroupBy(s => s)
                .Select(g => new { g.Key, Count = g.Count() });

            var bill = new StringBuilder();

            foreach (var result in query)
            {
                bill.Append(result.Key.name + " x " + result.Count + " = $" + boughtItems[result.Key.id] + "\n");
            }
            bill.Append("Total: $" + total + "\n");
            bill.Append("Discounts:" + "\n");
            foreach (var usedDiscount in usedDiscounts)
            {
                bill.Append(usedDiscount.ToString() + "\n");
            }
            return bill.ToString();
        }

        public double CalculateRemainingProductsPrice(List<int> remainingProducts, Dictionary<int,double> calculatedItems)
        {
            var total = 0d;
            foreach (var pi in remainingProducts)
            {
                total += ProductRepository.GetAll().Where(p => p.Id == pi).Select(p => p.Price).FirstOrDefault();
                if (calculatedItems.ContainsKey(pi))
                    calculatedItems[pi] += ProductRepository.GetAll().Where(p => p.Id == pi).Select(p => p.Price).FirstOrDefault();
                else
                    calculatedItems[pi] = ProductRepository.GetAll().Where(p => p.Id == pi).Select(p => p.Price).FirstOrDefault();
            }
            return total;
        }

        public void GetTotalCostWithDiscounts()
        {
            if (PurchasedItems.Count() == 0)
                Console.WriteLine("Total: $0.00");
            else
            {
                var total = 0.0;
                var sortedDiscounts = GetOrderedDisounts();
                var purchasedItems = new List<int>(PurchasedItems);
                var usedDiscounts = new List<IDiscount>(); 
                var calculatedItems = GetDiscountedItems(usedDiscounts, purchasedItems);
                total = calculatedItems.Sum(di => di.Value);
                total += CalculateRemainingProductsPrice(purchasedItems, calculatedItems);

                Console.WriteLine(GetBill(usedDiscounts, calculatedItems, total));
            }
        }
    }
}
