using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace DigitalRetailerPro.Models
{
    public partial class DigitalRetailersContext : DbContext
    {
        public DigitalRetailersContext()
        {
        }

        public DigitalRetailersContext(DbContextOptions<DigitalRetailersContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblAdmin> TblAdmin { get; set; }
        public virtual DbSet<TblLaptop> TblLaptop { get; set; }
        public virtual DbSet<TblUsers> TblUsers { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("server=(local);integrated security =true;database=DigitalRetailers");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblAdmin>(entity =>
            {
                entity.ToTable("tblAdmin");

                entity.Property(e => e.Email).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(20);
            });

            modelBuilder.Entity<TblLaptop>(entity =>
            {
                entity.ToTable("tblLaptop");

                entity.HasIndex(e => e.CidId);

                entity.HasIndex(e => e.SidId);

                entity.Property(e => e.Available)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.Brand).HasMaxLength(20);

                entity.Property(e => e.Configuration).HasMaxLength(20);
                entity.Property(e => e.PaymentMode).HasMaxLength(20);
                

                entity.HasOne(d => d.Cid)
                    .WithMany(p => p.TblLaptopCid)
                    .HasForeignKey(d => d.CidId);

                entity.HasOne(d => d.Sid)
                    .WithMany(p => p.TblLaptopSid)
                    .HasForeignKey(d => d.SidId);
            });

            modelBuilder.Entity<TblUsers>(entity =>
            {
                entity.ToTable("tblUsers");

                entity.Property(e => e.Email).HasMaxLength(20);

                entity.Property(e => e.Location).HasMaxLength(20);

                entity.Property(e => e.Mobile).HasMaxLength(20);

                entity.Property(e => e.Name).HasMaxLength(20);

                entity.Property(e => e.Role)
                    .HasColumnName("role")
                    .HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
