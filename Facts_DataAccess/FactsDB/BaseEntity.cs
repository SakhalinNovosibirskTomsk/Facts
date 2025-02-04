using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facts_DataAccess.FactsDB
{
    /// <summary>
    /// Базовый класс сущностей
    /// </summary>
    public class BaseEntity
    {
        /// <summary>
        /// ИД сущности
        /// </summary>
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Comment("ИД записи")]
        public int Id { get; set; }
    }
}
