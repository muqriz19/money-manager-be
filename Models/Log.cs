using System.ComponentModel.DataAnnotations.Schema;

namespace moneyManagerBE.Models
{
    public class Log
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();
        public int RecordId { get; set; }
        public decimal Value { get; set; } = 0;
        public string? Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public int UserId { get; set; }
        public List<Transaction> Transactions { get; set; } = [];
    }
}