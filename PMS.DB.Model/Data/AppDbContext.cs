using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PMS.DB.Model.EF.Models;

namespace PMS.DB.Model.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<PmsLookup> PmsLookups { get; set; }

    public virtual DbSet<PmsProduct> PmsProducts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<PmsLookup>(entity =>
        {
            entity.HasKey(e => e.LookupId).HasName("PK__PMS_Look__9A5F75E05E3D92AC");

            entity.Property(e => e.ActiveFlag).HasDefaultValue(true);
            entity.Property(e => e.CreatedDate).HasDefaultValueSql("(getdate())");
        });

        modelBuilder.Entity<PmsProduct>(entity =>
        {
            entity.HasKey(e => e.ProductId).HasName("PK__PMS_Prod__B40CC6CD06159BDB");

            entity.Property(e => e.ActiveFlag).HasDefaultValue(true);

            entity.HasOne(d => d.CategoryLkp).WithMany(p => p.PmsProducts)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK__PMS_Produ__Categ__3B75D760");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
