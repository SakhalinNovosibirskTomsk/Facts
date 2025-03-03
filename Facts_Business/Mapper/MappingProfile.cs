using AutoMapper;
using Facts_Domain.FactsDB;
using Facts_Models.FactsModels.State;

namespace Facts_Business.Mapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<State, StateItemResponse>();
            CreateMap<State, StateItemCreateRequest>();
            CreateMap<State, StateItemUpdateRequest>();

        }
    }
}
