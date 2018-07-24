namespace BookShop
{
    using BookShop.Data;
    using BookShop.Initializer;
    using BookShop.Models;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class StartUp
    {
        public static void Main()
        {
            using (var db = new BookShopContext())
            {
                //DbInitializer.ResetDatabase(db);

                //1. Age Restriction
                //var command = Console.ReadLine();
                //var result = GetBooksByAgeRestriction(db, command);
                //Console.WriteLine(result);

                //2. Golden Books
                //var result = GetGoldenBooks(db);
                //Console.WriteLine(result);

                //3. Books by Price
                //Console.WriteLine(GetBooksByPrice(db));

                //4. Not Released In
                //Console.WriteLine(GetBooksNotRealeasedIn(db, int.Parse(Console.ReadLine())));

                //5. Book Titles by Category
                //Console.WriteLine(GetBooksByCategory(db, Console.ReadLine()));

                //6. Released Before Date
                //Console.WriteLine(GetBooksReleasedBefore(db, Console.ReadLine()));

                //7. Author Search
                //Console.WriteLine(GetAuthorNamesEndingIn(db, Console.ReadLine()));

                //8. Book Search
                //Console.WriteLine(GetBookTitlesContaining(db, Console.ReadLine()));

                //9. Book Search by Author
                //Console.WriteLine(GetBooksByAuthor(db, Console.ReadLine()));

                //10. Count Books
                //Console.WriteLine(CountBooks(db, int.Parse(Console.ReadLine())));

                //11. Total Book Copies
                //Console.WriteLine(CountCopiesByAuthor(db));

                //12. Profit by Category
                //Console.WriteLine(GetTotalProfitByCategory(db));

                //13. Most Recent Books
                //Console.WriteLine(GetMostRecentBooks(db));

                //14. Increase Prices
                //IncreasePrices(db);

                //15. Remove Books
                Console.WriteLine(RemoveBooks(db) + " books were deleted");
            }
        }

        public static string GetBooksByAgeRestriction(BookShopContext context, string command)
        {
            var ageRestriction = (AgeRestriction)Enum.Parse(typeof(AgeRestriction), command, true);
            var books = context.Books
                .Where(x => x.AgeRestriction == ageRestriction)
                .OrderBy(x => x.Title)
                .Select(x => x.Title)
                .ToList();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetGoldenBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 5000 && x.EditionType == EditionType.Gold)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByPrice(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Price > 40)
                .OrderByDescending(x => x.Price)
                .Select(x => $"{x.Title} - ${x.Price:F2}")
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksNotRealeasedIn(BookShopContext context, int year)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year != year)
                .OrderBy(x => x.BookId)
                .Select(x => x.Title)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByCategory(BookShopContext context, string input)
        {
            var categories = input.ToLower().Split(' ', StringSplitOptions.RemoveEmptyEntries);
            
            var books = context.Books
                //.Include(x => x.BookCategories)
                //.ThenInclude(x => x.Category)
                .Where(x => x.BookCategories.Any(c => categories.Contains(c.Category.Name.ToLower())))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksReleasedBefore(BookShopContext context, string date)
        {
            var inputDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            var books = context.Books
                .Where(x => x.ReleaseDate < inputDate)
                .OrderByDescending(x => x.ReleaseDate)
                .Select(x => $"{x.Title} - {x.EditionType} - ${x.Price:F2}")
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetAuthorNamesEndingIn(BookShopContext context, string input)
        {
            var authors = context.Authors
                .Where(x => EF.Functions.Like(x.FirstName, "%" + input))
                //.Where(x => x.FirstName.EndsWith(input))
                .Select(x => $"{x.FirstName} {x.LastName}")
                .OrderBy(x => x)
                .ToArray();
            return string.Join(Environment.NewLine, authors);
        }

        public static string GetBookTitlesContaining(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => EF.Functions.Like(x.Title, "%" + input + "%"))
                //.Where(x => x.Title.ToLower().Contains(input.ToLower()))
                .Select(x => x.Title)
                .OrderBy(x => x)
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static string GetBooksByAuthor(BookShopContext context, string input)
        {
            var books = context.Books
                .Where(x => EF.Functions.Like(x.Author.LastName, input + "%"))
                .OrderBy(x => x.BookId)
                .Select(x => $"{x.Title} ({x.Author.FirstName} {x.Author.LastName})")
                .ToArray();
            return string.Join(Environment.NewLine, books);
        }

        public static int CountBooks(BookShopContext context, int lengthCheck)
        {
            var books = context.Books
                .Where(x => x.Title.Length > lengthCheck)
                .Count();
            return books;
        }

        public static string CountCopiesByAuthor(BookShopContext context)
        {
            var authors = context.Authors
                .Select(x => new
                {
                    FullName = x.FirstName + " " + x.LastName,
                    Copies = x.Books.Sum(c => c.Copies)
                })
                .OrderByDescending(x => x.Copies)
                .ToArray();

            var result = new StringBuilder();
            foreach (var author in authors)
            {
                result.AppendLine($"{author.FullName} - {author.Copies}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetTotalProfitByCategory(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    x.Name,
                    TotalProfit = x.CategoryBooks.Sum(c => c.Book.Price * c.Book.Copies)
                })
                .OrderByDescending(x => x.TotalProfit)
                .ToArray();

            var result = new StringBuilder();
            foreach (var c in categories)
            {
                result.AppendLine($"{c.Name} ${c.TotalProfit}");
            }

            return result.ToString().TrimEnd();
        }

        public static string GetMostRecentBooks(BookShopContext context)
        {
            var categories = context.Categories
                .Select(x => new
                {
                    x.Name,
                    Books = x.CategoryBooks
                        .OrderByDescending(b => b.Book.ReleaseDate)
                        .Select(b => new
                        {
                            b.Book.Title,
                            b.Book.ReleaseDate
                        })
                        .Take(3)
                        .ToArray()
                })
                .OrderBy(x => x.Name)
                .ToArray();

            var result = new StringBuilder();
            foreach (var c in categories)
            {
                result.AppendLine($"--{c.Name}");
                foreach (var b in c.Books)
                {
                    result.AppendLine($"{b.Title} ({b.ReleaseDate.Value.Year})");
                }
            }

            return result.ToString().TrimEnd();
        }

        public static void IncreasePrices(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.ReleaseDate.Value.Year < 2010)
                .ToArray();
            //.Update(x => new Book() { Price = x.Price + 5});

            foreach (var book in books)
            {
                book.Price += 5;
            }
            context.SaveChanges();
        }

        public static int RemoveBooks(BookShopContext context)
        {
            var books = context.Books
                .Where(x => x.Copies < 4200)
                .ToArray();
            context.Books.RemoveRange(books);
            context.SaveChanges();
            return books.Count();
        }
    }
}
