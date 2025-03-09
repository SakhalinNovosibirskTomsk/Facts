namespace Facts_Models.FactsModels.BookInstance
{

    /// <summary>
    /// Информация о выдаче, бронировании и списании экземпляров книг
    /// </summary>
    public class BookInstanceItemResponse
    {
        /// <summary>
        /// ИД экземпляра книги
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Признак выдан ли экземпляр книги читателю
        /// </summary>        
        public bool IsCheckedOut { get; set; } = false;

        /// <summary>
        /// ИД читателя, забронировавшего экземпляр книги
        /// </summary>        
        public int IsReservedMemberId { get; set; } = 0;

        /// <summary>
        /// Признак списан ли экземпляр книги
        /// </summary>        
        public bool IsWrittenOff { get; set; } = false;
    }
}
