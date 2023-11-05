namespace moneyManagerBE.Class
{
    public class Response<T>
    {
        public T Data { get; set; }
        public int Status { get; set; }
        public string Message { get; set; } = string.Empty;
    }

    public class ResponseList<T> : Response<T>
    {
        public int Total { get; set; }
    }
}