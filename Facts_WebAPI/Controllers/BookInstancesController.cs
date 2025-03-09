using AutoMapper;
using Facts_Business.Repository.IRepository;
using Facts_Domain.FactsDB;
using Facts_Models.FactsModels.BookInstance;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Facts_WebAPI.Controllers
{

    /// <summary>
    /// Информация о выдаче, бронировании и списании экземпляров книг
    /// </summary>
    [ApiController]
    [Route("api/v1/[controller]")]
    public class BookInstancesController : ControllerBase
    {
        private readonly IBookInstanceRepository _bookInstanceRepository;
        private readonly IMapper _mapper;
        public BookInstancesController(IBookInstanceRepository bookInstanceRepository, IMapper mapper)
        {
            _bookInstanceRepository = bookInstanceRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получить все записи с информация о выдаче, бронировании и списании экземпляров книг, имеющиеся в сервисе
        /// </summary>
        /// <returns>Возвращает все записи с информацией о выдаче, бронировании и списании экземпляров книг, имеющиеся в сервисе - объектов типа BookInstanceItemResponse</returns>
        /// <response code="200">Успешное выполнение</response>
        [HttpGet("All")]
        [ProducesResponseType(typeof(List<BookInstanceItemResponse>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<BookInstanceItemResponse>>> GetAllBookInstancesAsync()
        {
            var gotBookInstances = await _bookInstanceRepository.GetAllAsync();
            return Ok(_mapper.Map<IEnumerable<BookInstance>, IEnumerable<BookInstanceItemResponse>>(gotBookInstances));
        }


        /// <summary>
        /// Получить информацию о выдаче, бронировании и списании экземпляра книги, имеющуюся в сервисе, по ИД экземпляра книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает найденый объект типа BookInstanceItemResponse</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="404">Запись с заданным ИД не найдена</response>
        [HttpGet("GetBookInstanceInfoById/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceItemResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.NotFound)]
        public async Task<ActionResult<BookInstanceItemResponse>> GetBookInstanceInfoByIdAsync(int id)
        {
            var bookInstance = await _bookInstanceRepository.GetByIdAsync(id);

            if (bookInstance == null)
                return NotFound("Запись с ID = " + id.ToString() + " не найдена!");

            return Ok(_mapper.Map<BookInstance, BookInstanceItemResponse>(bookInstance));
        }


        /// <summary>
        /// Получить состояние флага о выдаче экземпляра книги читателю
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает найденый объект типа BookInstanceIsCheckedOutResponse</returns>
        /// <response code="200">Успешное выполнение</response>        
        [HttpGet("GetIsCheckedOutByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsCheckedOutResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BookInstanceIsCheckedOutResponse>> GetIsCheckedOutByBookInstanceIdAsync(int id)
        {
            var bookInstance = await _bookInstanceRepository.GetByIdAsync(id);

            var bookInstanceIsCheckedOutResponse = new BookInstanceIsCheckedOutResponse
            {
                IsCheckedOut = (bookInstance == null ? false : bookInstance.IsCheckedOut)

            };

            return Ok(bookInstanceIsCheckedOutResponse);
        }

        /// <summary>
        /// Установить флаг о выдаче экземпляра книги читателю
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>        
        /// <returns>Возвращает состояние флага выдаче экземпляра книги читателю после изменения - объект типа BookInstanceIsCheckedOutResponse</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="400">Не удалось произвести изменение. Причина описана в ответе</response>          
        [HttpPut("SetIsCheckedOutByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsCheckedOutResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BookInstanceIsCheckedOutResponse>> SetIsCheckedOutByBookInstanceIdAsync(int id)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(id);
            if (foundBookInstance != null)
                if (foundBookInstance.IsCheckedOut)
                    return BadRequest("У экземпляра книги с ИД = " + id.ToString() + " уже установлена отметка о выдаче читателю");

            if (foundBookInstance != null)
            {
                foundBookInstance.IsCheckedOut = true;
                var updatedBookInstance = await _bookInstanceRepository.UpdateAsync(foundBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsCheckedOutResponse>(updatedBookInstance));
            }
            else
            {
                var newBookInstance = new BookInstance
                {
                    Id = id,
                    IsCheckedOut = true,
                    IsReservedMemberId = 0,
                    IsWrittenOff = false
                };
                var addedBookInstance = await _bookInstanceRepository.AddAsync(newBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsCheckedOutResponse>(addedBookInstance));
            }
        }

        /// <summary>
        /// Снять флаг о выдаче экземпляра книги читателю
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>        
        /// <returns>Возвращает состояние флага выдаче экземпляра книги читателю после изменения - объект типа BookInstanceIsCheckedOutResponse</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="400">Не удалось выполнить изменение. Причина описана в ответе</response>          
        [HttpPut("UnSetIsCheckedOutByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsCheckedOutResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BookInstanceIsCheckedOutResponse>> UnSetIsCheckedOutByBookInstanceIdAsync(int id)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(id);
            if (foundBookInstance != null)
                if (!foundBookInstance.IsCheckedOut)
                    return BadRequest("У экземпляра книги с ИД = " + id.ToString() + " уже снята отметка о выдаче читателю");

            if (foundBookInstance != null)
            {
                foundBookInstance.IsCheckedOut = false;
                var updatedBookInstance = await _bookInstanceRepository.UpdateAsync(foundBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsCheckedOutResponse>(updatedBookInstance));
            }
            else
            {
                var newBookInstance = new BookInstance
                {
                    Id = id,
                    IsCheckedOut = false,
                    IsReservedMemberId = 0,
                    IsWrittenOff = false
                };
                var addedBookInstance = await _bookInstanceRepository.AddAsync(newBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsCheckedOutResponse>(addedBookInstance));
            }
        }

        //----

        /// <summary>
        /// Получить состояние флага о списании экземпляра книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает найденый объект типа BookInstanceIsWrittenOffResponse</returns>
        /// <response code="200">Успешное выполнение</response>        
        [HttpGet("GetIsWrittenOffByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsWrittenOffResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BookInstanceIsWrittenOffResponse>> GetIsWrittenOffByBookInstanceIdAsync(int id)
        {
            var bookInstance = await _bookInstanceRepository.GetByIdAsync(id);

            var bookInstanceIsWrittenOffResponse = new BookInstanceIsWrittenOffResponse
            {
                IsWrittenOff = (bookInstance == null ? false : bookInstance.IsWrittenOff)
            };

            return Ok(bookInstanceIsWrittenOffResponse);
        }

        /// <summary>
        /// Установить флаг о списании экземпляра книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>        
        /// <returns>Возвращает состояние флага о списании экземпляра книги после изменения - объект типа BookInstanceIsWrittenOffResponse</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="400">Не удалось произвести изменение. Причина описана в ответе</response>          
        [HttpPut("SetIsWrittenOffByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsWrittenOffResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BookInstanceIsWrittenOffResponse>> SetIsWrittenOffByBookInstanceIdAsync(int id)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(id);
            if (foundBookInstance != null)
                if (foundBookInstance.IsWrittenOff)
                    return BadRequest("У экземпляра книги с ИД = " + id.ToString() + " уже установлена отметка о списании");

            if (foundBookInstance != null)
            {
                foundBookInstance.IsWrittenOff = true;
                var updatedBookInstance = await _bookInstanceRepository.UpdateAsync(foundBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsWrittenOffResponse>(updatedBookInstance));
            }
            else
            {
                var newBookInstance = new BookInstance
                {
                    Id = id,
                    IsCheckedOut = false,
                    IsReservedMemberId = 0,
                    IsWrittenOff = true
                };
                var addedBookInstance = await _bookInstanceRepository.AddAsync(newBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsWrittenOffResponse>(addedBookInstance));
            }
        }

        /// <summary>
        /// Снять флаг о списании экземпляра книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>        
        /// <returns>Возвращает состояние флага списания экземпляра книги после изменения - объект типа BookInstanceIsWrittenOffResponse</returns>
        /// <response code="200">Успешное выполнение</response>
        /// <response code="400">Не удалось произвести изменения. Причина описана в ответе</response>          
        [HttpPut("UnSetIsWrittenOffByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsWrittenOffResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BookInstanceIsWrittenOffResponse>> UnSetIsWrittenOffByBookInstanceIdAsync(int id)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(id);
            if (foundBookInstance != null)
                if (!foundBookInstance.IsWrittenOff)
                    return BadRequest("У экземпляра книги с ИД = " + id.ToString() + " уже снята отметка о списании");

            if (foundBookInstance != null)
            {
                foundBookInstance.IsWrittenOff = false;
                var updatedBookInstance = await _bookInstanceRepository.UpdateAsync(foundBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsWrittenOffResponse>(updatedBookInstance));
            }
            else
            {
                var newBookInstance = new BookInstance
                {
                    Id = id,
                    IsCheckedOut = false,
                    IsReservedMemberId = 0,
                    IsWrittenOff = false
                };
                var addedBookInstance = await _bookInstanceRepository.AddAsync(newBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsWrittenOffResponse>(addedBookInstance));
            }
        }


        //----

        /// <summary>
        /// Получить ИД чмтателя забронировавшего экземпляр книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>
        /// <returns>Возвращает найденый объект типа BookInstanceIsReservedMemberIdResponse</returns>
        /// <response code="200">Успешное выполнение</response>        
        [HttpGet("GetIsReservedMemberIdByBookInstanceId/{id:int}")]
        [ProducesResponseType(typeof(BookInstanceIsReservedMemberIdResponse), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<BookInstanceIsReservedMemberIdResponse>> GetIsReservedMemberIdByBookInstanceIdAsync(int id)
        {
            var bookInstance = await _bookInstanceRepository.GetByIdAsync(id);

            var bookInstanceIsReservedMemberIdResponse = new BookInstanceIsReservedMemberIdResponse
            {
                IsReservedMemberId = (bookInstance == null ? 0 : bookInstance.IsReservedMemberId)
            };

            return Ok(bookInstanceIsReservedMemberIdResponse);
        }

        /// <summary>
        /// Установить ИД пользователя забронировавшего экземпляр книги
        /// </summary>
        /// <param name="id">ИД экземпляра книги</param>        
        /// <param name="memberId">ИД читателя</param>        
        /// <returns>Возвращает ИД пользователя забронировавшего экземпляр книги после изменения - объект типа BookInstanceIsReservedMemberIdResponse</returns>
        /// <response code="200">Успешное выполнение.</response>
        /// <response code="400">Не удалось произвести изменение. Причина описана в ответе</response>          
        [HttpPut("SetIsReservedMemberIdByBookInstanceId/{id:int}/{memberId:int}")]
        [ProducesResponseType(typeof(BookInstanceIsReservedMemberIdResponse), (int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.BadRequest)]
        public async Task<ActionResult<BookInstanceIsWrittenOffResponse>> SetIsReservedMemberIdByBookInstanceIdAsync(int id, int memberId)
        {
            var foundBookInstance = await _bookInstanceRepository.GetByIdAsync(id);

            if (foundBookInstance != null)
            {
                foundBookInstance.IsReservedMemberId = memberId;
                var updatedBookInstance = await _bookInstanceRepository.UpdateAsync(foundBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsReservedMemberIdResponse>(updatedBookInstance));
            }
            else
            {
                var newBookInstance = new BookInstance
                {
                    Id = id,
                    IsCheckedOut = false,
                    IsReservedMemberId = memberId,
                    IsWrittenOff = true
                };
                var addedBookInstance = await _bookInstanceRepository.AddAsync(newBookInstance);
                return Ok(_mapper.Map<BookInstance, BookInstanceIsReservedMemberIdResponse>(addedBookInstance));
            }
        }
    }
}

