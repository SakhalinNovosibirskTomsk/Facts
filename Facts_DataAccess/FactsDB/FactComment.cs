using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facts_DataAccess.FactsDB
{

    /// <summary>
    /// Комментарий к факту выдачи/возврата экземпляров книг
    /// </summary>
    [Table("FactComments")]
    [Index("FactId", "BookInstanceId" IsUnique = true, Name = "FactIdBookInstanceIdUnique")]
    [Comment("Таблица комментариев фактов выдачи/возврата экземпляров книг")]
    public class FactComment : BaseEntity
    {
        [Required]
        [Comment("ИД факта")]
        public int FactId { get; set; }

        /// <summary>
        /// Факт выдачи/возврата экземпляра книги
        /// </summary>
        public Fact Fact { get; set; }

        /// <summary>
        /// ИД Экземпляра книги
        /// </summary>
        [Required]
        [Comment("ИД экземпляра книги")]
        public int BookInstanceId { get; set; }

        /// <summary>
        /// Комментарий
        /// </summary>
        [Required]
        [MaxLength(300)]
        [MinLength(3)]
        [Comment("Комментарий")]
        public string Comment { get; set; }
    }
}
