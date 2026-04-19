using Domain.ItensFatura.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infrastructure.Data
{
    public class ItemConfiguration : IEntityTypeConfiguration<ItemFatura>
    {
        public void Configure(EntityTypeBuilder<ItemFatura> builder)
        {

            builder.HasKey(i => i.Id);

            builder.Property(i => i.FaturaId)
                .IsRequired();

            builder.Property(i => i.Descricao)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(i => i.Quantidade)
                .IsRequired();

            builder.Property(i => i.ValorUnitario)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.ValorTotal)
                .HasColumnType("decimal(18,2)");

            builder.Property(i => i.Justificativa)
                .HasMaxLength(1000);

        }
    }
}
