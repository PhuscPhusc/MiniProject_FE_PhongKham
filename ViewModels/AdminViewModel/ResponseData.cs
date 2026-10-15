public class ResponseData<T> where T : class
{
    public string message { get; set; }
    public T content { get; set; }
}