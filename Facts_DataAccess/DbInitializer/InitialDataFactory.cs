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
        /// Статусы состояния экземпляров книги
        /// </summary>
        public static List<State> States => new List<State>()
        {
            new State
            {
                Id = 1,
                Name = "Отличное",
                Description = "Новая или \"как новая\". Присваивается при поступлении нового экземпляра книги в библиотеку. Остаётся до тех пор, пока книга "+
                                "не измент своё сотояние на отличное от изначального",
                IsInitialState = true,
                IsNeedComment  = false,
                IsArchive = false,
            },
             new State
            {
                Id = 2,
                Name = "Хорошее",
                Description = "Незначительные потёртости на обложке, пожелтение страниц и прочие \"повреждения временем\" "+
                                "без следов умышленной порчи или вследвие небрежного отношения",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
             new State
             {
                Id = 3,
                Name = "Среднее",
                Description = "Есть следы умышленного или вследсвие небрежного отношения повреждения - мятые страницы, небольшие (не больше 3 см) порывы страниц. "+
                               "Следы чернил, надписей или иных загрязнений на некоторых страницах и/или обложке, не носящие массовый характер и не " +
                               "мешающие восприятию текста, картинок, рисунков и прочего материала книги",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
             new State
             {
                Id = 4,
                Name = "Удовлетворительное",
                Description = "Экземпляр заметно отличается от изначального (нового). Загрязнения, рваные страницы, надписи/заметки на страницах и обложке. " +
                 "При этом состояние не откладывает существенного влияние на возможность восприятия информации, представленной в книге",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
             new State
             {
                Id = 5,
                Name = "Плохое",
                Description = "Рваные или вырваные страницы, сильные повреждения обложки, большое количество надписей и загрязнений. Инфоомация считываема, но " +
                    "некоторые предложения могут быть не полностью читаемы.",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
             new State
             {
                Id = 6,
                Name = "Под списание",
                Description = "Рваные или вырваные страницы. Некоторые страницы утеряны или информация на них невоспроизводима или воспроизводима " +
                    "со значительными затруднениями. Состояние, которое даже при редкости экземпляра книги не позволяет полноценно ей пользоваться " +
                    "и возникает вопрос о целесообразности хранения его в библиотеке",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
             new State
             {
                Id = 7,
                Name = "Устарела",
                Description = "Особый статус, обозначающий неактуальность информации отражённой к экземпляре вплоть до введения читателя в заблждение." +
                    "Например, научная литература, объясняющая теорию, которая из-за новых исследований опровергнута и её использование при подготовке " +
                    "к экзаменам или работам учащимися может привести к снижению оценки или не сдаче работы или предмета. " +
                    "Статус может быть похож на статус \"Под списание\", если специалисты по предмету решат, что наличие книги в библиотеке и возможность " +
                    "ознакомления с её содержимым учащимися несёт больше вреда, чем пользы от возможности удовлетворения академического интереса по изучению "+
                    "истоиии предмета или теории.",
                IsInitialState = false,
                IsNeedComment  = true,
                IsArchive = false,
             },
        };

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
