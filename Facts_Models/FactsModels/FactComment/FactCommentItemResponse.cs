namespace Facts_Models.FactsModels.FactComment
{

    /// <summary>
    /// Комментарий к факту выдачи/возврата экземпляров книг
    /// </summary>
    public class FactCommentItemResponse
    {
        /// <summary>
        /// ИД комментария
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// ИД факта
        /// </summary>
        public int FactId { get; set; }

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
