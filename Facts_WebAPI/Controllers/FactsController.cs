using AutoMapper;
using Facts_Business.Repository.IRepository;
using Facts_Common;
using Facts_Domain.FactsDB;
using Facts_Models.CatalogModels;
using Facts_Models.FactsModels.Fact;
using Facts_Models.FactsModels.State;
using Facts_WebAPI.Controllers.Services.IServices;
using Facts_WebAPI.Service.ServiceModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;

namespace Facts_WebAPI.Controllers
{
    public class FactsController : ControllerBase
    {

        private readonly ICatalogService _catalogService;
        private readonly IFactRepository _factRepository;
        private readonly IBookInstanceRepository _bookInstanceRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IFactCommentRepository _factCommentRepository;
        private readonly IMapper _mapper;
        public FactsController(
            IFactRepository factRepository,
            IBookInstanceRepository bookInstanceRepository,
            IStateRepository stateRepository,
            ICatalogService catalogService,
            IFactCommentRepository factCommentRepository,
            IMapper mapper)
        {
            _factRepository = factRepository;
            _bookInstanceRepository = bookInstanceRepository;
            _stateRepository = stateRepository;
            _catalogService = catalogService;
            _factCommentRepository = factCommentRepository;
            _mapper = mapper;
        }


        /// <summary>
        /// Получить список фактов выдачи/возвратов экземпляров книг
        /// </summary>
        /// <returns>Возвращает список всех статусов - объектов типа StateItemResponse</returns>
        /// <response code="200">Успешное выполнение</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<StateItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<StateItemResponse>>> GetAllStatesAsync()
        {
            var gotFacts = await _factRepository.GetAllFactsAsync();
            return Ok(_mapper.Map<IEnumerable<Fact>, IEnumerable<FactItemResponse>>(gotFacts));
        }


