namespace TaskerApi
{
    public class ServiceResult<T>
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public bool IsSuccess { get; set; }
        public T Result { get; set; }

        public ServiceResult<T> Success(T Data) => new ServiceResult<T> { IsSuccess = true, Result = Data };
        public ServiceResult<T> Fail(string title, string message) => new() { IsSuccess = false, Title = title, Message = message };
    }
}