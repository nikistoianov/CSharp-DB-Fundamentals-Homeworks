namespace P01_BillsPaymentSystem.Generators
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Common.Generators;
    using Common.Validators;
    using Data;
    using Data.Models;
    using Data.Models.Enums;

    public class PaymentMethodGenerator
    {
        internal static void Seed(BillsPaymentSystemContext context, int count, List<User> users)
        {
            for (int i = 0; i < count; i++)
            {
                var payment = new PaymentMethod()
                {
                    User = users[IntGenerator.GenerateInt(0, users.Count - 1)],
                    Type = PaymentType.BankAccount,
                    BankAccount = new BankAccount()
                    {
                        BankAccountId = i,
                        //Balance = PriceGenerator.GeneratePrice(),
                        BankName = TextGenerator.FirstName() + "\'s Bank",
                        SwiftCode = TextGenerator.Password(10)
                    },
                    BankAccountId = i
                };
                payment.BankAccount.Deposit(PriceGenerator.GeneratePrice());

                var result = new List<ValidationResult>();
                if (AttributeValidator.IsValid(payment, result))
                {
                    context.PaymentMethods.Add(payment);
                }
                else
                {
                    Console.WriteLine(string.Join(Environment.NewLine, result));
                }

                payment = new PaymentMethod()
                {
                    User = users[IntGenerator.GenerateInt(0, users.Count - 1)],
                    Type = PaymentType.CreditCard,
                    CreditCard = new CreditCard()
                    {
                        CreditCardId = i,
                        ExpirationDate = DateGenerator.FutureDate(),
                        Limit = PriceGenerator.GeneratePrice(),
                        //MoneyOwed = PriceGenerator.GeneratePrice()
                    },
                    CreditCardId = i
                };
                payment.CreditCard.Withdraw(PriceGenerator.GeneratePrice());

                result = new List<ValidationResult>();
                if (AttributeValidator.IsValid(payment, result))
                {
                    context.PaymentMethods.Add(payment);
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
