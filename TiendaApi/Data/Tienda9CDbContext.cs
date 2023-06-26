using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TiendaApi.Models;

namespace TiendaApi.Data;

public partial class Tienda9CDbContext : DbContext
{
    public Tienda9CDbContext(DbContextOptions<Tienda9CDbContext> options)
        : base(options)
    {
    }


    public virtual DbSet<Fabricante> Fabricantes { get; set; }

    public virtual DbSet<Producto> Productos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_general_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Fabricante>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PRIMARY");

            entity.ToTable("fabricante");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
        });

        modelBuilder.Entity<Producto>(entity =>
        {
            entity.HasKey(e => e.Codigo).HasName("PRIMARY");

            entity.ToTable("producto");

            entity.HasIndex(e => e.CodigoFabricante, "codigo_fabricante");

            entity.Property(e => e.Codigo).HasColumnName("codigo");
            entity.Property(e => e.CodigoFabricante).HasColumnName("codigo_fabricante");
            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .HasColumnName("nombre");
            entity.Property(e => e.Precio).HasColumnName("precio");

            entity.HasOne(d => d.CodigoFabricanteNavigation).WithMany(p => p.Productos)
                .HasForeignKey(d => d.CodigoFabricante)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("producto_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
