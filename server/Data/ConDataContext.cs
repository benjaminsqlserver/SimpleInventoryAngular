using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

using InventoryManager.Models.ConData;

namespace InventoryManager.Data
{
    public partial class ConDataContext : Microsoft.EntityFrameworkCore.DbContext
    {
        private readonly IHttpContextAccessor httpAccessor;

        public ConDataContext(IHttpContextAccessor httpAccessor, DbContextOptions<ConDataContext> options):base(options)
        {
            this.httpAccessor = httpAccessor;
        }

        public ConDataContext(IHttpContextAccessor httpAccessor)
        {
            this.httpAccessor = httpAccessor;
        }

        partial void OnModelBuilding(ModelBuilder builder);

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .HasOne(i => i.Product)
                  .WithMany(i => i.InventoryTransactions)
                  .HasForeignKey(i => i.ProductID)
                  .HasPrincipalKey(i => i.ProductID);
            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .HasOne(i => i.Supplier)
                  .WithMany(i => i.Products)
                  .HasForeignKey(i => i.SupplierID)
                  .HasPrincipalKey(i => i.SupplierID);
            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .HasOne(i => i.Category)
                  .WithMany(i => i.Products)
                  .HasForeignKey(i => i.CategoryID)
                  .HasPrincipalKey(i => i.CategoryID);

            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .Property(p => p.TransactionDate)
                  .HasDefaultValueSql("(getdate())");

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.Discontinued)
                  .HasDefaultValueSql("((0))");


            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .Property(p => p.TransactionDate)
                  .HasColumnType("datetime");

            builder.Entity<InventoryManager.Models.ConData.Category>()
                  .Property(p => p.CategoryID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .Property(p => p.TransactionID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .Property(p => p.ProductID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.InventoryTransaction>()
                  .Property(p => p.Quantity)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.ProductID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.SupplierID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.CategoryID)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.UnitPrice)
                  .HasPrecision(18, 2);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.UnitsInStock)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.UnitsOnOrder)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Product>()
                  .Property(p => p.ReorderLevel)
                  .HasPrecision(10, 0);

            builder.Entity<InventoryManager.Models.ConData.Supplier>()
                  .Property(p => p.SupplierID)
                  .HasPrecision(10, 0);
            this.OnModelBuilding(builder);
        }


        public DbSet<InventoryManager.Models.ConData.Category> Categories
        {
          get;
          set;
        }

        public DbSet<InventoryManager.Models.ConData.InventoryTransaction> InventoryTransactions
        {
          get;
          set;
        }

        public DbSet<InventoryManager.Models.ConData.Product> Products
        {
          get;
          set;
        }

        public DbSet<InventoryManager.Models.ConData.Supplier> Suppliers
        {
          get;
          set;
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Conventions.Add(_ => new BlankTriggerAddingConvention());
        }
    }
}
