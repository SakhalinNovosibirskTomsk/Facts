namespace Facts_Models.FactsModels.BookInstance
{

    /// <summary>
    /// Информация выдан ли экземпляр книги
    /// </summary>
    public class BookInstanceIsCheckedOutResponse
    {
        /// <summary>
        /// Признак выдан ли экземпляр книги читателю
        /// </summary>        
        public bool IsCheckedOut { get; set; } = false;

    }
}
