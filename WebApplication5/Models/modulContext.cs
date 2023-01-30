using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace WebApplication5.Models
{
    public partial class modulContext : DbContext
    {
        public modulContext()
        {
        }

        public modulContext(DbContextOptions<modulContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Budget> Budgets { get; set; }
        public virtual DbSet<Employee> Employees { get; set; }
        public virtual DbSet<FinishedProduct> FinishedProducts { get; set; }
        public virtual DbSet<Ingredient> Ingredients { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Production> Productions { get; set; }
        public virtual DbSet<PurchaseOfRawMaterial> PurchaseOfRawMaterials { get; set; }
        public virtual DbSet<RawMaterial> RawMaterials { get; set; }
        public virtual DbSet<Salary> Salaries { get; set; }
        public virtual DbSet<SaleOfProduct> SaleOfProducts { get; set; }
        public virtual DbSet<Unit> Units { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server=LAPTOP-2QH9RIKC;Database=modul;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Budget>(entity =>
            {
                entity.ToTable("Budget");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.СуммаБюджета)
                    .HasColumnType("money")
                    .HasColumnName("сумма бюджета");
            });

            modelBuilder.Entity<Employee>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Адрес)
                    .HasMaxLength(40)
                    .IsUnicode(false);

                entity.Property(e => e.Оклад).HasColumnType("money");

                entity.Property(e => e.Телефон)
                    .HasMaxLength(50)
                    .IsUnicode(false)
                    .IsFixedLength(true);

                entity.Property(e => e.Фио)
                    .HasMaxLength(40)
                    .IsUnicode(false)
                    .HasColumnName("ФИО");

                entity.HasOne(d => d.Должность_)
                    .WithMany(p => p.Employees)
                    .HasForeignKey(d => d.Должность)
                    .HasConstraintName("FK__Employees__posit__440B1D61");
            });

            modelBuilder.Entity<FinishedProduct>(entity =>
            {
                entity.Property(e => e.Id)
                    .ValueGeneratedNever()
                    .HasColumnName("ID");

                entity.Property(e => e.ЕдиницаИзмерения).HasColumnName("Единица измерения");

                entity.Property(e => e.Количество).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Наименование)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Сумма).HasColumnType("money");

                entity.HasOne(d => d.ЕдиницаИзмерения_)
                    .WithMany(p => p.FinishedProducts)
                    .HasForeignKey(d => d.ЕдиницаИзмерения)
                    .HasConstraintName("FK_FinishedProducts_Units");
            });

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Количество).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Продукция_)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.Продукция)
                    .HasConstraintName("FK_Ingredients_FinishedProducts");

                entity.HasOne(d => d.Сырьё_)
                    .WithMany(p => p.Ingredients)
                    .HasForeignKey(d => d.Сырьё)
                    .HasConstraintName("FK_Ingredients_RawMaterials");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Должность)
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("должность");
            });

            modelBuilder.Entity<Production>(entity =>
            {
                entity.ToTable("Production");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Дата).HasColumnType("date");

                entity.Property(e => e.Количество).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Продукция_)
                    .WithMany(p => p.Productions)
                    .HasForeignKey(d => d.Продукция)
                    .HasConstraintName("FK_Production_FinishedProducts");

                entity.HasOne(d => d.Сотрудники)
                   .WithMany(p => p.Productions)
                   .HasForeignKey(d => d.Сотрудник)
                   .HasConstraintName("FK_Production_Employees");

            });

            modelBuilder.Entity<PurchaseOfRawMaterial>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Дата)
                    .HasColumnType("date")
                    .HasColumnName("дата");

                entity.Property(e => e.Количество)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("количество");

                entity.Property(e => e.Сотрудник).HasColumnName("сотрудник");

                entity.Property(e => e.Сумма)
                    .HasColumnType("money")
                    .HasColumnName("сумма");

                entity.HasOne(d => d.Сотрудник_)
                    .WithMany(p => p.PurchaseOfRawMaterials)
                    .HasForeignKey(d => d.Сотрудник)
                    .HasConstraintName("FK__PurchaseO__emplo__47DBAE45");

                entity.HasOne(d => d.Сырьё_)
                    .WithMany(p => p.PurchaseOfRawMaterials)
                    .HasForeignKey(d => d.Сырьё)
                    .HasConstraintName("FK__PurchaseO__rawMa__46E78A0C");
            });

            modelBuilder.Entity<RawMaterial>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.ЕдиницаИзмерения).HasColumnName("Единица измерения");

                entity.Property(e => e.Количество).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Наименование)
                    .HasMaxLength(30)
                    .IsUnicode(false);

                entity.Property(e => e.Сумма).HasColumnType("money");

                entity.HasOne(d => d.ЕдиницаИзмерения_)
                    .WithMany(p => p.RawMaterials)
                    .HasForeignKey(d => d.ЕдиницаИзмерения)
                    .HasConstraintName("FK__RawMateria__unit__2B3F6F97");
            });

            modelBuilder.Entity<Salary>(entity =>
            {
                entity.HasKey(e => e.IdЗарплата);

                entity.ToTable("Salary");

                entity.Property(e => e.IdЗарплата).HasColumnName("Id_зарплата");

                entity.Property(e => e.Закупка)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Зарплата).HasColumnType("money");

                entity.Property(e => e.ОбщееКоличество).HasColumnName("Общее количество");

                entity.Property(e => e.Продажа)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Производство)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.Сотрудник_)
                    .WithMany(p => p.Salaries)
                    .HasForeignKey(d => d.Сотрудник)
                   .HasConstraintName("FK_Salary_Employees");
                 //.HasConstraintName("FK__SaleOfPro__emplo__4CA06362");
            });

            modelBuilder.Entity<SaleOfProduct>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Дата)
                    .HasColumnType("date")
                    .HasColumnName("дата");

                entity.Property(e => e.Количество)
                    .HasColumnType("decimal(18, 0)")
                    .HasColumnName("количество");

                entity.Property(e => e.Продукция).HasColumnName("продукция");

                entity.Property(e => e.Сотрудник).HasColumnName("сотрудник");

                entity.Property(e => e.Сумма)
                    .HasColumnType("money")
                    .HasColumnName("сумма");

                entity.HasOne(d => d.Продукция_)
                    .WithMany(p => p.SaleOfProducts)
                    .HasForeignKey(d => d.Продукция)
                    .HasConstraintName("FK__SaleOfPro__produ__4BAC3F29");

                entity.HasOne(d => d.Сотрудник_)
                    .WithMany(p => p.SaleOfProducts)
                    .HasForeignKey(d => d.Сотрудник)
                    .HasConstraintName("FK__SaleOfPro__emplo__4CA06362");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Наименование)
                    .HasMaxLength(30)
                    .IsUnicode(false);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
