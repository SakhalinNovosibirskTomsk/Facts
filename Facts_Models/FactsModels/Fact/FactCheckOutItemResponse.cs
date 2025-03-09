using Facts_Models.FactsModels.State;

namespace Facts_Models.FactsModels.Fact
{

    /// <summary>
    /// Информация о выдаче экземпляра книги
    /// </summary>
    public class FactCheckOutItemResponse
    {
        /// <summary>
        /// ИД факта
        /// </summary>
        public int Id { get; set; }

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
        /// ИД читателя, которому выдан экземпляр книги
        /// </summary>
        public int MemberId { get; set; }

        /// <summary>
        /// ИД пользователя, добавившего запись о выдаче экземпляра книги читателю
        /// </summary>
        public Guid GiveUserId { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Статус состояния экземпляра книги при выдаче читателю
        /// </summary>
        public StateItemResponse StateOut { get; set; }
    }
}
