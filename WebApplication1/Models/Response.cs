public class ResponseObject<T>
{
    public string Message { get; set; }
    public T? Data { get; set; } 

    public ResponseObject(string message, T? data)
    {
        Message = message;
        Data = data;
    }
}
