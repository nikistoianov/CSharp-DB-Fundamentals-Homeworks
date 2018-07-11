using P03_SalesDatabase.Data;
using P03_SalesDatabase.Data.Models;

namespace P03_SalesDatabase
{
    class StartUp
    {
        static void Main()
        {
            using (var db = new SalesContext())
            {
                var sale = new Sale()
                {
                    Product = new Product()
                    {
                        Name = "Tv",
                        Price = 19.99m
                    },
                    Customer = new Customer()
                    {
                        Name = "Penka"
                    },
                    Store = new Store()
                    {
                        Name = "TechLux"
                    }
                };
                db.Sales.Add(sale);
                //db.Database.EnsureCreated();
                //var product = db.Products.Find(2);
                //product.Name = "Changed";
                db.SaveChanges();
                //Console.WriteLine(product.Name);
            }
        }
    }
}
