namespace ProductShop.App
{
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
            ImportData();
            ProductsInRange();
            SoldProducts();
            CategoriesByProductsCount();
            UsersAndProducts();
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

        #region ExportData

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

                var fileName = "products-in-range";
                var jsonString = JsonConvert.SerializeObject(products, Formatting.Indented);
                File.WriteAllText($"../../../Json/{fileName}.json", jsonString);
                Console.WriteLine($"Created file: {fileName}.json");
            }
        }

        private static void SoldProducts()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                    .Where(x => x.ProductsSold.Count > 0 && x.ProductsSold.Any(p => p.BuyerId != null))
                    .Select(x => new
                    {
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        soldProducts = x.ProductsSold
                            .Select(p => new
                            {
                                name = p.Name,
                                price = p.Price,
                                buyerFirstName = p.Buyer.FirstName,
                                buyerLastName = p.Buyer.LastName
                            }).ToArray()
                    })
                    .OrderBy(x => x.lastName)
                    .ThenBy(x => x.firstName)
                    .ToArray();

                var fileName = "users-sold-products";
                var jsonString = JsonConvert.SerializeObject(users, new JsonSerializerSettings
                    {
                        Formatting = Formatting.Indented,
                        NullValueHandling = NullValueHandling.Ignore
                    });
                File.WriteAllText($"../../../Json/{fileName}.json", jsonString);
                Console.WriteLine($"Created file: {fileName}.json");
            }
        }

        private static void CategoriesByProductsCount()
        {
            using (var db = new ProductShopContext())
            {
                var categories = db.Categories
                    .OrderByDescending(x => x.CategoryProducts.Count)
                    .Select(x => new
                    {
                        category = x.Name,
                        productsCount = x.CategoryProducts.Count,
                        averagePrice = x.CategoryProducts.Sum(p => p.Product.Price) / x.CategoryProducts.Count,
                        //averagePrice = x.CategoryProducts.Select(p => p.Product.Price).DefaultIfEmpty(0).Average(),
                        totalRevenue = x.CategoryProducts.Sum(p => p.Product.Price)
                    })
                    .ToArray();

                var fileName = "categories-by-products";
                var jsonString = JsonConvert.SerializeObject(categories, Formatting.Indented);
                File.WriteAllText($"../../../Json/{fileName}.json", jsonString);
                Console.WriteLine($"Created file: {fileName}.json");
            }
        }

        private static void UsersAndProducts()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                    .Where(x => x.ProductsSold.Count > 0)
                    .OrderByDescending(x => x.ProductsSold.Count)
                    .ThenBy(x => x.LastName)
                    .Select(x => new
                    {
                        firstName = x.FirstName,
                        lastName = x.LastName,
                        age = x.Age.ToString(),
                        soldProducts = new
                        {
                            count = x.ProductsSold.Count,
                            products = x.ProductsSold
                                .Select(p => new
                                {
                                    name = p.Name,
                                    price = p.Price
                                }).ToArray()
                        }                        
                    })
                    .ToArray();

                var rootUsers = new
                {
                    usersCount = users.Length,
                    users
                };

                var fileName = "users-and-products";
                var jsonString = JsonConvert.SerializeObject(rootUsers, new JsonSerializerSettings
                {
                    Formatting = Formatting.Indented,
                    NullValueHandling = NullValueHandling.Ignore
                });
                File.WriteAllText($"../../../Json/{fileName}.json", jsonString);
                Console.WriteLine($"Created file: {fileName}.json");
            }
        }

        #endregion
    }
}
