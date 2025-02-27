using Facts_Domain.FactsDB;

namespace Facts_DataAccess.DbInitializer
{

    /// <summary>
    /// Инициализация БД - создание и наполнение начальными данными
    /// </summary>
    public class DbInitializer : IDbInitializer
    {

        private readonly ApplicationDbContext _db;

        /// <summary>
        /// Конструктор инициализации БД значениями по умолчанию
        /// </summary>
        /// <param name="db">Контекст БД приложения</param>
        public DbInitializer(ApplicationDbContext db)
        {
            _db = db;
        }


        /// <summary>
        /// Метод наполения БД значениями по умолчанию
        /// </summary>
        public void InitializeDb()
        {
            Console.WriteLine("Инициализация БД: Удаление БД ... ");
            _db.Database.EnsureDeleted();
            Console.WriteLine("Инициализация БД: Удаление БД - Выполнено");

            Console.WriteLine("Инициализация БД: Создание БД ... ");
            _db.Database.EnsureCreated();
            Console.WriteLine("Инициализация БД: Создание БД - Выполнено");

            Console.WriteLine("Инициализация БД: Заполнение таблицы Facts ... ");
            FillTable<Fact>(InitialDataFactory.Facts);
            Console.WriteLine("Инициализация БД: Заполнение таблицы Facts - Выполнено");

            Console.WriteLine("Инициализация БД: Заполнение таблицы FactComments ... ");
            FillTable<FactComment>(InitialDataFactory.FactComments);
            Console.WriteLine("Инициализация БД: Заполнение таблицы FactComments - Выполнено");

        }


        /// <summary>
        /// Метод заполнения таблицы БД для определённой сущности
        /// </summary>
        /// <typeparam name="T">Тип сущности</typeparam>
        /// <param name="tableList">Список данных для записи в БД</param>
        public void FillTable<T>(List<T> tableList)
        {
            using (var transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    foreach (var item in tableList)
                    {
                        _db.Add(item);
                    }
                    _db.SaveChanges();
                    transaction.Commit();

                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }

        }
    }
}
