using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;

namespace moneyManagerBE.Models
{
    public class Record
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();
        public int AccountId { get; set; }
        public Collection<Log>? Logs { get; set; }
        public int UserId { get; set; }
    }
}