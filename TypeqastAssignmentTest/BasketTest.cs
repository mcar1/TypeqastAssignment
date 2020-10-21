using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Schema;
using TypeqastAssignment;

namespace TypeqastAssignmentTest
{
    public class BasketTest
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void AddItemToBasketTest()
        {
            var productRepository = new Mock<IRepository<Product>>();
            var discountRepository = new Mock<IRepository<IDiscount>>();
            var productList = new List<Product>();
            var discountList = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            productList.Add(bread);
            productList.Add(butter);
            productList.Add(milk);

            discountList.Add(new SimpleDiscount(1, bread, 2, bread, 0.5));
            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            discountList.Add(new ComplexDiscount(2, requiredProducts, discountedProducts));

            productRepository.Setup(x => x.GetAll()).Returns(() => productList);
            discountRepository.Setup(x => x.GetAll()).Returns(() => discountList);

            var basket = new Basket(productRepository.Object, discountRepository.Object);
            var expectedItems = new List<int>() { 1, 2, 2 };
            basket.AddItem(1);
            basket.AddItem(2);
            basket.AddItem(2);
            Assert.AreEqual(basket.PurchasedItems, expectedItems);
            Assert.Pass();
        }

        [Test]
        public void GetOrderedDiscountsTest()
        {
            var productRepository = new Mock<IRepository<Product>>();
            var discountRepository = new Mock<IRepository<IDiscount>>();
            var productList = new List<Product>();
            var discountList = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            productList.Add(bread);
            productList.Add(butter);
            productList.Add(milk);

            var discount1 = new SimpleDiscount(1, bread, 2, bread, 0.5);
            discountList.Add(discount1);
            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            var discount2 = new ComplexDiscount(2, requiredProducts, discountedProducts);
            discountList.Add(discount2);
            var discount3 = new SimpleDiscount(3, butter, 3, milk, 0.2);
            discountList.Add(discount3);

            productRepository.Setup(x => x.GetAll()).Returns(() => productList);
            discountRepository.Setup(x => x.GetAll()).Returns(() => discountList);

            var basket = new Basket(productRepository.Object, discountRepository.Object);
            var expectedResults = new List<IDiscount>() { discount2, discount1, discount3 };
            Assert.AreEqual(basket.GetOrderedDisounts(), expectedResults);
            Assert.Pass();
        }

        [Test]
        public void GetDiscountedItemsTest()
        {
            var productRepository = new Mock<IRepository<Product>>();
            var discountRepository = new Mock<IRepository<IDiscount>>();
            var productList = new List<Product>();
            var discountList = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            productList.Add(bread);
            productList.Add(butter);
            productList.Add(milk);

            var simpleDiscount = new SimpleDiscount(1, bread, 2, bread, 0.5);
            discountList.Add(simpleDiscount);
            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            var complexDiscount = new ComplexDiscount(2, requiredProducts, discountedProducts);
            discountList.Add(complexDiscount);

            productRepository.Setup(x => x.GetAll()).Returns(() => productList);
            discountRepository.Setup(x => x.GetAll()).Returns(() => discountList);
            var basket = new Basket(productRepository.Object, discountRepository.Object);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(2);
            basket.AddItem(2);
            basket.AddItem(3);
            basket.AddItem(3);
            
            var purchasedItems = new List<int>(basket.PurchasedItems);
            var usedDiscounts = new List<IDiscount>();
            var calculatedItems = basket.GetDiscountedItems(usedDiscounts, purchasedItems);
            var expectedUsedDiscounts = new List<IDiscount>() { complexDiscount, simpleDiscount };
            var expectedItemsLeftToCalculate = new List<int>() { 2 };
            var expectedCalculatedItems = new Dictionary<int, double>() { { 2, 0 },{ 1, 41 }, { 3, 12 } };
            Assert.AreEqual(expectedUsedDiscounts, usedDiscounts);
            Assert.AreEqual(expectedItemsLeftToCalculate, purchasedItems);
            Assert.AreEqual(calculatedItems, expectedCalculatedItems);
            Assert.Pass();
        }

        [Test]
        public void GetBillTest()
        {
            var productRepository = new Mock<IRepository<Product>>();
            var discountRepository = new Mock<IRepository<IDiscount>>();
            var productList = new List<Product>();
            var discountList = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            productList.Add(bread);
            productList.Add(butter);
            productList.Add(milk);

            var simpleDiscount = new SimpleDiscount(1, bread, 2, bread, 0.5);
            discountList.Add(simpleDiscount);
            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            var complexDiscount = new ComplexDiscount(2, requiredProducts, discountedProducts);
            discountList.Add(complexDiscount);

            productRepository.Setup(x => x.GetAll()).Returns(() => productList);
            discountRepository.Setup(x => x.GetAll()).Returns(() => discountList);
            var basket = new Basket(productRepository.Object, discountRepository.Object);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(2);
            basket.AddItem(3);
            basket.AddItem(3);

            var usedDiscounts = new List<IDiscount>() { complexDiscount };
            var expectedItemsLeftToCalculate = new List<int>() { 1 };
            var boughtItems = new Dictionary<int, double>() { { 2, 0 }, { 1, 26 }, { 3, 12 } };
            var total = boughtItems.Sum(x => x.Value);
            var expectedBill = "Bread x 3 = $26\nButter x 1 = $0\nMilk x 2 = $12\nTotal: $38\nDiscounts:\nBuy 2 milks and 1 bread and get 1 butter for free and an extra bread at 40% off\n";
            Assert.AreEqual(expectedBill,basket.GetBill(usedDiscounts, boughtItems, total));
            Assert.Pass();
        }

        [Test]
        public void CalculateRemainingProductsPriceTest()
        {
            var productRepository = new Mock<IRepository<Product>>();
            var discountRepository = new Mock<IRepository<IDiscount>>();
            var productList = new List<Product>();
            var discountList = new List<IDiscount>();
            var bread = new Product(1, "Bread", 10);
            var butter = new Product(2, "Butter", 2);
            var milk = new Product(3, "Milk", 6);

            productList.Add(bread);
            productList.Add(butter);
            productList.Add(milk);

            var simpleDiscount = new SimpleDiscount(1, bread, 2, bread, 0.5);
            discountList.Add(simpleDiscount);
            var requiredProducts = new List<Tuple<Product, int>>();
            var discountedProducts = new List<Tuple<Product, double>>();
            requiredProducts.Add(new Tuple<Product, int>(milk, 2));
            requiredProducts.Add(new Tuple<Product, int>(bread, 1));
            discountedProducts.Add(new Tuple<Product, double>(butter, 1));
            discountedProducts.Add(new Tuple<Product, double>(bread, 0.4));
            var complexDiscount = new ComplexDiscount(2, requiredProducts, discountedProducts);
            discountList.Add(complexDiscount);

            productRepository.Setup(x => x.GetAll()).Returns(() => productList);
            discountRepository.Setup(x => x.GetAll()).Returns(() => discountList);
            var basket = new Basket(productRepository.Object, discountRepository.Object);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(1);
            basket.AddItem(2);
            basket.AddItem(3);
            basket.AddItem(3);

            var itemsLeftToCalculate = new List<int>() { 1 };
            var boughtItems = new Dictionary<int, double>() { { 2, 0 }, { 1, 26 }, { 3, 12 } };
            var result = basket.CalculateRemainingProductsPrice(itemsLeftToCalculate, boughtItems);
            Assert.AreEqual(10, result);
            Assert.Pass();
        }
    }
}