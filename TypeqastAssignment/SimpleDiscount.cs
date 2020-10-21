using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace TypeqastAssignment
{
    public class SimpleDiscount : IDiscount
    {
        public int Id { get; set; }
        public Product ProductRequired { get; set; }
        public int RequiredAmount { get; set; }
        public Product DiscountedProduct { get; set; }
        public double DiscountedAmount { get; set; }

        public SimpleDiscount(int id, Product productRequired, int requiredAmount, Product discountedProduct, double discountedAmount)
        {
            Id = id;
            ProductRequired = productRequired;
            RequiredAmount = requiredAmount;
            DiscountedProduct = discountedProduct;
            DiscountedAmount = discountedAmount;
        }

        public IEnumerable<Tuple<int, double>> ApplyToBasket(List<int> purchasedItems)
        {
            var requiredAmount = DiscountedProduct.Id == ProductRequired.Id ? RequiredAmount + 1 : RequiredAmount;
            if (purchasedItems.Count(pi => pi == this.ProductRequired.Id) >= requiredAmount && purchasedItems.Any(pi => pi == DiscountedProduct.Id))
            {
                var listOfItems = new List<Tuple<int, double>>();
                listOfItems.Add(new Tuple<int, double>(ProductRequired.Id, RequiredAmount));
                listOfItems.Add(new Tuple<int, double>(DiscountedProduct.Id, 1 - DiscountedAmount));
                for (var i = 0; i < RequiredAmount; i++)
                {
                    purchasedItems.Remove(ProductRequired.Id);
                }
                purchasedItems.Remove(DiscountedProduct.Id);
                return listOfItems;
            }
            return null;
        }

        public double CalculateDiscount()
        {
            return DiscountedAmount * DiscountedProduct.Price;
        }

        public override string ToString()
        {
            var productName = ProductRequired.Name;
            productName = RequiredAmount > 1 ? productName + "s" : productName;
            var discountName = new StringBuilder();
            discountName.Append("Buy " + RequiredAmount + " " + productName.ToLower()+ " and get ");
            if(DiscountedAmount < 1)
            {
                var discountedProduct = DiscountedProduct.Name.ToLower();
                discountName.Append("an extra " + discountedProduct + " at " + DiscountedAmount * 100 + "% off");
            }
            else
            {
                var discountedProduct = DiscountedProduct.Name.ToLower();
                discountName.Append(DiscountedAmount + " " + (DiscountedAmount == 1 ? discountedProduct : discountedProduct + "s") + " for free.");
            }
            return discountName.ToString();
        }
    }
}
