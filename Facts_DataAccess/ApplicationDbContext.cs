using Facts_Domain.FactsDB;
using Microsoft.EntityFrameworkCore;

namespace Facts_DataAccess
{

    /// <summary>
    /// DbContext
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        /// <summary>
        /// DbContext приложения - конструктор
        /// </summary>
        /// <param name="options"></param>
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }

        /// <summary>
        /// Факт выдачи читателю/возврата от читателя экземпляра книги
        /// </summary>
        public DbSet<Fact> Facts { get; set; }

        /// <summary>
        /// Факт выдачи читателю/возврата от читателя экземпляра книги
        /// </summary>
        public DbSet<FactComment> FactComments { get; set; }


        /// <summary>
        /// Настройка сопоставления модели данных с БД
        /// </summary>
        /// <param name="modelBuilder">Объект построителя модели</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO разнести для каждой сужности в отдельный класс или метод настройку БД

            //------------------------------------------------------------------
            // Факты выдачи читателю/возврата от читателя экземпляра книги
            //------------------------------------------------------------------
            modelBuilder.Entity<Fact>()
                .ToTable("Facts")
                .ToTable(t => t.HasComment("Факты выдачи читателю/возврата от читателя экземпляра книги"));

            modelBuilder.Entity<Fact>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<Fact>()
                .Property(u => u.Id)
                .HasComment("ИД записи")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.BookInstanceId)
                .HasComment("ИД экземпляра книги")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.FromDate)
                .HasComment("Дата выдачи экземпляра книги читателю")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.PlanDateOfReturn)
                .HasComment("Плановая дата возврата экземпляра книги читателем")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.DateOfReturn)
                .HasComment("Дата возврата экземпляра книги читателем");

            modelBuilder.Entity<Fact>()
                .Property(u => u.MemberId)
                .HasComment("ИД читателя, которому выдан экземпляр книги")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.GiveUserId)
                .HasComment("ИД пользователя, добавившего запись о выдаче экземпляра книги читателю")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.ReturnUserId)
                .HasComment("ИД пользователя, добавившего информацию о возврате экземпляра книги читателем");

            modelBuilder.Entity<Fact>()
                .Property(u => u.StateIdOut)
                .HasComment("ИД статуса состояния экземпляра книги при выдаче читателю")
                .IsRequired();

            modelBuilder.Entity<Fact>()
                .Property(u => u.StateIdIn)
                .HasComment("ИД статуса состояния экземпляра книги при возхврате от читателя");


            //------------------------------------------------------------------

            //------------------------------------------------------------------
            // Комментарии к факту выдачи/возврата экземпляров книг
            //------------------------------------------------------------------
            modelBuilder.Entity<FactComment>()
                .ToTable("FactComments")
                .ToTable(t => t.HasComment("Таблица комментариев фактов выдачи/возврата экземпляров книг"))
                .HasOne(s => s.Fact)
                .WithMany(b => b.FactCommentList)
                .HasForeignKey(k => k.FactId);

            modelBuilder.Entity<FactComment>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<FactComment>()
                .Property(u => u.Id)
                .HasComment("ИД записи")
                .IsRequired();

            modelBuilder.Entity<FactComment>()
                .HasIndex(nameof(FactComment.FactId), nameof(FactComment.BookInstanceId))
                .IsUnique()
                .HasDatabaseName("FactIdBookInstanceIdUnique");

            modelBuilder.Entity<FactComment>()
                .Property(u => u.FactId)
                .HasComment("ИД факта выдачи/возврата экземпляра книги")
                .IsRequired();

            modelBuilder.Entity<FactComment>()
                .Property(u => u.BookInstanceId)
                .HasComment("ИД экземпляра книги")
                .IsRequired();

            modelBuilder.Entity<FactComment>()
                .Property(u => u.Comment)
                .HasComment("Комментарий")
                .HasMaxLength(300)
                .IsRequired();

            //------------------------------------------------------------------
        }
    }
}