        /// <summary>
        /// Получить факт по ИД
        /// </summary>
        /// <param name="id">ИД факта</param>
        /// <returns>Возвращает найденый факт по ИД - объект типа FactItemResponse</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Факт с заданным ИД не найден</response>
        [HttpGet("{id:int}")]
        [ProducesResponseType(typeof(FactItemResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<FactItemResponse>> GetFactByIdAsync(int id)
        {
            var fact = await _factRepository.GetByIdAsync(id);

            if (fact == null)
                return NotFound("Факт с ИД = " + id.ToString() + " не найден!");

            return Ok(_mapper.Map<Fact, FactItemResponse>(fact));
        }

        /// <summary>
        /// Получить текущее состояние экземпляра книги
        /// </summary>
        /// <param name="bookInstanceId">ИД факта</param>
        /// <returns>Возвращает текущий статус экземпляра книги - объект типа StateItemResponse</returns>
        /// <response code="200">Успешное выполнение</response>        
        [HttpGet("GetCurrentStateByBookInstanceId/{bookInstanceId:int}")]
        [ProducesResponseType(typeof(StateItemResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<StateItemResponse>> GetCurrentStateByBookInstanceIdAsync(int bookInstanceId)
        {
            var state = await _factRepository.GetCurrentBookInstanceStateAsync(bookInstanceId);

            return Ok(_mapper.Map<State, StateItemResponse>(state));
        }


        /// <summary>
        /// Добавить факт выдачи экземпляра книги читателю
        /// </summary>
        /// <param name="bookInstanceId">ИД экземпляра книги</param>
        /// <param name="memberId">ИД читателя</param>
        /// <returns>Объект с информации о созданном факте выдачи читателю экземпляра книги</returns>
        /// <response code="201">Создан факт выдачи экземпляра книги читателю</response>
        /// <response code="400">Не удалось создать факт выдачи экземпляра книги читателю. Подробнее о причинах - в ответе от сервера </response>
        [HttpPost("{bookInstanceId:int}/{memberId:int}")]
        [ProducesResponseType(typeof(FactCheckOutItemResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<FactCheckOutItemResponse>> AddCheckOutFactAsync(int bookInstanceId, int memberId)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(bookInstanceId);

            if (foundBookInstance != null)
            {
                if (foundBookInstance.IsCheckedOut)
                    return BadRequest("Экземпляр книги с ИД = " + bookInstanceId.ToString() + " уже выдан читателю");
                if (foundBookInstance.IsWrittenOff)
                    return BadRequest("Экземпляр книги с ИД = " + bookInstanceId.ToString() + " уже списан");
                if (foundBookInstance.IsReservedMemberId > 0)
                    if (memberId != foundBookInstance.IsReservedMemberId)
                        return BadRequest("Экземпляр книги с ИД = " + bookInstanceId.ToString() + " уже зарезервирован читателем с ИД " + memberId.ToString());
            }

            var bookInstanceOnlyForReadingRoomResponse = new BookInstanceOnlyForReadingRoomResponse
            { OnlyForReadingRoom = false };

            var response = await _catalogService.GetOnlyForReadingRoomByBookInstanceIdAsync<ResponseDTO>(bookInstanceId);

            if (response != null && response.IsSuccess)
            {
                BookInstanceOnlyForReadingRoomResponse bookInstanceOnlyForReadingRoomResponseFromCatalog = JsonConvert.DeserializeObject<BookInstanceOnlyForReadingRoomResponse>(Convert.ToString(response.Result));
                bookInstanceOnlyForReadingRoomResponse = bookInstanceOnlyForReadingRoomResponseFromCatalog;
            }
            else
                return BadRequest("Для экземпляра книги с ИД = " + bookInstanceId.ToString() + " не удалось получить OnlyForReadingRoom от сервиса CatalogAPI");

            var bookInstanceOutMaxDaysResponse = new BookInstanceOutMaxDaysResponse
            { OutMaxDays = 14 };

            response = await _catalogService.GetOutMaxDaysByBookInstanceIdAsync<ResponseDTO>(bookInstanceId);

            if (response != null && response.IsSuccess)
            {
                BookInstanceOutMaxDaysResponse bookInstanceOutMaxDaysResponseFromCatalog = JsonConvert.DeserializeObject<BookInstanceOutMaxDaysResponse>(Convert.ToString(response.Result));
                bookInstanceOutMaxDaysResponse = bookInstanceOutMaxDaysResponseFromCatalog;
            }
            else
                return BadRequest("Для экземпляра книги с ИД = " + bookInstanceId.ToString() + " не удалось получить OutMaxDays от сервиса CatalogAPI");


            if ((bookInstanceOnlyForReadingRoomResponse == null) || (bookInstanceOutMaxDaysResponse == null))
                return BadRequest("Не удалось получить данные по экземпляру книги с ИД = " + bookInstanceId.ToString() + " у сервиса Catalog");

            if (bookInstanceOnlyForReadingRoomResponse.OnlyForReadingRoom == true)
                return BadRequest("Экземпляр книги с ИД = " + bookInstanceId.ToString() + " выдаётся только в читальный зал");

            var currentState = await _factRepository.GetCurrentBookInstanceStateAsync(bookInstanceId);

            var newFact = new Fact
            {
                BookInstanceId = bookInstanceId,
                FromDate = DateTime.Now,
                PlanDateOfReturn = DateTime.Now.AddDays(bookInstanceOutMaxDaysResponse.OutMaxDays),
                MemberId = memberId,
                GiveUserId = SD.UserIdForInitialData,
                StateIdOut = currentState.Id
            };

            var addedFact = await _factRepository.AddAsync(newFact);

            if (foundBookInstance == null)
            {
                var newBookInstance = new BookInstance
                {
                    Id = bookInstanceId,
                    IsCheckedOut = true,
                    IsReservedMemberId = 0,
                    IsWrittenOff = false
                };
                await _bookInstanceRepository.AddAsync(newBookInstance);
            }
            else
            {
                foundBookInstance.IsCheckedOut = true;
                await _bookInstanceRepository.UpdateAsync(foundBookInstance);
            }

            var routVar = "";
            if (Request != null)
            {
                routVar = new UriBuilder(Request.Scheme, Request.Host.Host, (int)Request.Host.Port, Request.Path.Value).ToString()
                    + "/" + addedFact.Id.ToString();
            }
            return Created(routVar, _mapper.Map<Fact, FactCheckOutItemResponse>(addedFact));
        }


        /// <summary>
        /// Добавить факт факт возврата экземпляра книги читателем
        /// </summary>
        /// <param name="bookInstanceId">ИД экземпляра книги</param>
        /// <param name="memberId">ИД читателя</param>
        /// <param name="factReturnAddItemRequest">Информация о возврате экземпляра книги читателем</param>
        /// <returns>Объект с информацией возврате экземпляра книги</returns>
        /// <response code="200">Записан факт возврата книги читателем</response>
        /// <response code="400">Не удалось записать факт возврата экземпляра книги читателем. Подробнее о причинах - в ответе от сервера </response>
        /// <response code="404">Не найден факт выдачи экземпляра книги читателю или статус состояния при возврате</response>
        [HttpPut("{bookInstanceId:int}/{memberId:int}")]
        [ProducesResponseType(typeof(FactItemResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<FactItemResponse>> AddBookInstanceReturnFactAsync(int bookInstanceId, int memberId, FactReturnAddItemRequest request)
        {
            var foundFact = await _factRepository.GetCheckOutInfoByBookInstanceIsAndMemberIdAsync(bookInstanceId, memberId);

            if (foundFact == null)
                return NotFound("Для экземпляра книги с ИД = " + bookInstanceId.ToString() + " не удалось найти факт выдачи читателю с ИД = " + memberId.ToString());

            if (foundFact.DateOfReturn != null || foundFact.ReturnUserId != null || foundFact.StateIdIn != null)
                return BadRequest("В факте выдачи экземпляра книги читателю  (ИД факта = " + foundFact.Id.ToString() + ") уже есть информация о возврате экземпляра книги");

            var foundStateIn = await _stateRepository.GetByIdAsync(request.StateIdIn);

            if (foundStateIn == null)
                return NotFound("Не найден статус с ИД = " + request.StateIdIn.ToString() + " в справочнике статусов состояния экземпляров книг");

            bool needAddComment = false;
            if (String.IsNullOrWhiteSpace(request.Comment))
            {
                // Если статус состояния экземпляра книги поменялся при возврате в стравнении со статусом при выдаче
                if (foundFact.StateIdOut != request.StateIdIn)
                {
                    if (foundStateIn.IsNeedComment == true)
                    {
                        return BadRequest("Не найден комментарий. Переход экземпляра книги в статус " + foundStateIn.Name + " должен сопровождаться обязательным комметарием");
                    }
                }
            }
            else
            {
                needAddComment = true;
            }

            foundFact.DateOfReturn = DateTime.Now;
            foundFact.ReturnUserId = SD.UserIdForInitialData;
            foundFact.StateIdIn = request.StateIdIn;

            FactComment? factComment = null;

            if (needAddComment)
            {
                factComment = new FactComment();
                factComment.FactId = foundFact.Id;
                factComment.BookInstanceId = foundFact.BookInstanceId;
                factComment.Comment = request.Comment;
            }

            var bookInstanse = await _bookInstanceRepository.GetByIdAsync(foundFact.BookInstanceId);

            bool needAddbookInstanseRecord = false;
            if (bookInstanse == null)
            {
                bookInstanse = new BookInstance();
                needAddbookInstanseRecord = true;
            }

            bookInstanse.IsCheckedOut = false;

            await _factRepository.UpdateAsync(foundFact);

            if (needAddbookInstanseRecord)
                await _bookInstanceRepository.AddAsync(bookInstanse);
            else
                await _bookInstanceRepository.UpdateAsync(bookInstanse);

            if (needAddComment)
                await _factCommentRepository.AddAsync(factComment);

            var updatedFact = await _factRepository.GetByIdAsync(foundFact.Id);

            return Ok(_mapper.Map<Fact, FactItemResponse>(updatedFact));
        }
    }
}
