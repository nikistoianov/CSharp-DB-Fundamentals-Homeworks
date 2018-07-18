namespace P01_BillsPaymentSystem.Data.Models
{
    using Attributes;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.PaymentMethods = new HashSet<PaymentMethod>();
        }

        [Key]
        public int UserId { get; set; }

        [MaxLength(50)]
        [Required]
        public string FirstName { get; set; }

        [MaxLength(50)]
        [Required]
        public string LastName { get; set; }

        [MaxLength(80)]
        [Required]
        [NonUnicode]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [MaxLength(25)]
        [Required]
        [NonUnicode]
        public string Password { get; set; }

        public ICollection<PaymentMethod> PaymentMethods { get; set; }
    }
}
