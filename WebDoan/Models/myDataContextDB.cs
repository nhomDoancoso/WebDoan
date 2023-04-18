using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace WebDoan.Models
{
    public partial class myDataContextDB : DbContext
    {
        public myDataContextDB()
            : base("name=myDataContextDB2")
        {
        }

        public virtual DbSet<COMBODICHVU> COMBODICHVU { get; set; }
        public virtual DbSet<CHUCVU> CHUCVU { get; set; }
        public virtual DbSet<DICHVU> DICHVU { get; set; }
        public virtual DbSet<HOADON> HOADON { get; set; }
        public virtual DbSet<KHACHHANG> KHACHHANG { get; set; }
        public virtual DbSet<LOAIDICHVU> LOAIDICHVU { get; set; }
        public virtual DbSet<NHANVIEN> NHANVIEN { get; set; }
        public virtual DbSet<PHIEUDAT> PHIEUDAT { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<COMBODICHVU>()
                .HasMany(e => e.DICHVU)
                .WithMany(e => e.COMBODICHVU)
                .Map(m => m.ToTable("CTCOMBO").MapLeftKey("MaCB").MapRightKey("MaDV"));

            modelBuilder.Entity<COMBODICHVU>()
                .HasMany(e => e.PHIEUDAT)
                .WithMany(e => e.COMBODICHVU)
                .Map(m => m.ToTable("CTPHIEUDAT").MapLeftKey("MaCB").MapRightKey("MaPD"));

            modelBuilder.Entity<COMBODICHVU>()
                .HasMany(e => e.HOADON)
                .WithMany(e => e.COMBODICHVU)
                .Map(m => m.ToTable("CTHOADON").MapLeftKey("MaCB").MapRightKey("MaHD"));

            modelBuilder.Entity<DICHVU>()
                .Property(e => e.MaLoaiDV)
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Email)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.UserName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<KHACHHANG>()
                .Property(e => e.Password)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<LOAIDICHVU>()
                .Property(e => e.MaLoaiDV)
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.SDT)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.Email)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.UserName)
                .IsFixedLength()
                .IsUnicode(false);

            modelBuilder.Entity<NHANVIEN>()
                .Property(e => e.Password)
                .IsFixedLength()
                .IsUnicode(false);
        }
    }
}
