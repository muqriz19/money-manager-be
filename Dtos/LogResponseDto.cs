namespace moneyManagerBE.Models
{
    public class LogResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();
        public int RecordId { get; set; }
        public decimal Value { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public int UserId { get; set; }
    }
}