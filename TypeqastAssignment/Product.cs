using System;
using System.Collections.Generic;
using System.Text;

namespace TypeqastAssignment
{
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }

        public Product(int id, string name, double price)
        {
            Id = id;
            Name = name;
            Price = price;
        }
    }
}
