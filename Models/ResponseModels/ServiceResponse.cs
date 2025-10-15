namespace AuthSystem.Models.ResponseModels
{
    public class ServiceResponse<T> where T : class
    {
        public T? Data { get; set; }
        public bool HasError { get; set; }
        public string? Message { get; set; }
    }
}
