namespace ProductShop.App
{
    using AutoMapper;

    using Data;
    using Models;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DA = System.ComponentModel.DataAnnotations;

    public class StartUp
    {
        public static void Main(string[] args)
        {
            //ImportData();
            ProductsInRange();
        }

        #region ImportData

        private static void ImportData()
        {
            var users = ImportUsers("users");
            var products = ImportProducts("products", users);
            var categories = ImportCategories("categories");
            GenerateCategoriesProducts(products, categories);

            Console.WriteLine($"Imorted Users: {users.Count}, Products: {products.Count}, Categories: {categories.Count}");
        }

        private static List<User> ImportUsers(string jsonName)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonUsers = JsonConvert.DeserializeObject<User[]>(jsonString);

            var users = new List<User>();
            foreach (var jsonUser in jsonUsers)
            {
                if (IsValid(jsonUser))
                {
                    users.Add(jsonUser);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            return users;
        }

        private static List<Product> ImportProducts(string jsonName, List<User> users)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonProducts = JsonConvert.DeserializeObject<Product[]>(jsonString);

            var products = new List<Product>();
            foreach (var jsonProduct in jsonProducts)
            {
                if (IsValid(jsonProduct))
                {
                    var sellerId = new Random().Next(0, users.Count / 2);
                    var buyerId = new Random().Next(users.Count / 2 + 1, users.Count);
                    var addBuyer = Convert.ToBoolean(new Random().Next(0, 2));

                    jsonProduct.SellerId = users[sellerId].Id;
                    if (addBuyer)
                    {
                        jsonProduct.BuyerId = users[buyerId].Id;
                    }

                    products.Add(jsonProduct);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Products.AddRange(products);
                db.SaveChanges();
            }

            return products;
        }

        private static List<Category> ImportCategories(string jsonName)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonCategories = JsonConvert.DeserializeObject<Category[]>(jsonString);

            var categories = new List<Category>();
            foreach (var jsonCategory in jsonCategories)
            {
                if (IsValid(jsonCategory))
                {
                    categories.Add(jsonCategory);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            return categories;
        }

        private static void GenerateCategoriesProducts(List<Product> products, List<Category> categories)
        {
            var categoriesProducts = new List<CategoryProduct>();
            foreach (var product in products)
            {
                var categoryId = new Random().Next(0, categories.Count);
                var categoryProduct = new CategoryProduct() { ProductId = product.Id, CategoryId = categories[categoryId].Id };
                categoriesProducts.Add(categoryProduct);
            }

            using (var db = new ProductShopContext())
            {
                db.CategoryProducts.AddRange(categoriesProducts);
                db.SaveChanges();
            }
        }
        
        public static bool IsValid(object obj)
        {
            var validationContext = new DA.ValidationContext(obj);
            var result = new List<DA.ValidationResult>();
            var isValid = DA.Validator.TryValidateObject(obj, validationContext, result, true);

            return isValid;
        }

        #endregion

        private static void ProductsInRange()
        {
            using (var db = new ProductShopContext())
            {
                var products = db.Products
                    .Where(x => x.Price >= 500 && x.Price <= 1000)
                    .OrderBy(x => x.Price)
                    .Select(x => new
                    {
                        name = x.Name,
                        price = x.Price,
                        seller = x.Seller.FirstName + " " + x.Seller.LastName ?? x.Seller.LastName
                    })
                    .ToArray();

                var jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);
                File.WriteAllText("../../../Json/products-in-range.json", jsonString);
            }
        }

    }
}
