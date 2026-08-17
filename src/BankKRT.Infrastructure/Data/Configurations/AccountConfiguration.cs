using BankKRT.Domain.Entities;
using BankKRT.Domain.ValueObjects;
using BankKRT.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BankKRT.Infrastructure.Data.Configurations;

public class AccountConfiguration : IEntityTypeConfiguration<Account>
{
    public void Configure(EntityTypeBuilder<Account> builder)
    {
        builder.ToTable("accounts");

        builder.HasKey(a => a.Id);
        builder.Property(a => a.Id)
            .HasColumnName("id")
            .ValueGeneratedOnAdd();

        builder.Property(a => a.HolderName)
            .HasColumnName("holder_name")
            .IsRequired()
            .HasMaxLength(150);

        builder.Property(a => a.Cpf)
            .HasColumnName("cpf")
            .IsRequired()
            .HasMaxLength(11)
            .HasConversion(
                v => (string)v,
                v => CPF.Create(v));

        builder.HasIndex(a => a.Cpf)
            .IsUnique();

        builder.Property(a => a.Status)
            .HasColumnName("status")
            .IsRequired()
            .HasConversion<string>();

        builder.Property(a => a.CreatedAt)
            .HasColumnName("created_at")
            .IsRequired();

        builder.Property(a => a.UpdatedAt)
            .HasColumnName("updated_at");
    }
}
