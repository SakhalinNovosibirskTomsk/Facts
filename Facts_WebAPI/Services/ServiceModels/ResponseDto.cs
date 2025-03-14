namespace Facts_WebAPI.Service.ServiceModels
{
    /// <summary>
    /// Класс ответа на http-запрос
    /// </summary>
    public class ResponseDTO
    {
        public bool IsSuccess { get; set; } = true;
        public object Result { get; set; }
        public string DisplayMessage { get; set; } = "";
        public List<string> ErrorMessages { get; set; }
    }
}
