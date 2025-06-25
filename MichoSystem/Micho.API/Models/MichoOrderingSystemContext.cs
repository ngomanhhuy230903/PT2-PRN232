using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Micho.API.Models;

public partial class MichoOrderingSystemContext : DbContext
{
    public MichoOrderingSystemContext()
    {
    }

    public MichoOrderingSystemContext(DbContextOptions<MichoOrderingSystemContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Customer> Customers { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<IceCream> IceCreams { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderDetail> OrderDetails { get; set; }

    public virtual DbSet<OrderDetailIceCream> OrderDetailIceCreams { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=Dev\\HUY;Initial Catalog=MICHO_OrderingSystem;Integrated Security=True;Trust Server Certificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Customer>(entity =>
        {
            entity.HasKey(e => e.CustomerId).HasName("PK__Customer__A4AE64B8B13FBAF1");

            entity.ToTable("Customer");

            entity.HasIndex(e => e.Contact, "UQ__Customer__F7C04665468BEEBA").IsUnique();

            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.Contact)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.EmpId).HasName("PK__Employee__AF2DBA793B008025");

            entity.ToTable("Employee");

            entity.HasIndex(e => e.Idcard, "UQ__Employee__43A2A4E379E56F70").IsUnique();

            entity.HasIndex(e => e.Phone, "UQ__Employee__5C7E359E309CA017").IsUnique();

            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.Address).HasMaxLength(255);
            entity.Property(e => e.EmpName).HasMaxLength(100);
            entity.Property(e => e.Gender).HasMaxLength(10);
            entity.Property(e => e.Idcard)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("IDCard");
            entity.Property(e => e.Phone)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<IceCream>(entity =>
        {
            entity.HasKey(e => e.IceCreamId).HasName("PK__IceCream__47EA7527DE97EF10");

            entity.ToTable("IceCream");

            entity.HasIndex(e => e.Name, "UQ__IceCream__737584F61B78870E").IsUnique();

            entity.Property(e => e.IceCreamId).HasColumnName("IceCreamID");
            entity.Property(e => e.Color).HasMaxLength(50);
            entity.Property(e => e.Flavor).HasMaxLength(50);
            entity.Property(e => e.Name).HasMaxLength(100);
            entity.Property(e => e.Price).HasColumnType("decimal(18, 2)");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.OrderId).HasName("PK__Order__C3905BAF8024CF80");

            entity.ToTable("Order");

            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.CustomerId).HasColumnName("CustomerID");
            entity.Property(e => e.EmpId).HasColumnName("EmpID");
            entity.Property(e => e.OrderDate)
                .HasDefaultValueSql("(getdate())")
                .HasColumnType("datetime");
            entity.Property(e => e.Status).HasMaxLength(50);

            entity.HasOne(d => d.Customer).WithMany(p => p.Orders)
                .HasForeignKey(d => d.CustomerId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__Order__CustomerI__4222D4EF");

            entity.HasOne(d => d.Emp).WithMany(p => p.Orders)
                .HasForeignKey(d => d.EmpId)
                .HasConstraintName("FK__Order__EmpID__4316F928");
        });

        modelBuilder.Entity<OrderDetail>(entity =>
        {
            entity.HasKey(e => e.OrderDetailId).HasName("PK__OrderDet__D3B9D30C20D2EF4B");

            entity.ToTable("OrderDetail");

            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");
            entity.Property(e => e.TotalAmount).HasColumnType("decimal(18, 2)");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetails)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__45F365D3");
        });

        modelBuilder.Entity<OrderDetailIceCream>(entity =>
        {
            entity.HasKey(e => e.OrderDetailIceCreamId).HasName("PK__OrderDet__71A179FCD8C7F864");

            entity.ToTable("OrderDetailIceCream");

            entity.Property(e => e.OrderDetailIceCreamId).HasColumnName("OrderDetailIceCreamID");
            entity.Property(e => e.IceCreamId).HasColumnName("IceCreamID");
            entity.Property(e => e.OrderDetailId).HasColumnName("OrderDetailID");
            entity.Property(e => e.OrderId).HasColumnName("OrderID");

            entity.HasOne(d => d.IceCream).WithMany(p => p.OrderDetailIceCreams)
                .HasForeignKey(d => d.IceCreamId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__IceCr__4AB81AF0");

            entity.HasOne(d => d.OrderDetail).WithMany(p => p.OrderDetailIceCreams)
                .HasForeignKey(d => d.OrderDetailId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__48CFD27E");

            entity.HasOne(d => d.Order).WithMany(p => p.OrderDetailIceCreams)
                .HasForeignKey(d => d.OrderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__OrderDeta__Order__49C3F6B7");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
