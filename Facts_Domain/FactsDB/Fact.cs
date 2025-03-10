﻿namespace Facts_Domain.FactsDB
{

    /// <summary>
    /// Факт выдачи читателю/возврата от читателя экземпляра книги
    /// </summary>
    public class Fact : BaseEntity
    {
        /// <summary>
        /// ИД экземпляра книги
        /// </summary>
        public int BookInstanceId { get; set; }

        /// <summary>
        /// Дата выдачи экземпляра книги читателю
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// Плановая дата возврата экземпляра книги читателем
        /// </summary>
        public DateTime PlanDateOfReturn { get; set; }

        /// <summary>
        /// Дата возврата экземпляра книги читателем
        /// </summary>        
        public DateTime? DateOfReturn { get; set; } = null;

        /// <summary>
        /// ИД читателя, которому выдан экземпляр книги
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// ИД пользователя, добавившего запись о выдаче экземпляра книги читателю
        /// </summary>
        public Guid GiveUserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// ИД пользователя, добавившего информацию о возврате экземпляра книги читателем
        /// </summary>
        public Guid? ReturnUserId { get; set; } = null;

        /// <summary>
        /// ИД статуса состояния экземпляра книги при выдаче читателю
        /// </summary>
        public int StateIdOut { get; set; }

        /// <summary>
        /// Статус состояния экземпляра книги при выдаче читателю
        /// </summary>
        public virtual State StateOut { get; set; }

        /// <summary>
        /// ИД статуса состояния экземпляра книги при возврате от читателя
        /// </summary>        
        public int? StateIdIn { get; set; } = null;

        /// <summary>
        /// Статус состояния экземпляра книги при возврте от читателю
        /// </summary>
        public virtual State? StateIn { get; set; }

        /// <summary>
        /// Комментарий к статусу возврата экземпляра книги читателем
        /// </summary>
        public virtual FactComment? FactComment { get; set; } = null;
    }
}
