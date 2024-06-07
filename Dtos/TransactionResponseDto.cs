namespace moneyManagerBE.Models
{
    public class TransactionResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();
        public TransactionType TransactionType { get; set; }
        public int LogId { get; set; }
        public decimal Value { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public Category Category { get; set; }
        public int UserId { get; set; }
    }

}