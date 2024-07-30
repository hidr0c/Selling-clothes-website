using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Drawing.Drawing2D;
using System.Linq;

namespace codeweb.Models
{
    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Database")
        {
            this.Configuration.ProxyCreationEnabled = false;
        }

        public static IEnumerable<object> Cart { get; internal set; }
        public virtual DbSet<AdminUser> AdminUsers { get; set; }
/*        public virtual DbSet<Brand> Brands { get; set; }*/
        public virtual DbSet<Customer> Customers { get; set; }
        public virtual DbSet<OrderDetail> OrderDetails { get; set; }
        public virtual DbSet<OrderPro> OrderProes { get; set; }
        public virtual DbSet<Product> OrderPro { get; set; }
        public virtual DbSet<Brand> Brands { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AdminUser>()
                .Property(e => e.IDCus);

/*            modelBuilder.Entity<Brand>()
                .Property(e => e.IDBrand)
                .IsFixedLength();*/

            modelBuilder.Entity<Brand>()
            .Property(e => e.IDBrand)
            .IsFixedLength();


            modelBuilder.Entity<OrderPro>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.OrderPro)
                .HasForeignKey(e => e.IDOrder);

            modelBuilder.Entity<Product>()
                .Property(e => e.IDBrand)
                .IsFixedLength();

            modelBuilder.Entity<Product>()
                .HasMany(e => e.OrderDetails)
                .WithOptional(e => e.Product)
                .HasForeignKey(e => e.IDProduct);
        }
    }
}
