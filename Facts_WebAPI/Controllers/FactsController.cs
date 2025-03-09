using AutoMapper;
using Facts_Business.Repository.IRepository;
using Facts_Domain.FactsDB;
using Facts_Models.FactsModels.State;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Facts_WebAPI.Controllers
{
    public class FactsController : ControllerBase
    {

        private readonly IFactRepository _factRepository;
        private readonly IMapper _mapper;
        public FactsController(IFactRepository factRepository, IMapper mapper)
        {
            _factRepository = factRepository;
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

    }
}
