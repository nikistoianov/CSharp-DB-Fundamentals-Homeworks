namespace P01_BillsPaymentSystem.Data.Models
{
    using Attributes;
    using System.ComponentModel.DataAnnotations;

    public class BankAccount
    {
        [Key]
        public int BankAccountId { get; set; }

        [Required]
        public decimal Balance { get; private set; }

        [MaxLength(50)]
        [Required]
        public string BankName { get; set; }

        [MaxLength(20)]
        [Required]
        [NonUnicode]
        public string SwiftCode { get; set; }

        public PaymentMethod PaymentMethod { get; set; }

        public void Withdraw(decimal amount)
        {
            if (amount > 0 && this.Balance >= amount)
            {
                this.Balance -= amount;
            }
        }

        public void Deposit(decimal amount)
        {
            if (amount > 0)
            {
                this.Balance += amount;
            }
        }
    }
}
