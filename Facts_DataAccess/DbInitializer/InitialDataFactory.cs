using Facts_Common;
using Facts_Domain.FactsDB;

namespace Facts_DataAccess.DbInitializer
{

    /// <summary>
    /// Данные для начального заполенния БД
    /// </summary>
    public static class InitialDataFactory
    {

        /// <summary>
        /// Факты
        /// </summary>
        public static List<Fact> Facts => new List<Fact>()
        {
            new Fact()
               {
                Id = 1,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-60),
                PlanDateOfReturn = DateTime.Now.AddDays(-46),
                DateOfReturn = DateTime.Now.AddDays(-50),
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = SD.UserIdForInitialData,
                StateIdOut = 1,
                StateIdIn = 1
                },
            new Fact()
               {
                Id = 2,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-49),
                PlanDateOfReturn = DateTime.Now.AddDays(-35),
                DateOfReturn = DateTime.Now.AddDays(-40),
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = SD.UserIdForInitialData,
                StateIdOut = 1,
                StateIdIn = 3
                },
            new Fact()
               {
                Id = 3,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-30),
                PlanDateOfReturn = DateTime.Now.AddDays(-16),
                DateOfReturn = DateTime.Now.AddDays(-25),
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = SD.UserIdForInitialData,
                StateIdOut = 3,
                StateIdIn = 3
                },
            new Fact()
               {
                Id = 4,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-24),
                PlanDateOfReturn = DateTime.Now.AddDays(-10),
                DateOfReturn = DateTime.Now.AddDays(-20),
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = SD.UserIdForInitialData,
                StateIdOut = 3,
                StateIdIn = 4
                },

            new Fact()
               {
                Id = 5,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-16),
                PlanDateOfReturn = DateTime.Now.AddDays(-2),
                DateOfReturn = DateTime.Now.AddDays(-8),
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = SD.UserIdForInitialData,
                StateIdOut = 4,
                StateIdIn = 5
                },
            new Fact()
               {
                Id = 6,
                BookInstanceId  = 1,
                FromDate = DateTime.Now.AddDays(-2),
                PlanDateOfReturn = DateTime.Now.AddDays(12),
                DateOfReturn = null,
                MemberId = SD.MemberIdForInitialData,
                GiveUserId = SD.UserIdForInitialData,
                ReturnUserId = null,
                StateIdOut = 5,
                StateIdIn = null
                },
        };

        /// <summary>
        /// Комментарии к фактам
        /// </summary>
        public static List<FactComment> FactComments => new List<FactComment>()
        {
            new FactComment
                {
                    Id = 1,
                    FactId = 2,
                    BookInstanceId = 1,
                    Comment = "На 2-х сраницах есть небольшие надписи ручкой. Одна страница не сильно порвана."
                },
            new FactComment
                {
                    Id = 2,
                    FactId = 4,
                    BookInstanceId = 1,
                    Comment = "На обложке и некоторых страницах жирные пятна. Есть незначительно порваные страницы. Одна страница порвана пополам, но склеена скотчем."
                },
            new FactComment
                {
                    Id = 3,
                    FactId = 5,
                    BookInstanceId = 1,
                    Comment = "На обложке и некоторых страницах жирные пятна. Некоторые (больше 10) сраниц склеены скотчем. Страницы с содержанием утеряны."
                },
        };
    }
}
