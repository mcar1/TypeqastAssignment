using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using TypeqastAssignment;

namespace TypeqastAssignmentTest
{
    public class ComplexDiscountTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void ComplexDiscountCalculateDiscountTest()
        {
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(bread, 2));
            requiredProducts.Add(new Tuple<Product, int>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(milk, 0.5));
            discountedProducts.Add(new Tuple<Product, double>(bread, 1));
            var discount = new ComplexDiscount(1, requiredProducts, discountedProducts);
            var discountValue = discount.CalculateDiscount();

            Assert.AreEqual(13, discountValue);
            Assert.Pass();
        }

        [Test]
        public void ComplexDiscountApplyToBasketTest()
        {
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(bread, 2));
            requiredProducts.Add(new Tuple<Product, int>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(milk, 0.6));
            discountedProducts.Add(new Tuple<Product, double>(bread, 1));
            var discount = new ComplexDiscount(1, requiredProducts, discountedProducts);
            var purchasedItems = new List<int>();
            purchasedItems.Add(1);
            purchasedItems.Add(1);
            purchasedItems.Add(1);
            purchasedItems.Add(2);
            purchasedItems.Add(3);
            var result = discount.ApplyToBasket(purchasedItems);
            var expectedResult = new List<Tuple<int, double>>();
            expectedResult.Add(new Tuple<int, double>(1, 2));
            expectedResult.Add(new Tuple<int, double>(2, 1));
            expectedResult.Add(new Tuple<int, double>(3, 0.4));
            expectedResult.Add(new Tuple<int, double>(1, 0));
            Assert.AreEqual(expectedResult, result);
            Assert.Pass();
        }

        [Test]
        public void ComplexDiscountApplyToBasketWhereUnableTest()
        {
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(bread, 2));
            requiredProducts.Add(new Tuple<Product, int>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(milk, 0.6));
            discountedProducts.Add(new Tuple<Product, double>(bread, 1));
            var discount = new ComplexDiscount(1, requiredProducts, discountedProducts);
            var purchasedItems = new List<int>();
            purchasedItems.Add(1);
            purchasedItems.Add(1);
            purchasedItems.Add(2);
            purchasedItems.Add(3);
            var result = discount.ApplyToBasket(purchasedItems);
            Assert.AreEqual(null, result);
            Assert.Pass();
        }

        [Test]
        public void ComplexDiscountToStringTest()
        {
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(bread, 2));
            requiredProducts.Add(new Tuple<Product, int>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(milk, 0.6));
            discountedProducts.Add(new Tuple<Product, double>(bread, 1));
            var discount = new ComplexDiscount(1, requiredProducts, discountedProducts);
            var expectedResult = "Buy 2 breads and 1 butter and get an extra milk at 60% off and 1 bread for free";
            Assert.AreEqual(expectedResult, discount.ToString());
        }
    }
}
