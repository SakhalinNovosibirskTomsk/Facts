using Facts_Models.FactsModels.FactComment;
using Facts_Models.FactsModels.State;

namespace Facts_Models.FactsModels.Fact
{

    /// <summary>
    /// Статус состояния экземпляра книги (объект-ответ)
    /// </summary>
    public class FactItemResponse
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
        /// Статус состояния экземпляра книги при выдаче читателю
        /// </summary>
        public StateItemResponse StateOut { get; set; }

        /// <summary>
        /// Статус состояния экземпляра книги при возврте от читателю
        /// </summary>
        public StateItemResponse? StateIn { get; set; }

        public FactCommentItemResponse? FactComment { get; set; }
    }
}
