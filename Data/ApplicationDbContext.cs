using inaApp.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace inaApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<Producto> Producto { get; set; }
        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Categoria> Categoria { get; set; }
        public DbSet<Factura> Factura { get; set; }
        public DbSet<FacturaDetalle> FacturaDetalle { get; set; }
        public DbSet<NotaCredito> NotaCredito { get; set; }
        public DbSet<NotaCreditoDetalle> NotaCreditoDetalle { get; set; }

        //fluent api
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //relacion entre Producto y Categoria
            modelBuilder.Entity<Producto>()
                .HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación Factura - Cliente
            modelBuilder.Entity<Factura>()
                .HasOne(f => f.Cliente)
                .WithMany()
                .HasForeignKey(f => f.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación Factura - FacturaDetalle
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(d => d.Factura)
                .WithMany(f => f.FacturaDetalles)
                .HasForeignKey(d => d.FacturaId)
                .OnDelete(DeleteBehavior.Cascade);

            //relación FacturaDetalle - Producto
            modelBuilder.Entity<FacturaDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación NotaCredito - FacturaOriginal
            modelBuilder.Entity<NotaCredito>()
                .HasOne(n => n.FacturaOriginal)
                .WithMany(f => f.NotasCredito)
                .HasForeignKey(n => n.FacturaOriginalId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación NotaCredito - Cliente
            modelBuilder.Entity<NotaCredito>()
                .HasOne(n => n.Cliente)
                .WithMany()
                .HasForeignKey(n => n.ClienteId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación NotaCredito - NotaCreditoDetalle
            modelBuilder.Entity<NotaCreditoDetalle>()
                .HasOne(d => d.NotaCredito)
                .WithMany(n => n.NotaCreditoDetalles)
                .HasForeignKey(d => d.NotaCreditoId)
                .OnDelete(DeleteBehavior.Cascade);

            //relación NotaCreditoDetalle - FacturaDetalleOriginal
            modelBuilder.Entity<NotaCreditoDetalle>()
                .HasOne(d => d.FacturaDetalleOriginal)
                .WithMany()
                .HasForeignKey(d => d.FacturaDetalleOriginalId)
                .OnDelete(DeleteBehavior.Restrict);

            //relación NotaCreditoDetalle - Producto
            modelBuilder.Entity<NotaCreditoDetalle>()
                .HasOne(d => d.Producto)
                .WithMany()
                .HasForeignKey(d => d.ProductoId)
                .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(modelBuilder);
        }

    } 
}