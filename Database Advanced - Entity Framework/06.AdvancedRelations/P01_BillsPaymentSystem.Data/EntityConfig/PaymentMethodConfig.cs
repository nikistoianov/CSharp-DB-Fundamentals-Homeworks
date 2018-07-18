namespace P01_BillsPaymentSystem.Data.EntityConfig
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Metadata.Builders;
    using Models;
    using P01_BillsPaymentSystem.Data.Models.Enums;
    using System;

    public class PaymentMethodConfig : IEntityTypeConfiguration<PaymentMethod>
    {
        public void Configure(EntityTypeBuilder<PaymentMethod> builder)
        {
            builder
                .HasOne(x => x.BankAccount)
                .WithOne(x => x.PaymentMethod)
                .HasForeignKey<BankAccount>(x => x.BankAccountId);

            builder
                .HasOne(x => x.CreditCard)
                .WithOne(x => x.PaymentMethod)
                .HasForeignKey<CreditCard>(x => x.CreditCardId);

            builder
                .Property(x => x.Type)
                .HasConversion(x => x.ToString(), x => (PaymentType)Enum.Parse(typeof(PaymentType), x))
                .HasColumnType("varchar(20)");
        }
    }
}
