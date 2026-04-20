using Domain.Faturas.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data
{
    public class FaturaConfiguration : IEntityTypeConfiguration<Fatura>
    {
        public void Configure(EntityTypeBuilder<Fatura> builder)
        {

            builder.HasKey(f => f.Id);

            builder.Property(f => f.Id)
                .ValueGeneratedNever();

            builder.Property(f => f.Numero)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(f => f.NomeCliente)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(f => f.DataEmissao)
                .IsRequired(false); 

            builder.Property(f => f.Status)
                .IsRequired();

            builder.Property(f => f.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.HasMany(f => f.ItensFatura)
                .WithOne()
                .HasForeignKey(i => i.FaturaId) 
                .OnDelete(DeleteBehavior.Cascade);

            builder.Navigation(f => f.ItensFatura)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
