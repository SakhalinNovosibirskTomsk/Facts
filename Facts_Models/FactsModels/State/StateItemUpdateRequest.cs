using System.ComponentModel.DataAnnotations;

namespace Facts_Models.FactsModels.State
{

    /// <summary>
    /// Объект-запрос обнеовления статуса состояния экземпляра книги
    /// </summary>
    public class StateItemUpdateRequest
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
        /// Признак, что при выставлении статуса при возврате от читателя экземпляра книги обязателен комментарий
        /// </summary>
        public bool IsNeedComment { get; set; } = false;
    }
}
