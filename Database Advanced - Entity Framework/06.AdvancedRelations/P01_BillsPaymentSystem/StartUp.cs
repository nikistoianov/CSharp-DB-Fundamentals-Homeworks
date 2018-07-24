using Microsoft.EntityFrameworkCore;
using P01_BillsPaymentSystem.Data;
using P01_BillsPaymentSystem.Data.Models;
using P01_BillsPaymentSystem.Data.Models.Enums;
using System;
using System.Linq;

namespace P01_BillsPaymentSystem
{
    public class StartUp
    {
        public static void Main()
        {
            DatabaseInitializer.ResetDatabase();
            using (var db = new BillsPaymentSystemContext())
            {
                User user = GetUser(db);
                Console.WriteLine();

                PayBills(user, 50, db);
                Console.WriteLine();

                UserDetails(user);
            }
        }

        private static User GetUser(BillsPaymentSystemContext db)
        {
            User user = null;
            while (true)
            {
                Console.Write("UserId: ");
                var id = int.Parse(Console.ReadLine());
                user = db.Users
                    .Where(x => x.UserId == id)
                    .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.BankAccount)
                    .Include(x => x.PaymentMethods)
                    .ThenInclude(x => x.CreditCard)
                    .FirstOrDefault();

                if (user == null)
                {
                    Console.WriteLine($"User with id {id} not found!");
                }
                else
                {
                    break;
                }
            }

            return user;
        }

        private static void UserDetails(User user)
        {

            Console.WriteLine($"User: {user.FirstName} {user.LastName}");
            Console.WriteLine("Bank Accounts:");
            foreach (var pm in user.PaymentMethods.Where(x => x.BankAccountId != null).OrderBy(x => x.BankAccountId))
            {
                Console.WriteLine($"-- ID: {pm.BankAccountId}");
                Console.WriteLine($"--- Balance: {pm.BankAccount.Balance:F2}");
                Console.WriteLine($"--- Bank: {pm.BankAccount.BankName}");
                Console.WriteLine($"--- SWIFT: {pm.BankAccount.SwiftCode}");
            }

            Console.WriteLine($"Credit Cards:");
            foreach (var pm in user.PaymentMethods.Where(x => x.CreditCardId != null).OrderBy(x => x.CreditCardId))
            {
                Console.WriteLine($"-- ID: {pm.CreditCardId}");
                Console.WriteLine($"--- Limit: {pm.CreditCard.Limit:F2}");
                Console.WriteLine($"--- Money Owed: {pm.CreditCard.MoneyOwed:F2}");
                Console.WriteLine($"--- Limit Left: {pm.CreditCard.LimitLeft:F2}");
                Console.WriteLine($"--- Expiration Date: {pm.CreditCard.ExpirationDate.ToString(@"yyyy\/MM")}");
            }

        }

        private static void PayBills(User user, decimal amount, BillsPaymentSystemContext db)
        {
            var totalFromBanks = user.PaymentMethods.Where(x => x.BankAccountId != null).Sum(x => x.BankAccount.Balance);
            var totalFromCreditCards = user.PaymentMethods.Where(x => x.CreditCardId != null).Sum(x => x.CreditCard.LimitLeft);

            if (totalFromBanks + totalFromCreditCards < amount)
            {
                Console.WriteLine($"Insufficient funds ({totalFromBanks + totalFromCreditCards:F2} < {amount:F2})!");
            }
            else
            {
                while (amount > 0)
                {
                    var bankAccounts = user.PaymentMethods.Where(x => x.BankAccountId != null).Select(x => x.BankAccount).OrderBy(x => x.BankAccountId);
                    foreach (var bankAccount in bankAccounts)
                    {
                        if (amount <= 0)
                            break;

                        var withdraw = Math.Min(amount, bankAccount.Balance);
                        bankAccount.Withdraw(withdraw);
                        amount -= withdraw;
                    }

                    var creditCards = user.PaymentMethods.Where(x => x.CreditCardId != null).Select(x => x.CreditCard).OrderBy(x => x.CreditCardId);
                    foreach (var creditCard in creditCards)
                    {
                        if (amount <= 0)
                            break;

                        var withdraw = Math.Min(amount, creditCard.LimitLeft);
                        creditCard.Withdraw(withdraw);
                        amount -= withdraw;
                    }
                }
                db.SaveChanges();
                Console.WriteLine("Bills payed!");
            }
        }
    }
}
