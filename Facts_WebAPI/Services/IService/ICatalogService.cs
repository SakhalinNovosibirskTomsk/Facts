namespace Facts_WebAPI.Controllers.Services.IServices
{
    /// <summary>
    /// Интрефейс для запросов данных у сервиса Catalog
    /// </summary>
    public interface ICatalogService : IBaseService
    {
        /// <summary>
        /// Запрос максимального количества дней, на которые можно выдать экземпляр книги
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого отбъекта</typeparam>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает объект ответа сервиса</returns>
        Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id);

        /// <summary>
        /// Запрос признака, что экземпляр книги выдаётся только в читальный зал
        /// </summary>
        /// <typeparam name="T">Тип возвращаемого отбъекта</typeparam>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает объект ответа сервиса</returns>
        Task<T> GetOnlyForReadingRoomByBookInstanceIdAsync<T>(int id);
    }
}
