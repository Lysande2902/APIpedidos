using System;
using System.Collections.Generic;
using System.Linq;
using APIPedidos.Models;
using Microsoft.EntityFrameworkCore;

namespace APIPedidos.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Product
        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.Price).HasColumnType("decimal(18,2)");
            entity.Property(e => e.StockQuantity).IsRequired();

            // Índice único para el nombre del producto
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configuración de Order
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.Property(e => e.State).IsRequired();

            // Relación uno a muchos con OrderItem
            entity
                .HasMany(e => e.Items)
                .WithOne()
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configuración de OrderItem
        modelBuilder.Entity<OrderItem>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Quantity).IsRequired();
            entity.Property(e => e.UnitPrice).HasColumnType("decimal(18,2)").IsRequired();

            // Relación muchos a uno con Product
            entity
                .HasOne(e => e.Product)
                .WithMany()
                .HasForeignKey(e => e.ProductId)
                .OnDelete(DeleteBehavior.Restrict); // No permitir eliminar productos que están en órdenes

            // Relación muchos a uno con Order
            entity
                .HasOne<Order>()
                .WithMany(e => e.Items)
                .HasForeignKey(e => e.OrderId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Datos iniciales para productos
        modelBuilder
            .Entity<Product>()
            .HasData(
                new Product
                {
                    Id = 1,
                    Name = "Producto 001",
                    Description = "Descripción del producto 001",
                    Price = 10.99m,
                    StockQuantity = 100,
                },
                new Product
                {
                    Id = 2,
                    Name = "Producto 002",
                    Description = "Descripción del producto 002",
                    Price = 11.99m,
                    StockQuantity = 101,
                },
                new Product
                {
                    Id = 3,
                    Name = "Producto 003",
                    Description = "Descripción del producto 003",
                    Price = 12.99m,
                    StockQuantity = 102,
                },
                new Product
                {
                    Id = 4,
                    Name = "Producto 004",
                    Description = "Descripción del producto 004",
                    Price = 13.99m,
                    StockQuantity = 103,
                },
                new Product
                {
                    Id = 5,
                    Name = "Producto 005",
                    Description = "Descripción del producto 005",
                    Price = 14.99m,
                    StockQuantity = 104,
                },
                new Product
                {
                    Id = 6,
                    Name = "Producto 006",
                    Description = "Descripción del producto 006",
                    Price = 15.99m,
                    StockQuantity = 105,
                },
                new Product
                {
                    Id = 7,
                    Name = "Producto 007",
                    Description = "Descripción del producto 007",
                    Price = 16.99m,
                    StockQuantity = 106,
                },
                new Product
                {
                    Id = 8,
                    Name = "Producto 008",
                    Description = "Descripción del producto 008",
                    Price = 17.99m,
                    StockQuantity = 107,
                },
                new Product
                {
                    Id = 9,
                    Name = "Producto 009",
                    Description = "Descripción del producto 009",
                    Price = 18.99m,
                    StockQuantity = 108,
                },
                new Product
                {
                    Id = 10,
                    Name = "Producto 010",
                    Description = "Descripción del producto 010",
                    Price = 19.99m,
                    StockQuantity = 109,
                },
                new Product
                {
                    Id = 11,
                    Name = "Producto 011",
                    Description = "Descripción del producto 011",
                    Price = 20.99m,
                    StockQuantity = 110,
                },
                new Product
                {
                    Id = 12,
                    Name = "Producto 012",
                    Description = "Descripción del producto 012",
                    Price = 21.99m,
                    StockQuantity = 111,
                },
                new Product
                {
                    Id = 13,
                    Name = "Producto 013",
                    Description = "Descripción del producto 013",
                    Price = 22.99m,
                    StockQuantity = 112,
                },
                new Product
                {
                    Id = 14,
                    Name = "Producto 014",
                    Description = "Descripción del producto 014",
                    Price = 23.99m,
                    StockQuantity = 113,
                },
                new Product
                {
                    Id = 15,
                    Name = "Producto 015",
                    Description = "Descripción del producto 015",
                    Price = 24.99m,
                    StockQuantity = 114,
                },
                new Product
                {
                    Id = 16,
                    Name = "Producto 016",
                    Description = "Descripción del producto 016",
                    Price = 25.99m,
                    StockQuantity = 115,
                },
                new Product
                {
                    Id = 17,
                    Name = "Producto 017",
                    Description = "Descripción del producto 017",
                    Price = 26.99m,
                    StockQuantity = 116,
                },
                new Product
                {
                    Id = 18,
                    Name = "Producto 018",
                    Description = "Descripción del producto 018",
                    Price = 27.99m,
                    StockQuantity = 117,
                },
                new Product
                {
                    Id = 19,
                    Name = "Producto 019",
                    Description = "Descripción del producto 019",
                    Price = 28.99m,
                    StockQuantity = 118,
                },
                new Product
                {
                    Id = 20,
                    Name = "Producto 020",
                    Description = "Descripción del producto 020",
                    Price = 29.99m,
                    StockQuantity = 119,
                },
                new Product
                {
                    Id = 21,
                    Name = "Producto 021",
                    Description = "Descripción del producto 021",
                    Price = 30.99m,
                    StockQuantity = 120,
                },
                new Product
                {
                    Id = 22,
                    Name = "Producto 022",
                    Description = "Descripción del producto 022",
                    Price = 31.99m,
                    StockQuantity = 121,
                },
                new Product
                {
                    Id = 23,
                    Name = "Producto 023",
                    Description = "Descripción del producto 023",
                    Price = 32.99m,
                    StockQuantity = 122,
                },
                new Product
                {
                    Id = 24,
                    Name = "Producto 024",
                    Description = "Descripción del producto 024",
                    Price = 33.99m,
                    StockQuantity = 123,
                },
                new Product
                {
                    Id = 25,
                    Name = "Producto 025",
                    Description = "Descripción del producto 025",
                    Price = 34.99m,
                    StockQuantity = 124,
                },
                new Product
                {
                    Id = 26,
                    Name = "Producto 026",
                    Description = "Descripción del producto 026",
                    Price = 35.99m,
                    StockQuantity = 125,
                },
                new Product
                {
                    Id = 27,
                    Name = "Producto 027",
                    Description = "Descripción del producto 027",
                    Price = 36.99m,
                    StockQuantity = 126,
                },
                new Product
                {
                    Id = 28,
                    Name = "Producto 028",
                    Description = "Descripción del producto 028",
                    Price = 37.99m,
                    StockQuantity = 127,
                },
                new Product
                {
                    Id = 29,
                    Name = "Producto 029",
                    Description = "Descripción del producto 029",
                    Price = 38.99m,
                    StockQuantity = 128,
                },
                new Product
                {
                    Id = 30,
                    Name = "Producto 030",
                    Description = "Descripción del producto 030",
                    Price = 39.99m,
                    StockQuantity = 129,
                },
                new Product
                {
                    Id = 31,
                    Name = "Producto 031",
                    Description = "Descripción del producto 031",
                    Price = 40.99m,
                    StockQuantity = 130,
                },
                new Product
                {
                    Id = 32,
                    Name = "Producto 032",
                    Description = "Descripción del producto 032",
                    Price = 41.99m,
                    StockQuantity = 131,
                },
                new Product
                {
                    Id = 33,
                    Name = "Producto 033",
                    Description = "Descripción del producto 033",
                    Price = 42.99m,
                    StockQuantity = 132,
                },
                new Product
                {
                    Id = 34,
                    Name = "Producto 034",
                    Description = "Descripción del producto 034",
                    Price = 43.99m,
                    StockQuantity = 133,
                },
                new Product
                {
                    Id = 35,
                    Name = "Producto 035",
                    Description = "Descripción del producto 035",
                    Price = 44.99m,
                    StockQuantity = 134,
                },
                new Product
                {
                    Id = 36,
                    Name = "Producto 036",
                    Description = "Descripción del producto 036",
                    Price = 45.99m,
                    StockQuantity = 135,
                },
                new Product
                {
                    Id = 37,
                    Name = "Producto 037",
                    Description = "Descripción del producto 037",
                    Price = 46.99m,
                    StockQuantity = 136,
                },
                new Product
                {
                    Id = 38,
                    Name = "Producto 038",
                    Description = "Descripción del producto 038",
                    Price = 47.99m,
                    StockQuantity = 137,
                },
                new Product
                {
                    Id = 39,
                    Name = "Producto 039",
                    Description = "Descripción del producto 039",
                    Price = 48.99m,
                    StockQuantity = 138,
                },
                new Product
                {
                    Id = 40,
                    Name = "Producto 040",
                    Description = "Descripción del producto 040",
                    Price = 49.99m,
                    StockQuantity = 139,
                },
                new Product
                {
                    Id = 41,
                    Name = "Producto 041",
                    Description = "Descripción del producto 041",
                    Price = 50.99m,
                    StockQuantity = 140,
                },
                new Product
                {
                    Id = 42,
                    Name = "Producto 042",
                    Description = "Descripción del producto 042",
                    Price = 51.99m,
                    StockQuantity = 141,
                },
                new Product
                {
                    Id = 43,
                    Name = "Producto 043",
                    Description = "Descripción del producto 043",
                    Price = 52.99m,
                    StockQuantity = 142,
                },
                new Product
                {
                    Id = 44,
                    Name = "Producto 044",
                    Description = "Descripción del producto 044",
                    Price = 53.99m,
                    StockQuantity = 143,
                },
                new Product
                {
                    Id = 45,
                    Name = "Producto 045",
                    Description = "Descripción del producto 045",
                    Price = 54.99m,
                    StockQuantity = 144,
                },
                new Product
                {
                    Id = 46,
                    Name = "Producto 046",
                    Description = "Descripción del producto 046",
                    Price = 55.99m,
                    StockQuantity = 145,
                },
                new Product
                {
                    Id = 47,
                    Name = "Producto 047",
                    Description = "Descripción del producto 047",
                    Price = 56.99m,
                    StockQuantity = 146,
                },
                new Product
                {
                    Id = 48,
                    Name = "Producto 048",
                    Description = "Descripción del producto 048",
                    Price = 57.99m,
                    StockQuantity = 147,
                },
                new Product
                {
                    Id = 49,
                    Name = "Producto 049",
                    Description = "Descripción del producto 049",
                    Price = 58.99m,
                    StockQuantity = 148,
                },
                new Product
                {
                    Id = 50,
                    Name = "Producto 050",
                    Description = "Descripción del producto 050",
                    Price = 59.99m,
                    StockQuantity = 149,
                },
                new Product
                {
                    Id = 51,
                    Name = "Producto 051",
                    Description = "Descripción del producto 051",
                    Price = 60.99m,
                    StockQuantity = 150,
                },
                new Product
                {
                    Id = 52,
                    Name = "Producto 052",
                    Description = "Descripción del producto 052",
                    Price = 61.99m,
                    StockQuantity = 151,
                },
                new Product
                {
                    Id = 53,
                    Name = "Producto 053",
                    Description = "Descripción del producto 053",
                    Price = 62.99m,
                    StockQuantity = 152,
                },
                new Product
                {
                    Id = 54,
                    Name = "Producto 054",
                    Description = "Descripción del producto 054",
                    Price = 63.99m,
                    StockQuantity = 153,
                },
                new Product
                {
                    Id = 55,
                    Name = "Producto 055",
                    Description = "Descripción del producto 055",
                    Price = 64.99m,
                    StockQuantity = 154,
                },
                new Product
                {
                    Id = 56,
                    Name = "Producto 056",
                    Description = "Descripción del producto 056",
                    Price = 65.99m,
                    StockQuantity = 155,
                },
                new Product
                {
                    Id = 57,
                    Name = "Producto 057",
                    Description = "Descripción del producto 057",
                    Price = 66.99m,
                    StockQuantity = 156,
                },
                new Product
                {
                    Id = 58,
                    Name = "Producto 058",
                    Description = "Descripción del producto 058",
                    Price = 67.99m,
                    StockQuantity = 157,
                },
                new Product
                {
                    Id = 59,
                    Name = "Producto 059",
                    Description = "Descripción del producto 059",
                    Price = 68.99m,
                    StockQuantity = 158,
                },
                new Product
                {
                    Id = 60,
                    Name = "Producto 060",
                    Description = "Descripción del producto 060",
                    Price = 69.99m,
                    StockQuantity = 159,
                },
                new Product
                {
                    Id = 61,
                    Name = "Producto 061",
                    Description = "Descripción del producto 061",
                    Price = 70.99m,
                    StockQuantity = 160,
                },
                new Product
                {
                    Id = 62,
                    Name = "Producto 062",
                    Description = "Descripción del producto 062",
                    Price = 71.99m,
                    StockQuantity = 161,
                },
                new Product
                {
                    Id = 63,
                    Name = "Producto 063",
                    Description = "Descripción del producto 063",
                    Price = 72.99m,
                    StockQuantity = 162,
                },
                new Product
                {
                    Id = 64,
                    Name = "Producto 064",
                    Description = "Descripción del producto 064",
                    Price = 73.99m,
                    StockQuantity = 163,
                },
                new Product
                {
                    Id = 65,
                    Name = "Producto 065",
                    Description = "Descripción del producto 065",
                    Price = 74.99m,
                    StockQuantity = 164,
                },
                new Product
                {
                    Id = 66,
                    Name = "Producto 066",
                    Description = "Descripción del producto 066",
                    Price = 75.99m,
                    StockQuantity = 165,
                },
                new Product
                {
                    Id = 67,
                    Name = "Producto 067",
                    Description = "Descripción del producto 067",
                    Price = 76.99m,
                    StockQuantity = 166,
                },
                new Product
                {
                    Id = 68,
                    Name = "Producto 068",
                    Description = "Descripción del producto 068",
                    Price = 77.99m,
                    StockQuantity = 167,
                },
                new Product
                {
                    Id = 69,
                    Name = "Producto 069",
                    Description = "Descripción del producto 069",
                    Price = 78.99m,
                    StockQuantity = 168,
                },
                new Product
                {
                    Id = 70,
                    Name = "Producto 070",
                    Description = "Descripción del producto 070",
                    Price = 79.99m,
                    StockQuantity = 169,
                },
                new Product
                {
                    Id = 71,
                    Name = "Producto 071",
                    Description = "Descripción del producto 071",
                    Price = 80.99m,
                    StockQuantity = 170,
                },
                new Product
                {
                    Id = 72,
                    Name = "Producto 072",
                    Description = "Descripción del producto 072",
                    Price = 81.99m,
                    StockQuantity = 171,
                },
                new Product
                {
                    Id = 73,
                    Name = "Producto 073",
                    Description = "Descripción del producto 073",
                    Price = 82.99m,
                    StockQuantity = 172,
                },
                new Product
                {
                    Id = 74,
                    Name = "Producto 074",
                    Description = "Descripción del producto 074",
                    Price = 83.99m,
                    StockQuantity = 173,
                },
                new Product
                {
                    Id = 75,
                    Name = "Producto 075",
                    Description = "Descripción del producto 075",
                    Price = 84.99m,
                    StockQuantity = 174,
                },
                new Product
                {
                    Id = 76,
                    Name = "Producto 076",
                    Description = "Descripción del producto 076",
                    Price = 85.99m,
                    StockQuantity = 175,
                },
                new Product
                {
                    Id = 77,
                    Name = "Producto 077",
                    Description = "Descripción del producto 077",
                    Price = 86.99m,
                    StockQuantity = 176,
                },
                new Product
                {
                    Id = 78,
                    Name = "Producto 078",
                    Description = "Descripción del producto 078",
                    Price = 87.99m,
                    StockQuantity = 177,
                },
                new Product
                {
                    Id = 79,
                    Name = "Producto 079",
                    Description = "Descripción del producto 079",
                    Price = 88.99m,
                    StockQuantity = 178,
                },
                new Product
                {
                    Id = 80,
                    Name = "Producto 080",
                    Description = "Descripción del producto 080",
                    Price = 89.99m,
                    StockQuantity = 179,
                },
                new Product
                {
                    Id = 81,
                    Name = "Producto 081",
                    Description = "Descripción del producto 081",
                    Price = 90.99m,
                    StockQuantity = 180,
                },
                new Product
                {
                    Id = 82,
                    Name = "Producto 082",
                    Description = "Descripción del producto 082",
                    Price = 91.99m,
                    StockQuantity = 181,
                },
                new Product
                {
                    Id = 83,
                    Name = "Producto 083",
                    Description = "Descripción del producto 083",
                    Price = 92.99m,
                    StockQuantity = 182,
                },
                new Product
                {
                    Id = 84,
                    Name = "Producto 084",
                    Description = "Descripción del producto 084",
                    Price = 93.99m,
                    StockQuantity = 183,
                },
                new Product
                {
                    Id = 85,
                    Name = "Producto 085",
                    Description = "Descripción del producto 085",
                    Price = 94.99m,
                    StockQuantity = 184,
                },
                new Product
                {
                    Id = 86,
                    Name = "Producto 086",
                    Description = "Descripción del producto 086",
                    Price = 95.99m,
                    StockQuantity = 185,
                },
                new Product
                {
                    Id = 87,
                    Name = "Producto 087",
                    Description = "Descripción del producto 087",
                    Price = 96.99m,
                    StockQuantity = 186,
                },
                new Product
                {
                    Id = 88,
                    Name = "Producto 088",
                    Description = "Descripción del producto 088",
                    Price = 97.99m,
                    StockQuantity = 187,
                },
                new Product
                {
                    Id = 89,
                    Name = "Producto 089",
                    Description = "Descripción del producto 089",
                    Price = 98.99m,
                    StockQuantity = 188,
                },
                new Product
                {
                    Id = 90,
                    Name = "Producto 090",
                    Description = "Descripción del producto 090",
                    Price = 99.99m,
                    StockQuantity = 189,
                },
                new Product
                {
                    Id = 91,
                    Name = "Producto 091",
                    Description = "Descripción del producto 091",
                    Price = 100.99m,
                    StockQuantity = 190,
                },
                new Product
                {
                    Id = 92,
                    Name = "Producto 092",
                    Description = "Descripción del producto 092",
                    Price = 101.99m,
                    StockQuantity = 191,
                },
                new Product
                {
                    Id = 93,
                    Name = "Producto 093",
                    Description = "Descripción del producto 093",
                    Price = 102.99m,
                    StockQuantity = 192,
                },
                new Product
                {
                    Id = 94,
                    Name = "Producto 094",
                    Description = "Descripción del producto 094",
                    Price = 103.99m,
                    StockQuantity = 193,
                },
                new Product
                {
                    Id = 95,
                    Name = "Producto 095",
                    Description = "Descripción del producto 095",
                    Price = 104.99m,
                    StockQuantity = 194,
                },
                new Product
                {
                    Id = 96,
                    Name = "Producto 096",
                    Description = "Descripción del producto 096",
                    Price = 105.99m,
                    StockQuantity = 195,
                },
                new Product
                {
                    Id = 97,
                    Name = "Producto 097",
                    Description = "Descripción del producto 097",
                    Price = 106.99m,
                    StockQuantity = 196,
                },
                new Product
                {
                    Id = 98,
                    Name = "Producto 098",
                    Description = "Descripción del producto 098",
                    Price = 107.99m,
                    StockQuantity = 197,
                },
                new Product
                {
                    Id = 99,
                    Name = "Producto 099",
                    Description = "Descripción del producto 099",
                    Price = 108.99m,
                    StockQuantity = 198,
                },
                new Product
                {
                    Id = 100,
                    Name = "Producto 100",
                    Description = "Descripción del producto 100",
                    Price = 109.99m,
                    StockQuantity = 199,
                },
                new Product
                {
                    Id = 101,
                    Name = "Producto 101",
                    Description = "Descripción del producto 101",
                    Price = 110.99m,
                    StockQuantity = 200,
                },
                new Product
                {
                    Id = 102,
                    Name = "Producto 102",
                    Description = "Descripción del producto 102",
                    Price = 111.99m,
                    StockQuantity = 201,
                },
                new Product
                {
                    Id = 103,
                    Name = "Producto 103",
                    Description = "Descripción del producto 103",
                    Price = 112.99m,
                    StockQuantity = 202,
                },
                new Product
                {
                    Id = 104,
                    Name = "Producto 104",
                    Description = "Descripción del producto 104",
                    Price = 113.99m,
                    StockQuantity = 203,
                },
                new Product
                {
                    Id = 105,
                    Name = "Producto 105",
                    Description = "Descripción del producto 105",
                    Price = 114.99m,
                    StockQuantity = 204,
                },
                new Product
                {
                    Id = 106,
                    Name = "Producto 106",
                    Description = "Descripción del producto 106",
                    Price = 115.99m,
                    StockQuantity = 205,
                },
                new Product
                {
                    Id = 107,
                    Name = "Producto 107",
                    Description = "Descripción del producto 107",
                    Price = 116.99m,
                    StockQuantity = 206,
                },
                new Product
                {
                    Id = 108,
                    Name = "Producto 108",
                    Description = "Descripción del producto 108",
                    Price = 117.99m,
                    StockQuantity = 207,
                },
                new Product
                {
                    Id = 109,
                    Name = "Producto 109",
                    Description = "Descripción del producto 109",
                    Price = 118.99m,
                    StockQuantity = 208,
                },
                new Product
                {
                    Id = 110,
                    Name = "Producto 110",
                    Description = "Descripción del producto 110",
                    Price = 119.99m,
                    StockQuantity = 209,
                },
                new Product
                {
                    Id = 111,
                    Name = "Producto 111",
                    Description = "Descripción del producto 111",
                    Price = 120.99m,
                    StockQuantity = 210,
                },
                new Product
                {
                    Id = 112,
                    Name = "Producto 112",
                    Description = "Descripción del producto 112",
                    Price = 121.99m,
                    StockQuantity = 211,
                },
                new Product
                {
                    Id = 113,
                    Name = "Producto 113",
                    Description = "Descripción del producto 113",
                    Price = 122.99m,
                    StockQuantity = 212,
                },
                new Product
                {
                    Id = 114,
                    Name = "Producto 114",
                    Description = "Descripción del producto 114",
                    Price = 123.99m,
                    StockQuantity = 213,
                },
                new Product
                {
                    Id = 115,
                    Name = "Producto 115",
                    Description = "Descripción del producto 115",
                    Price = 124.99m,
                    StockQuantity = 214,
                },
                new Product
                {
                    Id = 116,
                    Name = "Producto 116",
                    Description = "Descripción del producto 116",
                    Price = 125.99m,
                    StockQuantity = 215,
                },
                new Product
                {
                    Id = 117,
                    Name = "Producto 117",
                    Description = "Descripción del producto 117",
                    Price = 126.99m,
                    StockQuantity = 216,
                },
                new Product
                {
                    Id = 118,
                    Name = "Producto 118",
                    Description = "Descripción del producto 118",
                    Price = 127.99m,
                    StockQuantity = 217,
                },
                new Product
                {
                    Id = 119,
                    Name = "Producto 119",
                    Description = "Descripción del producto 119",
                    Price = 128.99m,
                    StockQuantity = 218,
                },
                new Product
                {
                    Id = 120,
                    Name = "Producto 120",
                    Description = "Descripción del producto 120",
                    Price = 129.99m,
                    StockQuantity = 219,
                },
                new Product
                {
                    Id = 121,
                    Name = "Producto 121",
                    Description = "Descripción del producto 121",
                    Price = 130.99m,
                    StockQuantity = 220,
                },
                new Product
                {
                    Id = 122,
                    Name = "Producto 122",
                    Description = "Descripción del producto 122",
                    Price = 131.99m,
                    StockQuantity = 221,
                },
                new Product
                {
                    Id = 123,
                    Name = "Producto 123",
                    Description = "Descripción del producto 123",
                    Price = 132.99m,
                    StockQuantity = 222,
                },
                new Product
                {
                    Id = 124,
                    Name = "Producto 124",
                    Description = "Descripción del producto 124",
                    Price = 133.99m,
                    StockQuantity = 223,
                },
                new Product
                {
                    Id = 125,
                    Name = "Producto 125",
                    Description = "Descripción del producto 125",
                    Price = 134.99m,
                    StockQuantity = 224,
                },
                new Product
                {
                    Id = 126,
                    Name = "Producto 126",
                    Description = "Descripción del producto 126",
                    Price = 135.99m,
                    StockQuantity = 225,
                },
                new Product
                {
                    Id = 127,
                    Name = "Producto 127",
                    Description = "Descripción del producto 127",
                    Price = 136.99m,
                    StockQuantity = 226,
                },
                new Product
                {
                    Id = 128,
                    Name = "Producto 128",
                    Description = "Descripción del producto 128",
                    Price = 137.99m,
                    StockQuantity = 227,
                },
                new Product
                {
                    Id = 129,
                    Name = "Producto 129",
                    Description = "Descripción del producto 129",
                    Price = 138.99m,
                    StockQuantity = 228,
                },
                new Product
                {
                    Id = 130,
                    Name = "Producto 130",
                    Description = "Descripción del producto 130",
                    Price = 139.99m,
                    StockQuantity = 229,
                },
                new Product
                {
                    Id = 131,
                    Name = "Producto 131",
                    Description = "Descripción del producto 131",
                    Price = 140.99m,
                    StockQuantity = 230,
                },
                new Product
                {
                    Id = 132,
                    Name = "Producto 132",
                    Description = "Descripción del producto 132",
                    Price = 141.99m,
                    StockQuantity = 231,
                },
                new Product
                {
                    Id = 133,
                    Name = "Producto 133",
                    Description = "Descripción del producto 133",
                    Price = 142.99m,
                    StockQuantity = 232,
                },
                new Product
                {
                    Id = 134,
                    Name = "Producto 134",
                    Description = "Descripción del producto 134",
                    Price = 143.99m,
                    StockQuantity = 233,
                },
                new Product
                {
                    Id = 135,
                    Name = "Producto 135",
                    Description = "Descripción del producto 135",
                    Price = 144.99m,
                    StockQuantity = 234,
                },
                new Product
                {
                    Id = 136,
                    Name = "Producto 136",
                    Description = "Descripción del producto 136",
                    Price = 145.99m,
                    StockQuantity = 235,
                },
                new Product
                {
                    Id = 137,
                    Name = "Producto 137",
                    Description = "Descripción del producto 137",
                    Price = 146.99m,
                    StockQuantity = 236,
                },
                new Product
                {
                    Id = 138,
                    Name = "Producto 138",
                    Description = "Descripción del producto 138",
                    Price = 147.99m,
                    StockQuantity = 237,
                },
                new Product
                {
                    Id = 139,
                    Name = "Producto 139",
                    Description = "Descripción del producto 139",
                    Price = 148.99m,
                    StockQuantity = 238,
                },
                new Product
                {
                    Id = 140,
                    Name = "Producto 140",
                    Description = "Descripción del producto 140",
                    Price = 149.99m,
                    StockQuantity = 239,
                },
                new Product
                {
                    Id = 141,
                    Name = "Producto 141",
                    Description = "Descripción del producto 141",
                    Price = 150.99m,
                    StockQuantity = 240,
                },
                new Product
                {
                    Id = 142,
                    Name = "Producto 142",
                    Description = "Descripción del producto 142",
                    Price = 151.99m,
                    StockQuantity = 241,
                },
                new Product
                {
                    Id = 143,
                    Name = "Producto 143",
                    Description = "Descripción del producto 143",
                    Price = 152.99m,
                    StockQuantity = 242,
                },
                new Product
                {
                    Id = 144,
                    Name = "Producto 144",
                    Description = "Descripción del producto 144",
                    Price = 153.99m,
                    StockQuantity = 243,
                },
                new Product
                {
                    Id = 145,
                    Name = "Producto 145",
                    Description = "Descripción del producto 145",
                    Price = 154.99m,
                    StockQuantity = 244,
                },
                new Product
                {
                    Id = 146,
                    Name = "Producto 146",
                    Description = "Descripción del producto 146",
                    Price = 155.99m,
                    StockQuantity = 245,
                },
                new Product
                {
                    Id = 147,
                    Name = "Producto 147",
                    Description = "Descripción del producto 147",
                    Price = 156.99m,
                    StockQuantity = 246,
                },
                new Product
                {
                    Id = 148,
                    Name = "Producto 148",
                    Description = "Descripción del producto 148",
                    Price = 157.99m,
                    StockQuantity = 247,
                },
                new Product
                {
                    Id = 149,
                    Name = "Producto 149",
                    Description = "Descripción del producto 149",
                    Price = 158.99m,
                    StockQuantity = 248,
                },
                new Product
                {
                    Id = 150,
                    Name = "Producto 150",
                    Description = "Descripción del producto 150",
                    Price = 159.99m,
                    StockQuantity = 249,
                }
            );

        // Datos iniciales para órdenes (valores estáticos)
        modelBuilder
            .Entity<Order>()
            .HasData(
                new Order
                {
                    Id = 1,
                    CreatedAt = new DateTime(2024, 1, 15),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 2,
                    CreatedAt = new DateTime(2024, 1, 16),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 3,
                    CreatedAt = new DateTime(2024, 1, 17),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 4,
                    CreatedAt = new DateTime(2024, 1, 18),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 5,
                    CreatedAt = new DateTime(2024, 1, 19),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 6,
                    CreatedAt = new DateTime(2024, 1, 20),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 7,
                    CreatedAt = new DateTime(2024, 1, 21),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 8,
                    CreatedAt = new DateTime(2024, 1, 22),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 9,
                    CreatedAt = new DateTime(2024, 1, 23),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 10,
                    CreatedAt = new DateTime(2024, 1, 24),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 11,
                    CreatedAt = new DateTime(2024, 1, 25),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 12,
                    CreatedAt = new DateTime(2024, 1, 26),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 13,
                    CreatedAt = new DateTime(2024, 1, 27),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 14,
                    CreatedAt = new DateTime(2024, 1, 28),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 15,
                    CreatedAt = new DateTime(2024, 1, 29),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 16,
                    CreatedAt = new DateTime(2024, 1, 30),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 17,
                    CreatedAt = new DateTime(2024, 1, 31),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 18,
                    CreatedAt = new DateTime(2024, 2, 1),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 19,
                    CreatedAt = new DateTime(2024, 2, 2),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 20,
                    CreatedAt = new DateTime(2024, 2, 3),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 21,
                    CreatedAt = new DateTime(2024, 2, 4),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 22,
                    CreatedAt = new DateTime(2024, 2, 5),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 23,
                    CreatedAt = new DateTime(2024, 2, 6),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 24,
                    CreatedAt = new DateTime(2024, 2, 7),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 25,
                    CreatedAt = new DateTime(2024, 2, 8),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 26,
                    CreatedAt = new DateTime(2024, 2, 9),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 27,
                    CreatedAt = new DateTime(2024, 2, 10),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 28,
                    CreatedAt = new DateTime(2024, 2, 11),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 29,
                    CreatedAt = new DateTime(2024, 2, 12),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 30,
                    CreatedAt = new DateTime(2024, 2, 13),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 31,
                    CreatedAt = new DateTime(2024, 2, 14),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 32,
                    CreatedAt = new DateTime(2024, 2, 15),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 33,
                    CreatedAt = new DateTime(2024, 2, 16),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 34,
                    CreatedAt = new DateTime(2024, 2, 17),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 35,
                    CreatedAt = new DateTime(2024, 2, 18),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 36,
                    CreatedAt = new DateTime(2024, 2, 19),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 37,
                    CreatedAt = new DateTime(2024, 2, 20),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 38,
                    CreatedAt = new DateTime(2024, 2, 21),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 39,
                    CreatedAt = new DateTime(2024, 2, 22),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 40,
                    CreatedAt = new DateTime(2024, 2, 23),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 41,
                    CreatedAt = new DateTime(2024, 2, 24),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 42,
                    CreatedAt = new DateTime(2024, 2, 25),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 43,
                    CreatedAt = new DateTime(2024, 2, 26),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 44,
                    CreatedAt = new DateTime(2024, 2, 27),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 45,
                    CreatedAt = new DateTime(2024, 2, 28),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 46,
                    CreatedAt = new DateTime(2024, 2, 29),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 47,
                    CreatedAt = new DateTime(2024, 3, 1),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 48,
                    CreatedAt = new DateTime(2024, 3, 2),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 49,
                    CreatedAt = new DateTime(2024, 3, 3),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 50,
                    CreatedAt = new DateTime(2024, 3, 4),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 51,
                    CreatedAt = new DateTime(2024, 3, 5),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 52,
                    CreatedAt = new DateTime(2024, 3, 6),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 53,
                    CreatedAt = new DateTime(2024, 3, 7),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 54,
                    CreatedAt = new DateTime(2024, 3, 8),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 55,
                    CreatedAt = new DateTime(2024, 3, 9),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 56,
                    CreatedAt = new DateTime(2024, 3, 10),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 57,
                    CreatedAt = new DateTime(2024, 3, 11),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 58,
                    CreatedAt = new DateTime(2024, 3, 12),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 59,
                    CreatedAt = new DateTime(2024, 3, 13),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 60,
                    CreatedAt = new DateTime(2024, 3, 14),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 61,
                    CreatedAt = new DateTime(2024, 3, 15),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 62,
                    CreatedAt = new DateTime(2024, 3, 16),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 63,
                    CreatedAt = new DateTime(2024, 3, 17),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 64,
                    CreatedAt = new DateTime(2024, 3, 18),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 65,
                    CreatedAt = new DateTime(2024, 3, 19),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 66,
                    CreatedAt = new DateTime(2024, 3, 20),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 67,
                    CreatedAt = new DateTime(2024, 3, 21),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 68,
                    CreatedAt = new DateTime(2024, 3, 22),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 69,
                    CreatedAt = new DateTime(2024, 3, 23),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 70,
                    CreatedAt = new DateTime(2024, 3, 24),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 71,
                    CreatedAt = new DateTime(2024, 3, 25),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 72,
                    CreatedAt = new DateTime(2024, 3, 26),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 73,
                    CreatedAt = new DateTime(2024, 3, 27),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 74,
                    CreatedAt = new DateTime(2024, 3, 28),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 75,
                    CreatedAt = new DateTime(2024, 3, 29),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 76,
                    CreatedAt = new DateTime(2024, 3, 30),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 77,
                    CreatedAt = new DateTime(2024, 3, 31),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 78,
                    CreatedAt = new DateTime(2024, 4, 1),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 79,
                    CreatedAt = new DateTime(2024, 4, 2),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 80,
                    CreatedAt = new DateTime(2024, 4, 3),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 81,
                    CreatedAt = new DateTime(2024, 4, 4),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 82,
                    CreatedAt = new DateTime(2024, 4, 5),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 83,
                    CreatedAt = new DateTime(2024, 4, 6),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 84,
                    CreatedAt = new DateTime(2024, 4, 7),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 85,
                    CreatedAt = new DateTime(2024, 4, 8),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 86,
                    CreatedAt = new DateTime(2024, 4, 9),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 87,
                    CreatedAt = new DateTime(2024, 4, 10),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 88,
                    CreatedAt = new DateTime(2024, 4, 11),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 89,
                    CreatedAt = new DateTime(2024, 4, 12),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 90,
                    CreatedAt = new DateTime(2024, 4, 13),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 91,
                    CreatedAt = new DateTime(2024, 4, 14),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 92,
                    CreatedAt = new DateTime(2024, 4, 15),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 93,
                    CreatedAt = new DateTime(2024, 4, 16),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 94,
                    CreatedAt = new DateTime(2024, 4, 17),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 95,
                    CreatedAt = new DateTime(2024, 4, 18),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 96,
                    CreatedAt = new DateTime(2024, 4, 19),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 97,
                    CreatedAt = new DateTime(2024, 4, 20),
                    State = OrderState.Pendiente,
                },
                new Order
                {
                    Id = 98,
                    CreatedAt = new DateTime(2024, 4, 21),
                    State = OrderState.Pagado,
                },
                new Order
                {
                    Id = 99,
                    CreatedAt = new DateTime(2024, 4, 22),
                    State = OrderState.Enviado,
                },
                new Order
                {
                    Id = 100,
                    CreatedAt = new DateTime(2024, 4, 23),
                    State = OrderState.Pendiente,
                }
            );

        // Datos iniciales para ítems de órdenes (valores estáticos)
        modelBuilder
            .Entity<OrderItem>()
            .HasData(
                new OrderItem
                {
                    Id = 1,
                    OrderId = 1,
                    ProductId = 1,
                    Quantity = 2,
                    UnitPrice = 10.99m,
                },
                new OrderItem
                {
                    Id = 2,
                    OrderId = 1,
                    ProductId = 5,
                    Quantity = 1,
                    UnitPrice = 14.99m,
                },
                new OrderItem
                {
                    Id = 3,
                    OrderId = 2,
                    ProductId = 10,
                    Quantity = 3,
                    UnitPrice = 19.99m,
                },
                new OrderItem
                {
                    Id = 4,
                    OrderId = 3,
                    ProductId = 15,
                    Quantity = 1,
                    UnitPrice = 24.99m,
                },
                new OrderItem
                {
                    Id = 5,
                    OrderId = 4,
                    ProductId = 20,
                    Quantity = 2,
                    UnitPrice = 29.99m,
                },
                new OrderItem
                {
                    Id = 6,
                    OrderId = 5,
                    ProductId = 25,
                    Quantity = 1,
                    UnitPrice = 34.99m,
                },
                new OrderItem
                {
                    Id = 7,
                    OrderId = 6,
                    ProductId = 30,
                    Quantity = 4,
                    UnitPrice = 39.99m,
                },
                new OrderItem
                {
                    Id = 8,
                    OrderId = 7,
                    ProductId = 35,
                    Quantity = 2,
                    UnitPrice = 44.99m,
                },
                new OrderItem
                {
                    Id = 9,
                    OrderId = 8,
                    ProductId = 40,
                    Quantity = 1,
                    UnitPrice = 49.99m,
                },
                new OrderItem
                {
                    Id = 10,
                    OrderId = 9,
                    ProductId = 45,
                    Quantity = 3,
                    UnitPrice = 54.99m,
                },
                new OrderItem
                {
                    Id = 11,
                    OrderId = 10,
                    ProductId = 50,
                    Quantity = 2,
                    UnitPrice = 59.99m,
                },
                new OrderItem
                {
                    Id = 12,
                    OrderId = 11,
                    ProductId = 55,
                    Quantity = 1,
                    UnitPrice = 64.99m,
                },
                new OrderItem
                {
                    Id = 13,
                    OrderId = 12,
                    ProductId = 60,
                    Quantity = 5,
                    UnitPrice = 69.99m,
                },
                new OrderItem
                {
                    Id = 14,
                    OrderId = 13,
                    ProductId = 65,
                    Quantity = 2,
                    UnitPrice = 74.99m,
                },
                new OrderItem
                {
                    Id = 15,
                    OrderId = 14,
                    ProductId = 70,
                    Quantity = 1,
                    UnitPrice = 79.99m,
                },
                new OrderItem
                {
                    Id = 16,
                    OrderId = 15,
                    ProductId = 75,
                    Quantity = 3,
                    UnitPrice = 84.99m,
                },
                new OrderItem
                {
                    Id = 17,
                    OrderId = 16,
                    ProductId = 80,
                    Quantity = 2,
                    UnitPrice = 89.99m,
                },
                new OrderItem
                {
                    Id = 18,
                    OrderId = 17,
                    ProductId = 85,
                    Quantity = 1,
                    UnitPrice = 94.99m,
                },
                new OrderItem
                {
                    Id = 19,
                    OrderId = 18,
                    ProductId = 90,
                    Quantity = 4,
                    UnitPrice = 99.99m,
                },
                new OrderItem
                {
                    Id = 20,
                    OrderId = 19,
                    ProductId = 95,
                    Quantity = 2,
                    UnitPrice = 104.99m,
                },
                new OrderItem
                {
                    Id = 21,
                    OrderId = 20,
                    ProductId = 100,
                    Quantity = 1,
                    UnitPrice = 109.99m,
                },
                new OrderItem
                {
                    Id = 22,
                    OrderId = 21,
                    ProductId = 105,
                    Quantity = 3,
                    UnitPrice = 114.99m,
                },
                new OrderItem
                {
                    Id = 23,
                    OrderId = 22,
                    ProductId = 110,
                    Quantity = 2,
                    UnitPrice = 119.99m,
                },
                new OrderItem
                {
                    Id = 24,
                    OrderId = 23,
                    ProductId = 115,
                    Quantity = 1,
                    UnitPrice = 124.99m,
                },
                new OrderItem
                {
                    Id = 25,
                    OrderId = 24,
                    ProductId = 120,
                    Quantity = 5,
                    UnitPrice = 129.99m,
                },
                new OrderItem
                {
                    Id = 26,
                    OrderId = 25,
                    ProductId = 125,
                    Quantity = 2,
                    UnitPrice = 134.99m,
                },
                new OrderItem
                {
                    Id = 27,
                    OrderId = 26,
                    ProductId = 130,
                    Quantity = 1,
                    UnitPrice = 139.99m,
                },
                new OrderItem
                {
                    Id = 28,
                    OrderId = 27,
                    ProductId = 135,
                    Quantity = 3,
                    UnitPrice = 144.99m,
                },
                new OrderItem
                {
                    Id = 29,
                    OrderId = 28,
                    ProductId = 140,
                    Quantity = 2,
                    UnitPrice = 149.99m,
                },
                new OrderItem
                {
                    Id = 30,
                    OrderId = 29,
                    ProductId = 145,
                    Quantity = 1,
                    UnitPrice = 154.99m,
                },
                new OrderItem
                {
                    Id = 31,
                    OrderId = 30,
                    ProductId = 150,
                    Quantity = 4,
                    UnitPrice = 159.99m,
                },
                new OrderItem
                {
                    Id = 32,
                    OrderId = 31,
                    ProductId = 2,
                    Quantity = 2,
                    UnitPrice = 11.99m,
                },
                new OrderItem
                {
                    Id = 33,
                    OrderId = 32,
                    ProductId = 7,
                    Quantity = 1,
                    UnitPrice = 16.99m,
                },
                new OrderItem
                {
                    Id = 34,
                    OrderId = 33,
                    ProductId = 12,
                    Quantity = 3,
                    UnitPrice = 21.99m,
                },
                new OrderItem
                {
                    Id = 35,
                    OrderId = 34,
                    ProductId = 17,
                    Quantity = 2,
                    UnitPrice = 26.99m,
                },
                new OrderItem
                {
                    Id = 36,
                    OrderId = 35,
                    ProductId = 22,
                    Quantity = 1,
                    UnitPrice = 31.99m,
                },
                new OrderItem
                {
                    Id = 37,
                    OrderId = 36,
                    ProductId = 27,
                    Quantity = 5,
                    UnitPrice = 36.99m,
                },
                new OrderItem
                {
                    Id = 38,
                    OrderId = 37,
                    ProductId = 32,
                    Quantity = 2,
                    UnitPrice = 41.99m,
                },
                new OrderItem
                {
                    Id = 39,
                    OrderId = 38,
                    ProductId = 37,
                    Quantity = 1,
                    UnitPrice = 46.99m,
                },
                new OrderItem
                {
                    Id = 40,
                    OrderId = 39,
                    ProductId = 42,
                    Quantity = 3,
                    UnitPrice = 51.99m,
                },
                new OrderItem
                {
                    Id = 41,
                    OrderId = 40,
                    ProductId = 47,
                    Quantity = 2,
                    UnitPrice = 56.99m,
                },
                new OrderItem
                {
                    Id = 42,
                    OrderId = 41,
                    ProductId = 52,
                    Quantity = 1,
                    UnitPrice = 61.99m,
                },
                new OrderItem
                {
                    Id = 43,
                    OrderId = 42,
                    ProductId = 57,
                    Quantity = 4,
                    UnitPrice = 66.99m,
                },
                new OrderItem
                {
                    Id = 44,
                    OrderId = 43,
                    ProductId = 62,
                    Quantity = 2,
                    UnitPrice = 71.99m,
                },
                new OrderItem
                {
                    Id = 45,
                    OrderId = 44,
                    ProductId = 67,
                    Quantity = 1,
                    UnitPrice = 76.99m,
                },
                new OrderItem
                {
                    Id = 46,
                    OrderId = 45,
                    ProductId = 72,
                    Quantity = 3,
                    UnitPrice = 81.99m,
                },
                new OrderItem
                {
                    Id = 47,
                    OrderId = 46,
                    ProductId = 77,
                    Quantity = 2,
                    UnitPrice = 86.99m,
                },
                new OrderItem
                {
                    Id = 48,
                    OrderId = 47,
                    ProductId = 82,
                    Quantity = 1,
                    UnitPrice = 91.99m,
                },
                new OrderItem
                {
                    Id = 49,
                    OrderId = 48,
                    ProductId = 87,
                    Quantity = 5,
                    UnitPrice = 96.99m,
                },
                new OrderItem
                {
                    Id = 50,
                    OrderId = 49,
                    ProductId = 92,
                    Quantity = 2,
                    UnitPrice = 101.99m,
                },
                new OrderItem
                {
                    Id = 51,
                    OrderId = 50,
                    ProductId = 97,
                    Quantity = 1,
                    UnitPrice = 106.99m,
                },
                new OrderItem
                {
                    Id = 52,
                    OrderId = 51,
                    ProductId = 102,
                    Quantity = 3,
                    UnitPrice = 111.99m,
                },
                new OrderItem
                {
                    Id = 53,
                    OrderId = 52,
                    ProductId = 107,
                    Quantity = 2,
                    UnitPrice = 116.99m,
                },
                new OrderItem
                {
                    Id = 54,
                    OrderId = 53,
                    ProductId = 112,
                    Quantity = 1,
                    UnitPrice = 121.99m,
                },
                new OrderItem
                {
                    Id = 55,
                    OrderId = 54,
                    ProductId = 117,
                    Quantity = 4,
                    UnitPrice = 126.99m,
                },
                new OrderItem
                {
                    Id = 56,
                    OrderId = 55,
                    ProductId = 122,
                    Quantity = 2,
                    UnitPrice = 131.99m,
                },
                new OrderItem
                {
                    Id = 57,
                    OrderId = 56,
                    ProductId = 127,
                    Quantity = 1,
                    UnitPrice = 136.99m,
                },
                new OrderItem
                {
                    Id = 58,
                    OrderId = 57,
                    ProductId = 132,
                    Quantity = 3,
                    UnitPrice = 141.99m,
                },
                new OrderItem
                {
                    Id = 59,
                    OrderId = 58,
                    ProductId = 137,
                    Quantity = 2,
                    UnitPrice = 146.99m,
                },
                new OrderItem
                {
                    Id = 60,
                    OrderId = 59,
                    ProductId = 142,
                    Quantity = 1,
                    UnitPrice = 151.99m,
                },
                new OrderItem
                {
                    Id = 61,
                    OrderId = 60,
                    ProductId = 147,
                    Quantity = 5,
                    UnitPrice = 156.99m,
                },
                new OrderItem
                {
                    Id = 62,
                    OrderId = 61,
                    ProductId = 3,
                    Quantity = 2,
                    UnitPrice = 12.99m,
                },
                new OrderItem
                {
                    Id = 63,
                    OrderId = 62,
                    ProductId = 8,
                    Quantity = 1,
                    UnitPrice = 17.99m,
                },
                new OrderItem
                {
                    Id = 64,
                    OrderId = 63,
                    ProductId = 13,
                    Quantity = 3,
                    UnitPrice = 22.99m,
                },
                new OrderItem
                {
                    Id = 65,
                    OrderId = 64,
                    ProductId = 18,
                    Quantity = 2,
                    UnitPrice = 27.99m,
                },
                new OrderItem
                {
                    Id = 66,
                    OrderId = 65,
                    ProductId = 23,
                    Quantity = 1,
                    UnitPrice = 32.99m,
                },
                new OrderItem
                {
                    Id = 67,
                    OrderId = 66,
                    ProductId = 28,
                    Quantity = 4,
                    UnitPrice = 37.99m,
                },
                new OrderItem
                {
                    Id = 68,
                    OrderId = 67,
                    ProductId = 33,
                    Quantity = 2,
                    UnitPrice = 42.99m,
                },
                new OrderItem
                {
                    Id = 69,
                    OrderId = 68,
                    ProductId = 38,
                    Quantity = 1,
                    UnitPrice = 47.99m,
                },
                new OrderItem
                {
                    Id = 70,
                    OrderId = 69,
                    ProductId = 43,
                    Quantity = 3,
                    UnitPrice = 52.99m,
                },
                new OrderItem
                {
                    Id = 71,
                    OrderId = 70,
                    ProductId = 48,
                    Quantity = 2,
                    UnitPrice = 57.99m,
                },
                new OrderItem
                {
                    Id = 72,
                    OrderId = 71,
                    ProductId = 53,
                    Quantity = 1,
                    UnitPrice = 62.99m,
                },
                new OrderItem
                {
                    Id = 73,
                    OrderId = 72,
                    ProductId = 58,
                    Quantity = 5,
                    UnitPrice = 67.99m,
                },
                new OrderItem
                {
                    Id = 74,
                    OrderId = 73,
                    ProductId = 63,
                    Quantity = 2,
                    UnitPrice = 72.99m,
                },
                new OrderItem
                {
                    Id = 75,
                    OrderId = 74,
                    ProductId = 68,
                    Quantity = 1,
                    UnitPrice = 77.99m,
                },
                new OrderItem
                {
                    Id = 76,
                    OrderId = 75,
                    ProductId = 73,
                    Quantity = 3,
                    UnitPrice = 82.99m,
                },
                new OrderItem
                {
                    Id = 77,
                    OrderId = 76,
                    ProductId = 78,
                    Quantity = 2,
                    UnitPrice = 87.99m,
                },
                new OrderItem
                {
                    Id = 78,
                    OrderId = 77,
                    ProductId = 83,
                    Quantity = 1,
                    UnitPrice = 92.99m,
                },
                new OrderItem
                {
                    Id = 79,
                    OrderId = 78,
                    ProductId = 88,
                    Quantity = 4,
                    UnitPrice = 97.99m,
                },
                new OrderItem
                {
                    Id = 80,
                    OrderId = 79,
                    ProductId = 93,
                    Quantity = 2,
                    UnitPrice = 102.99m,
                },
                new OrderItem
                {
                    Id = 81,
                    OrderId = 80,
                    ProductId = 98,
                    Quantity = 1,
                    UnitPrice = 107.99m,
                },
                new OrderItem
                {
                    Id = 82,
                    OrderId = 81,
                    ProductId = 103,
                    Quantity = 3,
                    UnitPrice = 112.99m,
                },
                new OrderItem
                {
                    Id = 83,
                    OrderId = 82,
                    ProductId = 108,
                    Quantity = 2,
                    UnitPrice = 117.99m,
                },
                new OrderItem
                {
                    Id = 84,
                    OrderId = 83,
                    ProductId = 113,
                    Quantity = 1,
                    UnitPrice = 122.99m,
                },
                new OrderItem
                {
                    Id = 85,
                    OrderId = 84,
                    ProductId = 118,
                    Quantity = 5,
                    UnitPrice = 127.99m,
                },
                new OrderItem
                {
                    Id = 86,
                    OrderId = 85,
                    ProductId = 123,
                    Quantity = 2,
                    UnitPrice = 132.99m,
                },
                new OrderItem
                {
                    Id = 87,
                    OrderId = 86,
                    ProductId = 128,
                    Quantity = 1,
                    UnitPrice = 137.99m,
                },
                new OrderItem
                {
                    Id = 88,
                    OrderId = 87,
                    ProductId = 133,
                    Quantity = 3,
                    UnitPrice = 142.99m,
                },
                new OrderItem
                {
                    Id = 89,
                    OrderId = 88,
                    ProductId = 138,
                    Quantity = 2,
                    UnitPrice = 147.99m,
                },
                new OrderItem
                {
                    Id = 90,
                    OrderId = 89,
                    ProductId = 143,
                    Quantity = 1,
                    UnitPrice = 152.99m,
                },
                new OrderItem
                {
                    Id = 91,
                    OrderId = 90,
                    ProductId = 148,
                    Quantity = 4,
                    UnitPrice = 157.99m,
                },
                new OrderItem
                {
                    Id = 92,
                    OrderId = 91,
                    ProductId = 4,
                    Quantity = 2,
                    UnitPrice = 13.99m,
                },
                new OrderItem
                {
                    Id = 93,
                    OrderId = 92,
                    ProductId = 9,
                    Quantity = 1,
                    UnitPrice = 18.99m,
                },
                new OrderItem
                {
                    Id = 94,
                    OrderId = 93,
                    ProductId = 14,
                    Quantity = 3,
                    UnitPrice = 23.99m,
                },
                new OrderItem
                {
                    Id = 95,
                    OrderId = 94,
                    ProductId = 19,
                    Quantity = 2,
                    UnitPrice = 28.99m,
                },
                new OrderItem
                {
                    Id = 96,
                    OrderId = 95,
                    ProductId = 24,
                    Quantity = 1,
                    UnitPrice = 33.99m,
                },
                new OrderItem
                {
                    Id = 97,
                    OrderId = 96,
                    ProductId = 29,
                    Quantity = 5,
                    UnitPrice = 38.99m,
                },
                new OrderItem
                {
                    Id = 98,
                    OrderId = 97,
                    ProductId = 34,
                    Quantity = 2,
                    UnitPrice = 43.99m,
                },
                new OrderItem
                {
                    Id = 99,
                    OrderId = 98,
                    ProductId = 39,
                    Quantity = 1,
                    UnitPrice = 48.99m,
                },
                new OrderItem
                {
                    Id = 100,
                    OrderId = 99,
                    ProductId = 44,
                    Quantity = 3,
                    UnitPrice = 53.99m,
                },
                new OrderItem
                {
                    Id = 101,
                    OrderId = 100,
                    ProductId = 49,
                    Quantity = 2,
                    UnitPrice = 58.99m,
                }
            );
    }
}
