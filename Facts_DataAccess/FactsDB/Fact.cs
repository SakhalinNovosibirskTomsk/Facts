using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Facts_DataAccess.FactsDB
{

    /// <summary>
    /// Факт выдачи читателю/возврата от читателя экземпляра книги
    /// </summary>
    [Table("Facts")]
    [Comment("Справочник авторов книг")]
    public class Fact : BaseEntity
    {
        /// <summary>
        /// ИД экземпляра книги
        /// </summary>
        [Required]
        [Comment("ИД экземпляра книги")]
        public int BookInstanceId { get; set; }

        /// <summary>
        /// Дата выдачи экземпляра книги читателю
        /// </summary>
        [Required]
        [Comment("Дата выдачи экземпляра книги читателю")]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Плановая дата возврата экземпляра книги читателем
        /// </summary>
        [Required]
        [Comment("Плановая дата возврата экземпляра книги читателем")]
        public DateTime PlanDateOfReturn { get; set; }

        /// <summary>
        /// Дата возврата экземпляра книги читателем
        /// </summary>
        [Comment("Дата возврата экземпляра книги читателем")]
        public DateTime? DateOfReturn { get; set; }

        /// <summary>
        /// ИД читателя, которому выдан экземпляр книги
        /// </summary>
        [Required]
        [Comment("ИД читателя, которому выдан экземпляр книги")]
        public int MemberId { get; set; }

        /// <summary>
        /// ИД пользователя, добавившего запись о выдаче экземпляра книги читателю
        /// </summary>
        [Required]
        [Comment("ИД пользователя, добавившего запись о выдаче экземпляра книги читателю")]
        public Guid GiveUserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ИД пользователя, добавившего информацию о возврате экземпляра книги читателем
        /// </summary>
        [Required]
        [Comment("ИД пользователя, добавившего информацию о возврате экземпляра книги читателем")]
        public Guid? ReturnUserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ИД статуса состояния экземпляра книги при выдаче читателю
        /// </summary>
        [Required]
        [Comment("ИД статуса состояния экземпляра книги при выдаче читателю")]
        public int StateIdOut { get; set; }

        /// <summary>
        /// ИД статуса состояния экземпляра книги при возхврате от читателя
        /// </summary>
        [Comment("ИД статуса состояния экземпляра книги при возхврате от читателя")]
        public int? StateIdIn { get; set; }
    }
}
