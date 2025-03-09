namespace Facts_Models.FactsModels.BookInstance
{

    /// <summary>
    /// Информация читателе забронировавшем экземпляр книги
    /// </summary>
    public class BookInstanceIsReservedMemberIdResponse
    {

        /// <summary>
        /// ИД читателя, забронировавшего экземпляр книги
        /// </summary>        
        public int IsReservedMemberId { get; set; } = 0;
    }
}
