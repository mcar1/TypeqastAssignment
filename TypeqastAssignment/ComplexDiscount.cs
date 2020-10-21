using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TypeqastAssignment
{
    public class ComplexDiscount : IDiscount
    {
        public int Id { get; set; }
        public List<Tuple<Product, int>> RequiredProducts { get; set; }
        public List<Tuple<Product, double>> DiscountedProducts { get; set; }

        public ComplexDiscount(int id, List<Tuple<Product, int>> requiredProducts, List<Tuple<Product, double>> discountedProducts)
        {
            Id = id;
            RequiredProducts = requiredProducts;
            DiscountedProducts = discountedProducts;
        }

        public IEnumerable<Tuple<int, double>> ApplyToBasket(List<int> purchasedItems)
        {
            var hasAllRequired = true;
            var hasAllDiscounted = true;
            foreach (var requiredProduct in RequiredProducts)
            {
                var requiredAmount = DiscountedProducts.Any(dp => dp.Item1 == requiredProduct.Item1) ? requiredProduct.Item2 
                    + Math.Ceiling(DiscountedProducts.Where(dp => dp.Item1 == requiredProduct.Item1).Select(dp =>dp.Item2).FirstOrDefault()) : requiredProduct.Item2;
                
                if (purchasedItems.Count(pi => pi == requiredProduct.Item1.Id) < requiredAmount)
                {
                    hasAllRequired = false;
                    break;
                }
            }
            foreach (var disoucntedProduct in DiscountedProducts)
            {
                if(purchasedItems.Count(pi => pi == disoucntedProduct.Item1.Id) < Math.Ceiling(disoucntedProduct.Item2))
                {
                    hasAllDiscounted = false;
                }
            }
            if (hasAllRequired && hasAllDiscounted)
            {
                var listOfItems = new List<Tuple<int, double>>();

                foreach(var pr in RequiredProducts)
                {
                    listOfItems.Add(new Tuple<int, double>(pr.Item1.Id, pr.Item2));
                    for(int i=0; i < pr.Item2; i++)
                    {
                        purchasedItems.Remove(pr.Item1.Id);
                    }
                }
                foreach (var dp in DiscountedProducts)
                {
                    listOfItems.Add(new Tuple<int, double>(dp.Item1.Id, 1 - dp.Item2));
                    for (int i = 0; i < dp.Item2; i++)
                    {
                        purchasedItems.Remove(dp.Item1.Id);
                    }
                }
                return listOfItems;
            }
            return null;
        }

        public double CalculateDiscount()
        {
            var discount = 0d;
            foreach(var discountedProduct in DiscountedProducts)
            {
                discount += discountedProduct.Item1.Price * discountedProduct.Item2;
            }
            return discount;
        }

        public override string ToString()
        {
            var discountName = new StringBuilder();

            foreach (var requiredProduct in RequiredProducts)
            {
                var productName = requiredProduct.Item1.Name;
                productName = requiredProduct.Item2 > 1 ? productName + "s" : productName;
                if(RequiredProducts.IndexOf(requiredProduct) == 0)
                {
                    discountName.Append("Buy " + requiredProduct.Item2 + " " + productName.ToLower());
                }
                else if(RequiredProducts.IndexOf(requiredProduct) == RequiredProducts.Count() - 1)
                {
                    discountName.Append(" and " + requiredProduct.Item2 + " " + productName.ToLower()+" and get ");
                }
                else
                {
                    discountName.Append(" and " + requiredProduct.Item2 + " " + productName.ToLower());
                }
            }

            foreach(var discountedProduct in DiscountedProducts)
            {
                if (discountedProduct.Item2 < 1)
                {
                    var discountedProductName = discountedProduct.Item1.Name.ToLower(); ;
                    discountName.Append("an extra " + discountedProductName + " at " + discountedProduct.Item2 * 100 + "% off");
                    if(DiscountedProducts.IndexOf(discountedProduct) != DiscountedProducts.Count() - 1)
                    {
                        discountName.Append(" and ");
                    }
                }
                else
                {
                    var discountedProductName = discountedProduct.Item1.Name.ToLower();
                    discountName.Append(discountedProduct.Item2 + " " + (discountedProduct.Item2 == 1 ? discountedProductName : discountedProductName + "s") + " for free");
                    if (DiscountedProducts.IndexOf(discountedProduct) != DiscountedProducts.Count() - 1)
                    {
                        discountName.Append(" and ");
                    }
                }
            }
            return discountName.ToString();
        }
    }
}
