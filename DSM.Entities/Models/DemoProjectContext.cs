using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DSM.Entities.Models;

public partial class DemoProjectContext : DbContext
{
    public DemoProjectContext()
    {
    }

    public DemoProjectContext(DbContextOptions<DemoProjectContext> options)
        : base(options)
    {
    }

    public virtual DbSet<SignUp> SignUps { get; set; }

    public virtual DbSet<UserType> UserTypes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=MOBACK;Initial Catalog=DemoProject;Integrated Security=True; TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SignUp>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__SignUp__1788CC4CA5276BAE");

            entity.ToTable("SignUp");

            entity.Property(e => e.CreatedDate).HasColumnType("date");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.Password)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.UpdatedDate).HasColumnType("date");

            entity.HasOne(d => d.TypeOfUserNavigation).WithMany(p => p.SignUps)
                .HasForeignKey(d => d.TypeOfUser)
                .HasConstraintName("FK__SignUp__TypeOfUs__267ABA7A");
        });

        modelBuilder.Entity<UserType>(entity =>
        {
            entity.HasKey(e => e.UserTypeId).HasName("PK__UserType__40D2D816F3C1184C");

            entity.ToTable("UserType");

            entity.Property(e => e.UserType1)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("UserType");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
