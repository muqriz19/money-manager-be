namespace moneyManagerBE.Class
{
    public class DbResponse<T>
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; } = string.Empty;

        public T? Data { get; set; }
    }

    public class DbResponseList<T> : DbResponse<T>
    {
        public int Total { get; set; }
    }
}