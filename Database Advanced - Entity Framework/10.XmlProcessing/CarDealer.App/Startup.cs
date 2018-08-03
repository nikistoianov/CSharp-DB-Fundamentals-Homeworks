using AutoMapper;
using CarDealer.App.Dto.Import;
using CarDealer.Data;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using DA = System.ComponentModel.DataAnnotations;

namespace CarDealer.App
{
    class Startup
    {
        static void Main()
        {
            ImportData();
        }

        private static void ImportData()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<CarDealerProfile>();
            });
            var mapper = config.CreateMapper();

            var suppliers = ImportSuppliers(mapper, "suppliers.xml");
            var parts = ImportParts(mapper, "parts.xml", suppliers);
            var cars = ImportCars(mapper, "cars.xml", parts);
            GenerateCarsParts(cars, parts);

            Console.WriteLine($"Imorted Suppliers: {suppliers.Count}, Parts: {parts.Count}, Cars: {cars.Count}");
        }

        private static List<Supplier> ImportSuppliers(IMapper mapper, string xmlName)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(SupplierDto[]), new XmlRootAttribute("suppliers"));
            var supplierDtos = (SupplierDto[])serializer.Deserialize(new StringReader(xmlString));

            var suppliers = new List<Supplier>();

            foreach (var supplierDto in supplierDtos)
            {
                if (IsValid(supplierDto))
                {
                    var user = mapper.Map<Supplier>(supplierDto);
                    suppliers.Add(user);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Suppliers.AddRange(suppliers);
                db.SaveChanges();
            }

            return suppliers;
        }

        private static List<Part> ImportParts(IMapper mapper, string xmlName, List<Supplier> suppliers)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(PartDto[]), new XmlRootAttribute("parts"));
            var partDtos = (PartDto[])serializer.Deserialize(new StringReader(xmlString));

            var parts = new List<Part>();

            foreach (var partDto in partDtos)
            {
                if (IsValid(partDto))
                {
                    var part = mapper.Map<Part>(partDto);
                    var supplierId = new Random().Next(0, suppliers.Count);

                    part.SupplierId = suppliers[supplierId].Id;

                    parts.Add(part);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Parts.AddRange(parts);
                db.SaveChanges();
            }

            return parts;
        }

        private static List<Car> ImportCars(IMapper mapper, string xmlName, List<Part> parts)
        {
            var xmlString = File.ReadAllText("../../../Xml/" + xmlName);

            var serializer = new XmlSerializer(typeof(CarDto[]), new XmlRootAttribute("cars"));
            var carDtos = (CarDto[])serializer.Deserialize(new StringReader(xmlString));

            var cars = new List<Car>();

            foreach (var carDto in carDtos)
            {
                if (IsValid(carDto))
                {
                    var car = mapper.Map<Car>(carDto);
                    //for (int i = 10; i < new Random().Next(10,21); i++)
                    //{
                    //    var partId = new Random().Next(0, parts.Count);
                    //    car.PartsCars.Add(new PartCar() { PartId = parts[partId].Id });
                    //}                    

                    cars.Add(car);
                }
            }

            using (var db = new CarDealerContext())
            {
                db.Cars.AddRange(cars);
                db.SaveChanges();
            }

            return cars;
        }

        private static void GenerateCarsParts(List<Car> cars, List<Part> parts)
        {
            var partsCars = new List<PartCar>();
            foreach (var car in cars)
            {
                for (int i = 10; i < new Random().Next(10, 21); i++)
                {
                    var partId = new Random().Next(0, parts.Count);
                    partsCars.Add(new PartCar() { PartId = parts[partId].Id, CarId = car.Id });
                }
            }

            using (var db = new CarDealerContext())
            {
                db.PartsCars.AddRange(partsCars);
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

    }
}
