using System.ComponentModel.DataAnnotations;

namespace Facts_Models.FactsModels.State
{

    /// <summary>
    /// Объект-запрос создания нового статуса
    /// </summary>
    public class StateItemCreateRequest
    {

        /// <summary>
        /// Наименование статуса
        /// </summary>
        [Required]
        [MaxLength(100)]
        [MinLength(1)]
        public string Name { get; set; }

        /// <summary>
        /// Описание статуса
        /// </summary>
        [MaxLength(1000)]
        public string Description { get; set; }

        /// <summary>
        /// Признак, что это первоначальный статус (статус по умолчанию при поступлении экземпляра книги
        /// </summary>
        public bool IsInitialState { get; set; } = false;

        /// <summary>
        /// Признак, что выставление данног статуса при возврате книги читателем требует обязательного комментария
        /// </summary>
        public bool IsNeedComment { get; set; } = false;

        /// <summary>
        /// Признак, что статус удалён в архив
        /// </summary>
        public bool IsArchive { get; set; } = false;
    }
}
