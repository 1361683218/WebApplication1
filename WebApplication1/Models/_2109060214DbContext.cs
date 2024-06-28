using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace WebApplication1.Models;

public partial class _2109060214DbContext : DbContext
{
    public _2109060214DbContext()
    {
    }

    public _2109060214DbContext(DbContextOptions<_2109060214DbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Course> Courses { get; set; }

    public virtual DbSet<Enrollment> Enrollments { get; set; }

    public virtual DbSet<Student> Students { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("persist security info=True;data source=rm-bp1tg219t7o5j5ex8fo.rwlb.rds.aliyuncs.com;port=3306;initial catalog=2109060214_db;user id=2109060214;password=2109060214;character set=utf8;allow zero datetime=true;convert zero datetime=true;pooling=true;maximumpoolsize=3000", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.34-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb3_general_ci")
            .HasCharSet("utf8mb3");

        modelBuilder.Entity<Course>(entity =>
        {
            entity.HasKey(e => e.CourseCode).HasName("PRIMARY");

            entity.ToTable("courses");

            entity.Property(e => e.CourseCode)
                .HasMaxLength(20)
                .HasComment("课程代码，主键");
            entity.Property(e => e.CourseName)
                .HasMaxLength(20)
                .HasComment("课程名称");
            entity.Property(e => e.Credits).HasComment("学分");
        });

        modelBuilder.Entity<Enrollment>(entity =>
        {
            entity.HasKey(e => e.EnrollmentId).HasName("PRIMARY");

            entity.ToTable("enrollments");

            entity.HasIndex(e => e.CourseCode, "CourseCode");

            entity.HasIndex(e => e.StudentId, "StudentId");

            entity.Property(e => e.CourseCode).HasMaxLength(20);

            entity.HasOne(d => d.CourseCodeNavigation).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.CourseCode)
                .HasConstraintName("enrollments_ibfk_2");

            entity.HasOne(d => d.Student).WithMany(p => p.Enrollments)
                .HasForeignKey(d => d.StudentId)
                .HasConstraintName("enrollments_ibfk_1");
        });

        modelBuilder.Entity<Student>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("student");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasComment("学号，主键");
            entity.Property(e => e.ClassName)
                .HasMaxLength(64)
                .HasComment("班级");
            entity.Property(e => e.InitialPassword)
                .HasMaxLength(20)
                .HasComment("初始密码，默认学号");
            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .HasComment("姓名");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
