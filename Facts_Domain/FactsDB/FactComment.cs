namespace Facts_Domain.FactsDB
{

    /// <summary>
    /// Комментарий к факту выдачи/возврата экземпляров книг
    /// </summary>
    public class FactComment : BaseEntity
    {
        /// <summary>
        /// ИД факта
        /// </summary>
        public int FactId { get; set; }

        /// <summary>
        /// Факт выдачи/возврата экземпляра книги
        /// </summary>
        public virtual Fact Fact { get; set; }

        /// <summary>
        /// ИД Экземпляра книги
        /// </summary>
        public int BookInstanceId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string Comment { get; set; }
    }
}
