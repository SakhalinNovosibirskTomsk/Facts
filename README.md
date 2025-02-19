## Описание сервиса Facts
Сервис Facts является частью системы Library - библиотека учебного заведения. 

**Назначение сервиса:**
* регистрация фактов выдачи экземпляров книг читателю
* регистрация фактов возврата экземпляров книг читателю
* ввод информации о состоянии экземпляра книги после возврата его читателем

**Роли пользователей, использующих функционал сервиса:**
* Администратор
* Библиотекарь
* Стажёр библиотекаря

**Не имеют доступа к сервису:**
* Незарегистрированный пользователь
* Зарегистрированный пользователь без подтверждения регистрации
* Зарегистрированный пользователь с подтверждением регистрации

**Пользователи имеющие доступ к сервису могут выполнять следующие функции:**
* Регистрация факта выдачи экземпляра книги читателю - внести/изменить/удалить информацию о факте выдачи экземпляра книги читателю
* Регистрация факта возврата экземпляра книги читателем - внести информацию о возврате, выбрать состояние книги при возврате читателем, ввести комментарий по состоянию книги, изменить информацию о возврате книги читателем
* просмотр списков фактов выдачи и возвратов книг
* поиск по фактам выдачи и возвратов книг
* просмотр комментариев к фактам возврата экземпляров книг читателем

**Межсервисное взаимодействие**

**С сервисом Calalog - Каталог книг:**
* получение информации о экземплярах книг - в библиотеке/не в библиотеке, статус состояния экземпляра, забронирована/не забронирована, возможность выдачи на дом или только в читальный зал и тп
* получение информации о книге - ИД, автор, дата издания, издатель, название и прочее
* максимальное кол-во дней, допустимое для выдачи на руки для вычисления плановой даты возврата
* список возможных состояний книг для выбора при возврате экземпляра читателем (справочник States сервиса Catalog)
* Передача сервису Calalog информации о выдаче экземпляра книги читателю и возврате от читателя, даты выдачи или возврата, текущего статуса состояния экземпляра книги

**С сервисом "Профиль читателя":**
* Список читателей библиотеки
* Информация о конкретном читателе

**С сервисом "Аутентификация, авторизация и регистрация":**
* Информация о текущем пользователе сервиса - ИД, роль, признак удаления в архив и тп


## Диаграмма вариантов использования сервиса Facts

[![Диаграмма вариантов использования сервиса Facts](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/UseCases%20сервиса%20Facts.drawio.png)](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/UseCases%20сервиса%20Facts.drawio.png)
[Ссылка на картинку](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/UseCases%20сервиса%20Facts.drawio.png)

[Ссылка на исходник схемы](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/UseCases%20сервиса%20Facts.drawio)

## БД сервиса Facts

### ER-диаграмма
[![ER-диаграмма БД сервиса Facts](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/БД%20сервиса%20Facts.drawio.png)](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/БД%20сервиса%20Facts.drawio.png)
[Ссылка на картинку](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/БД%20сервиса%20Facts.drawio.png)

[Ссылка на исходник схемы](https://github.com/den3011den/Library/blob/main/Docs/Services/Facts/БД%20сервиса%20Facts.drawio)


### Табличное описание

##### Facts - факты выдачи и возврата от читателей экземпляров книг

| Ключ                    | Наименование         | тип              | Описание                                             | внеш. ключ              | Доп инфо                                                            |
| ----------------------- | -------------------- | ---------------- | ---------------------------------------------------- | ----------------------- | ------------------------------------------------------------------- |
| <center>**PK**</center> | **Id**               | int              | Ид записи                                            |                         | not null, autoincrement                                             |
|                         | **BookInstanceId**   | int              | ИД экземпляра книги                                  | <center>**FK**</center> | not null, Связь по полю BookInstances.Id (сервис Каталог книг)      |
|                         | **FromDate**         | datetime         | дата выдачи                                          |                         | not null                                                            |
|                         | **PlanDateOfReturn** | datetime         | плановая дата возврата                               |                         | not null                                                            |
|                         | **DateOfReturn**     | datetime         | дата возврата                                        |                         |                                                                     |
|                         | **MemberId**         | int              | Ид читателя                                          | <center>**FK**</center> | not null, Связь по полю Members.Id (сервис Профиля читателя)        |
|                         | **GiveUserId**       | uniqueidentifier | ИД пользователя, выдавшего читателю книгу            | <center>**FK**</center> | not null, Связь по полю Users.Id (сервис Авторизация и регистрация) |
|                         | **ReturnUserId**     | uniqueidentifier | ИД пользователя, принявшего назад книгу от  читателя | <center>**FK**</center> | Связь по полю Users.Id (сервис Авторизация и регистрация)           |
|                         | **StateIdOut**       | int              | ИД статуса состояния книги при выдаче читателю       | <center>**FK**</center> | not null, Связь по полю States.Id (сервис Каталог книг)             |
|                         | **StateIdIn**        | int              | ИД статуса состояния книги при возврате от  читателя | <center>**FK**</center> | Связь по полю States.Id (сервис Каталог книг)                       |

##### FactComments - комментарии для фактов возврата книги

| Ключ                    | Наименование       | тип           | Описание                        | внеш. ключ              | Доп инфо                                                       |
| ----------------------- | ------------------ | ------------- | ------------------------------- | ----------------------- | -------------------------------------------------------------- |
| <center>**PK**</center> | **Id**             | int           | Ид записи                       |                         | not null, autoincrement                                        |
|                         | **FactId**         | int           | ИД факта                        | <center>**FK**</center> | not null, Связь по полю Facts.Id                               |
|                         | **BookInstanceId** | int           | ИД экземпляра книги             | <center>**FK**</center> | not null, Связь по полю BookInstances.Id (сервис Каталог книг) |
|                         | **Comment**        | nvarchar(300) | Комментарий                     |                         | not null                                                       |
