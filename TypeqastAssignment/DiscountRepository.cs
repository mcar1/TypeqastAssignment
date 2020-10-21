using System;
using System.Collections.Generic;
using System.Text;

namespace TypeqastAssignment
{
    public class DiscountRepository : IRepository<IDiscount>
    {
        public IEnumerable<IDiscount> GetAll()
        {
            var discountRepository = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            discountRepository.Add(new SimpleDiscount(1, bread, 2, bread, 0.5));

            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            discountRepository.Add(new ComplexDiscount(2, requiredProducts, discountedProducts));
            return discountRepository;
        }
    }
}
