namespace moneyManagerBE.Models
{
    public class RecordResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();
        public int AccountId { get; set; }
        public List<LogResponseDto> Logs { get; set; } = [];
        public int UserId { get; set; }
    }
}