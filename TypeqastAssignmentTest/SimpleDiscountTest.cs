using Moq;
using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TypeqastAssignment;

namespace TypeqastAssignmentTest
{
    public class SimpleDiscountTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void SimpleDiscountCalculateDiscountTest()
        {
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var discount = new SimpleDiscount(1, bread, 1, butter, 0.5);
            var discountValue = discount.CalculateDiscount();

            Assert.AreEqual(1, discountValue);
            Assert.Pass();
        }

        [Test]
        public void SimpleDiscountCalculateSameProductDiscountTest()
        {
            var bread = new Product(1, "Bread", 10);
            var discount = new SimpleDiscount(1, bread, 1, bread, 0.5);
            var discountValue = discount.CalculateDiscount();

            Assert.AreEqual(5, discountValue);
            Assert.Pass();
        }

        [Test]
        public void SimpleDiscountApplyToBasketTest()
        {
            var bread = new Product(1, "Bread", 10);
            var milk = new Product(3, "Milk", 6);
            
            var discount = new SimpleDiscount(1, bread, 2, milk, 0.6);
            var purchasedItems = new List<int>();
            purchasedItems.Add(1);
            purchasedItems.Add(1);
            purchasedItems.Add(1);
            purchasedItems.Add(3);
            var result = discount.ApplyToBasket(purchasedItems);
            var expectedResult = new List<Tuple<int, double>>();
            expectedResult.Add(new Tuple<int, double>(1, 2));
            expectedResult.Add(new Tuple<int, double>(3, 0.4));
            Assert.AreEqual(expectedResult, result);
            Assert.Pass();
        }

        [Test]
        public void SimpleDiscountApplyToBasketWhereUnableTest()
        {
            var bread = new Product(1, "Bread", 10);
            var milk = new Product(3, "Milk", 6);

            var discount = new SimpleDiscount(1, bread, 2, milk, 0.6);
            var purchasedItems = new List<int>();
            purchasedItems.Add(1);
            purchasedItems.Add(2);
            purchasedItems.Add(2);
            purchasedItems.Add(3);
            var result = discount.ApplyToBasket(purchasedItems);
            Assert.AreEqual(null, result);
            Assert.Pass();
        }

        [Test]
        public void SimpleDiscountToStringTest()
        {
            var bread = new Product(1, "Bread", 10);
            var milk = new Product(3, "Milk", 6);

            var discount = new SimpleDiscount(1, bread, 2, milk, 0.6);
            var expectedResult = "Buy 2 breads and get an extra milk at 60% off";
            Assert.AreEqual(expectedResult, discount.ToString());
        }
    }
}