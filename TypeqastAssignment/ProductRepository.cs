using System;
using System.Collections.Generic;
using System.Text;

namespace TypeqastAssignment
{
    public class ProductRepository: IRepository<Product>
    {
        public IEnumerable<Product> GetAll()
        {
            var productRepository = new List<Product>();
            productRepository.Add(new Product(1, "Bread", 10));
            productRepository.Add(new Product(2, "Butter", 2));
            productRepository.Add(new Product(3, "Milk", 6));
            return productRepository;
        }
    }
}
