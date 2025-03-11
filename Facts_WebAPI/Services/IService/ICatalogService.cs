namespace Facts_WebAPI.Controllers.Services.IServices
{
    public interface ICatalogService : IBaseService
    {
        //Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id, string token);
        Task<T> GetOutMaxDaysByBookInstanceIdAsync<T>(int id);
        //Task<T> GetOnlyForReadingRoomByBookInstanceIdAsync<T>(int id, string token);
        Task<T> GetOnlyForReadingRoomByBookInstanceIdAsync<T>(int id);
    }
}
