namespace Facts_Models.FactsModels.State
{

    /// <summary>
    /// Статус состояния экземпляра книги (объект-ответ)
    /// </summary>
    public class StateItemResponse
    {
        /// <summary>
        /// ИД статуса
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Наименование статуса
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Описание статуса
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Признак начального статуса экземпляра книги (статуса по умолчанию)
        /// </summary>
        public bool IsInitialState { get; set; } = false;

        /// <summary>
        /// Признак, что при выставлении данного статуса при возврате книги от чимтатетля обязателен комментарий
        /// </summary>
        public bool IsNeedComment { get; set; } = false;

        /// <summary>
        /// Признак удаления статуса в архив
        /// </summary>
        public bool IsArchive { get; set; }
    }
}
