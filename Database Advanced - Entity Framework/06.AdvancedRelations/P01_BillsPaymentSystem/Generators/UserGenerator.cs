namespace P01_BillsPaymentSystem.Generators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Generators;
    using Common.Validators;
    using Data;
    using Data.Models;

    public class UserGenerator
    {
        internal static void Seed(BillsPaymentSystemContext context, int count)
        {
            for (int i = 0; i < count; i++)
            {
                var firstName = TextGenerator.FirstName();
                var user = new User()
                {
                    FirstName = firstName,
                    LastName = TextGenerator.LastName(),
                    Email = EmailGenerator.NewEmail(firstName),
                    Password = TextGenerator.Password(12)
                };

                var result = new List<ValidationResult>();
                if (AttributeValidator.IsValid(user, result))
                {
                    context.Users.Add(user);
                }
                else
                {
                    Console.WriteLine(string.Join(Environment.NewLine, result));
                }
            }

            context.SaveChanges();
        }
    }
}
