namespace Facts_Models.FactsModels.Fact
{

    /// <summary>
    /// Запрос на добавление факта возврата экземпляра книги читателем
    /// </summary>
    public class FactReturnAddItemRequest
    {

        /// <summary>
        /// ИД статуса состояния возвращаемого читателем экземпляра книги
        /// </summary>
        public int StateIdIn { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        public string? Comment { get; set; }
    }
}
