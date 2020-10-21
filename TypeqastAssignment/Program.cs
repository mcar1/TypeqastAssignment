using System;
using System.Collections.Generic;

namespace TypeqastAssignment
{
    class Program
    {
        static void Main(string[] args)
        {
            var productRepo = new ProductRepository();
            var discountRepo = new DiscountRepository();

            var myBasket = new Basket(productRepo, discountRepo);
            myBasket.AddItem(1);
            myBasket.AddItem(1);
            myBasket.AddItem(1);
            myBasket.AddItem(1);
            myBasket.AddItem(1);
            myBasket.AddItem(2);
            myBasket.AddItem(3);
            myBasket.AddItem(3);
            myBasket.GetTotalCostWithDiscounts();
        }
    }
}
