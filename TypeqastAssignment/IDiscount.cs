using System;
using System.Collections.Generic;
using System.Text;

namespace TypeqastAssignment
{
    public interface IDiscount
    {
        public int Id { get; set; }
        public IEnumerable<Tuple<int, double>> ApplyToBasket(List<int> purchasedItems);
        public double CalculateDiscount();
        public string ToString();
    }
}
