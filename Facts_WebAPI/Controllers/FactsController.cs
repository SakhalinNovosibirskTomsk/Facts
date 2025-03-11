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
        private readonly IMapper _mapper;
        public FactsController(
            IFactRepository factRepository,
            IBookInstanceRepository bookInstanceRepository,
            IStateRepository stateRepository,
            ICatalogService catalogService,
            IMapper mapper)
        {
            _factRepository = factRepository;
            _bookInstanceRepository = bookInstanceRepository;
            _stateRepository = stateRepository;
            _catalogService = catalogService;
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


        [HttpPost("{bookInstanceId:int}/{memberId:int}")]
        [ProducesResponseType(typeof(FactCheckOutItemResponse), (int)HttpStatusCode.Created)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<List<FactCheckOutItemResponse>>> AddCheckOutFactAsync(int bookInstanceId, int memberId)
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

            // TODO Запросить у Catalog BookInstanceOnlyForReadingRoomResponse
            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var response = await _catalogService.GetOutMaxDaysByBookInstanceIdAsync<ResponseDTO>(bookInstanceId, "");

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

            // TODO Запросить у Catalog BookInstanceOutMaxDaysResponse
            var bookInstanceOutMaxDaysResponse = new BookInstanceOutMaxDaysResponse
            { OutMaxDays = 14 };

            //var accessToken = await HttpContext.GetTokenAsync("access_token");
            //var response = await _catalogService.GetOutMaxDaysByBookInstanceIdAsync<ResponseDTO>(bookInstanceId, "");
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

    }
}
