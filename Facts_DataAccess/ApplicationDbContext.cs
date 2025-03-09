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
        /// Статусы состояния экземпляра книги
        /// </summary>
        public DbSet<State> States { get; set; }

        /// <summary>
        /// Факт выдачи читателю/возврата от читателя экземпляра книги
        /// </summary>
        public DbSet<Fact> Facts { get; set; }

        /// <summary>
        /// Факт выдачи читателю/возврата от читателя экземпляра книги
        /// </summary>
        public DbSet<FactComment> FactComments { get; set; }

        /// <summary>
        /// Информация о выдаче, бронировании и списании экземпляров книг
        /// </summary>
        public DbSet<BookInstance> BookInstances { get; set; }


        /// <summary>
        /// Настройка сопоставления модели данных с БД
        /// </summary>
        /// <param name="modelBuilder">Объект построителя модели</param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // TODO разнести для каждой сужности в отдельный класс или метод настройку БД

            //------------------------------------------------------------------
            // Статусы
            //------------------------------------------------------------------
            modelBuilder.Entity<State>()
                .ToTable("States")
                .ToTable(t => t.HasComment("Справочник статусов состояния экземпляров книг"));


            modelBuilder.Entity<State>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<State>()
                .HasIndex(nameof(State.Name))
                .IsUnique()
                .HasDatabaseName(nameof(State) + nameof(State.Name));

            modelBuilder.Entity<State>()
                .Property(u => u.Id)
                .HasComment("ИД записи")
                .IsRequired();

            modelBuilder.Entity<State>()
                .Property(u => u.Name)
                .HasComment("Наименование статуса состояния экземпляра книги")
                .IsRequired()
                .HasMaxLength(100);

            modelBuilder.Entity<State>()
                .Property(u => u.Description)
                .HasComment("Описание статуса состояния экземпляра книги")
                .HasMaxLength(1000);

            modelBuilder.Entity<State>()
                .Property(u => u.IsInitialState)
                .HasComment("Признак, что состояние является исходным (например, присваивается по умолчанию при поступлении нового экземпляра книги)")
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<State>()
                .Property(u => u.IsNeedComment)
                .HasComment("Признак, что при выставлении данного состояния экземпляру книги (например, при возврате) требуется обязательный комментарий")
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<State>()
                .Property(u => u.IsArchive)
                .HasComment("Признак удаления записи в архив")
                .IsRequired()
                .HasDefaultValue(false);


            //------------------------------------------------------------------
            // Факты выдачи читателю/возврата от читателя экземпляра книги
            //------------------------------------------------------------------
            modelBuilder.Entity<Fact>()
                .ToTable("Facts")
                .ToTable(t => t.HasComment("Факты выдачи читателю/возврата от читателя экземпляра книги"))
                .HasOne(s => s.StateIn)
                .WithMany(b => b.FactListIn)
                .HasForeignKey(k => k.StateIdIn);

            modelBuilder.Entity<Fact>()
                .HasOne(s => s.StateOut)
                .WithMany(b => b.FactListOut)
                .HasForeignKey(k => k.StateIdOut);

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

            //------------------------------------------------------------------
            // Информация о выдаче, бронировании и списании экземпляров книг
            //------------------------------------------------------------------
            modelBuilder.Entity<BookInstance>()
                .ToTable("BookInstances")
                .ToTable(t => t.HasComment("Информация о выдаче, бронировании и списании экземпляров книг"));


            modelBuilder.Entity<BookInstance>()
                .HasKey(u => u.Id);

            modelBuilder.Entity<BookInstance>()
                .Property(u => u.Id)
                .HasComment("ИД экземпляра книги")
                .IsRequired();

            modelBuilder.Entity<BookInstance>()
                .Property(u => u.IsCheckedOut)
                .HasComment("Признак выдан ли экземпляр книги читателю")
                .IsRequired()
                .HasDefaultValue(false);

            modelBuilder.Entity<BookInstance>()
                .Property(u => u.IsReservedMemberId)
                .HasComment("ИД читателя, забронировавшего экземпляр книги")
                .IsRequired()
                .HasDefaultValue(0);

            modelBuilder.Entity<BookInstance>()
                .Property(u => u.IsWrittenOff)
                .HasComment("Признак списан ли экземпляр книги")
                .IsRequired()
                .HasDefaultValue(false);

            //------------------------------------------------------------------

        }
    }
}
