namespace Facts_Models.FactsModels.BookInstance
{

    /// <summary>
    /// Информация о списании экземпляра книги
    /// </summary>
    public class BookInstanceIsWrittenOffResponse
    {
        /// <summary>
        /// Признак списан ли экземпляр книги
        /// </summary>        
        public bool IsWrittenOff { get; set; } = false;
    }
}
