namespace moneyManagerBE.Models
{
    public class Transaction
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();
        public TransactionType TransactionType { get; set; }
        public int LogId { get; set; }
        public decimal Value { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int UserId { get; set; }
    }

    // public class Transaction
    // {
    //     public int Id { get; set; }
    //     public string Name { get; set; } = string.Empty;
    //     public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();
    //     public TransactionType TransactionType { get; set; }
    //     public int LogId { get; set; }
    //     public decimal Value { get; set; } = 0;
    //     public string? Description { get; set; } = string.Empty;
    //     public int CategoryId { get; set; }
    //     public int UserId { get; set; }
    // }

    public enum TransactionType
    {
        Income,
        Expenses
    }
}