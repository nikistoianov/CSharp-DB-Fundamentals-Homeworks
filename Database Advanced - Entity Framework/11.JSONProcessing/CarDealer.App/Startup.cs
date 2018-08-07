namespace CarDealer.App
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using DA = System.ComponentModel.DataAnnotations;

    using Data;
    using Models;

    using Newtonsoft.Json;

    class Startup
    {
        static void Main()
        {
            ImportData();
            OrderedCustomers();
            CarsFromMakeToyota();
            LocalSuppliers();
            CarsWithTheirListOfParts();
            TotalSalesByCustomer();
            SalesWithAppliedDiscount();
        }

        #region ImportData

        private static void ImportData()
        {
            var suppliers = ImportSuppliers("suppliers");
            var parts = ImportParts("parts", suppliers);
            var cars = ImportCars("cars", parts);
            var customers = ImportCustomers("customers");
            GenerateSales(cars, customers);

            Console.WriteLine($"Imorted Suppliers: {suppliers.Count}, Parts: {parts.Count}, Cars: {cars.Count}, Customers: {customers.Count}");
        }

        private static List<Supplier> ImportSuppliers(string jsonName)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonSuppliers = JsonConvert.DeserializeObject<Supplier[]>(jsonString);

            var suppliers = new List<Supplier>();

            foreach (var jsonSupplier in jsonSuppliers)
            {
                if (IsValid(jsonSupplier))
                {
                    suppliers.Add(jsonSupplier);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Suppliers.AddRange(suppliers);
                db.SaveChanges();
            }

            return suppliers;
        }

        private static List<Part> ImportParts(string jsonName, List<Supplier> suppliers)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonParts = JsonConvert.DeserializeObject<Part[]>(jsonString);

            var parts = new List<Part>();

            foreach (var jsonPart in jsonParts)
            {
                if (IsValid(jsonPart))
                {
                    var supplierId = new Random().Next(0, suppliers.Count);
                    jsonPart.SupplierId = suppliers[supplierId].Id;

                    parts.Add(jsonPart);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Parts.AddRange(parts);
                db.SaveChanges();
            }

            return parts;
        }

        private static List<Car> ImportCars(string jsonName, List<Part> parts)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonCars = JsonConvert.DeserializeObject<Car[]>(jsonString);

            var cars = new List<Car>();

            foreach (var jsonCar in jsonCars)
            {
                if (IsValid(jsonCar))
                {
                    var partIds = new List<int>(parts.Select(x => x.Id).ToArray());
                    for (int i = 0; i < new Random().Next(10, 21); i++)
                    {
                        var partId = new Random().Next(0, partIds.Count);
                        jsonCar.PartsCars.Add(new PartCar() { PartId = partIds[partId] });
                        partIds.RemoveAt(partId);
                    }

                    cars.Add(jsonCar);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Cars.AddRange(cars);
                db.SaveChanges();
            }

            return cars;
        }

        private static List<Customer> ImportCustomers(string jsonName)
        {
            var jsonString = File.ReadAllText("../../../Json/" + jsonName + ".json");
            var jsonCustomers = JsonConvert.DeserializeObject<Customer[]>(jsonString);

            var customers = new List<Customer>();

            foreach (var jsonCustomer in jsonCustomers)
            {
                if (IsValid(jsonCustomer))
                {
                    customers.Add(jsonCustomer);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Customers.AddRange(customers);
                db.SaveChanges();
            }

            return customers;
        }

        private static void GenerateSales(List<Car> cars, List<Customer> customers)
        {
            var sales = new List<Sale>();
            int[] discounts = { 0, 5, 10, 15, 20, 30, 40, 50 };
            for (int i = 0; i < 50; i++)
            {
                var carId = new Random().Next(0, cars.Count);
                var customerId = new Random().Next(0, customers.Count);
                var discountId = new Random().Next(0, discounts.Length);

                sales.Add(new Sale()
                {
                    CarId = cars[carId].Id,
                    CustomerId = customers[customerId].Id,
                    Discount = discounts[discountId]
                });
            }

            using (var db = new CarDealerContext())
            {
                db.Sales.AddRange(sales);
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

        private static void OrderedCustomers()
        {
            using (var db = new CarDealerContext())
            {
                var customers = db.Customers
                    .OrderBy(x => x.BirthDate)
                    .ThenBy(x => x.IsYoungDriver)
                    .ToArray();

                CreateFile("ordered-customers", customers);
            }
        }

        private static void CarsFromMakeToyota()
        {
            using (var db = new CarDealerContext())
            {
                var cars = db.Cars
                    .Where(x => x.Make == "Toyota")
                    .Select(x => new
                    {
                        x.Id,
                        x.Make,
                        x.Model,
                        x.TravelledDistance
                    })
                    .OrderBy(x => x.Model)
                    .ThenBy(x => x.TravelledDistance)
                    .ToArray();

                CreateFile("toyota-cars", cars);
            }
        }

        private static void LocalSuppliers()
        {
            using (var db = new CarDealerContext())
            {
                var suppliers = db.Suppliers
                    .Where(x => x.IsImporter == false)
                    .Select(x => new
                    {
                        x.Id,
                        x.Name,
                        PartsCount = x.Parts.Count
                    })
                    .ToArray();

                CreateFile("local-suppliers", suppliers);
            }
        }

        private static void CarsWithTheirListOfParts()
        {
            using (var db = new CarDealerContext())
            {
                var cars = db.Cars
                    .Select(x => new
                    {
                        car = new
                        {
                            x.Make,
                            x.Model,
                            x.TravelledDistance
                        },
                        parts = x.PartsCars.Select(p => new
                        {
                            p.Part.Name,
                            p.Part.Price
                        })
                    })
                    .ToArray();

                CreateFile("cars-and-parts", cars);
            }
        }

        private static void TotalSalesByCustomer()
        {
            using (var db = new CarDealerContext())
            {
                var customers = db.Customers
                    .Where(x => x.Sales.Count > 0)
                    .Select(x => new
                    {
                        fullName = x.Name,
                        boughtCars = x.Sales.Count,
                        spentMoney = x.Sales.Select(s => s.Car.PartsCars.Select(p => p.Part.Price).Sum()).Sum()
                    })
                    .OrderByDescending(x => x.spentMoney)
                    .ThenByDescending(x => x.boughtCars)
                    .ToArray();

                CreateFile("customers-total-sales", customers);
            }
        }

        private static void SalesWithAppliedDiscount()
        {
            using (var db = new CarDealerContext())
            {
                var sales = db.Sales
                    .Select(x => new
                    {
                        car = new
                        {
                            x.Car.Make,
                            x.Car.Model,
                            x.Car.TravelledDistance
                        },
                        customerName = x.Customer.Name,
                        Discount = (double)x.Discount / 100,
                        price = x.Car.PartsCars.Select(p => p.Part.Price).Sum(),
                        priceWithDiscount = x.Car.PartsCars.Select(p => p.Part.Price).Sum() * (decimal)(100 - x.Discount) / 100
                    })
                    .ToArray();

                CreateFile("sales-discounts", sales);
            }
        }

        private static void CreateFile(string fileName, object value)
        {
            var jsonString = JsonConvert.SerializeObject(value, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented,
                NullValueHandling = NullValueHandling.Ignore
            });
            File.WriteAllText($"../../../Json/{fileName}.json", jsonString);
            Console.WriteLine($"Created file: {fileName}.json");
        }

        #endregion
    }
}
