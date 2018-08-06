namespace ProductShop.App
{
    using AutoMapper;

    using System;
    using System.Collections.Generic;
    using DA = System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Xml;
    using System.Xml.Serialization;
    using System.Linq;
    using System.Text;

    using Dto;
    using Data;
    using Models;

    using Dto.Export;

    class Startup
    {
        static void Main()
        {
            ImportData();
            ProductsInRange();
            //SoldProducts();
            //CategoriesByProductsCount();
            //UsersAndProducts();
        }

        #region ImportData

        private static void ImportData()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<ProductShopProfile>();
            });
            var mapper = config.CreateMapper();

            var users = ImportUsers(mapper, "users.xml");
            var products = ImportProducts(mapper, "products.xml", users);
            var categories = ImportCategories(mapper, "categories.xml");
            GenerateCategoriesProducts(products, categories);

            Console.WriteLine($"Imorted Users: {users.Count}, Products: {products.Count}, Categories: {categories.Count}");
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
                db.CategoriesProducts.AddRange(categoriesProducts);
                db.SaveChanges();
            }
        }

        private static List<User> ImportUsers(IMapper mapper, string xmlName)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(UserDto[]), new XmlRootAttribute("users"));
            var userDtos = (UserDto[])serializer.Deserialize(new StringReader(xmlString));

            var users = new List<User>();

            foreach (var userDto in userDtos)
            {
                if (IsValid(userDto))
                {
                    var user = mapper.Map<User>(userDto);
                    users.Add(user);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Users.AddRange(users);
                db.SaveChanges();
            }

            return users;
        }

        private static List<Product> ImportProducts(IMapper mapper, string xmlName, List<User> users)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(ProductDto[]), new XmlRootAttribute("products"));
            var productDtos = (ProductDto[])serializer.Deserialize(new StringReader(xmlString));

            var products = new List<Product>();

            foreach (var productDto in productDtos)
            {
                if (IsValid(productDto))
                {
                    var product = mapper.Map<Product>(productDto);
                    var sellerId = new Random().Next(0, users.Count / 2);
                    var buyerId = new Random().Next(users.Count / 2 + 1, users.Count);
                    var addBuyer = Convert.ToBoolean(new Random().Next(0, 2));

                    product.SellerId = users[sellerId].Id;
                    if (addBuyer)
                    {
                        product.BuyerId = users[buyerId].Id;
                    }

                    products.Add(product);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Products.AddRange(products);
                db.SaveChanges();
            }

            return products;
        }

        private static List<Category> ImportCategories(IMapper mapper, string xmlName)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(CategoryDto[]), new XmlRootAttribute("categories"));
            var categoryDtos = (CategoryDto[])serializer.Deserialize(new StringReader(xmlString));

            var categories = new List<Category>();

            foreach (var categoryDto in categoryDtos)
            {
                if (IsValid(categoryDto))
                {
                    var category = mapper.Map<Category>(categoryDto);
                    categories.Add(category);
                }
            }

            using (var db = new ProductShopContext())
            {
                db.Categories.AddRange(categories);
                db.SaveChanges();
            }

            return categories;
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
                    .Where(x => x.Price >= 1000 && x.Price <= 2000 && x.BuyerId != null)
                    .OrderBy(x => x.Price)
                    .Select(x => new ProductsInRangeDto()
                    {
                        Name = x.Name,
                        Price = x.Price,
                        BuyerFullName = x.Buyer.FirstName + " " + x.Buyer.LastName ?? x.Buyer.LastName
                    })
                    .ToArray();

                var sb = new StringBuilder();
                var serializer = new XmlSerializer(typeof(ProductsInRangeDto[]), new XmlRootAttribute("products"));
                var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(new StringWriter(sb), products, namespaces);
                File.WriteAllText("../../../Xml/products-in-range.xml", sb.ToString());
            }
        }

        private static void SoldProducts()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                    .Where(x => x.SoldProducts.Count > 0)
                    .Select(x => new UserNamesDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        SoldProducts = x.SoldProducts
                            .Select(p => new ProductDto()
                            {
                                Name = p.Name,
                                Price = p.Price
                            }).ToArray()
                    })
                    .OrderBy(x => x.LastName)
                    .ThenBy(x => x.FirstName)
                    .ToArray();

                var sb = new StringBuilder();
                var serializer = new XmlSerializer(typeof(UserNamesDto[]), new XmlRootAttribute("users"));
                var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(new StringWriter(sb), users, namespaces);
                File.WriteAllText("../../../Xml/users-sold-products.xml", sb.ToString());
            }
        }

        private static void CategoriesByProductsCount()
        {
            using (var db = new ProductShopContext())
            {
                var categories = db.Categories
                    .OrderByDescending(x => x.CategoriesProducts.Count)
                    .Select(x => new CategoryByProductDto()
                    {
                        Name = x.Name,
                        ProductCount = x.CategoriesProducts.Count,
                        AveragePrice = x.CategoriesProducts.Average(p => p.Product.Price),
                        TotalRevenue = x.CategoriesProducts.Sum(p => p.Product.Price)
                    })
                    .ToArray();

                var sb = new StringBuilder();
                var serializer = new XmlSerializer(typeof(CategoryByProductDto[]), new XmlRootAttribute("categories"));
                var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(new StringWriter(sb), categories, namespaces);
                File.WriteAllText("../../../Xml/categories-by-products.xml", sb.ToString());
            }
        }

        private static void UsersAndProducts()
        {
            using (var db = new ProductShopContext())
            {
                var users = db.Users
                    .Where(x => x.SoldProducts.Count > 0)
                    .OrderByDescending(x => x.SoldProducts.Count)
                    .ThenBy(x => x.LastName)
                    .Select(x => new UserSoldDto()
                    {
                        FirstName = x.FirstName,
                        LastName = x.LastName,
                        Age = x.Age.ToString(),
                        SoldProducts = x.SoldProducts
                            .Select(p => new ProductSoldDto()
                            {
                                Name = p.Name,
                                Price = p.Price
                            }).ToArray()
                    })
                    .ToArray();

                var rootUsers = new UsersDto()
                {
                    Count = users.Length,
                    Users = users
                };

                var sb = new StringBuilder();
                var serializer = new XmlSerializer(typeof(UsersDto));
                var namespaces = new XmlSerializerNamespaces(new[] { XmlQualifiedName.Empty });
                serializer.Serialize(new StringWriter(sb), rootUsers, namespaces);
                File.WriteAllText("../../../Xml/users-and-products.xml", sb.ToString());
            }
        }
    }
}
