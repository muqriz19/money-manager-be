using System.ComponentModel.DataAnnotations;

namespace moneyManagerBE.Models
{
    public class Account
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        [Required]
        public DateTimeOffset? CreatedDate { get; set; } = new DateTimeOffset();

        public int UserId { get; set; }

        public List<Record> Records { get; set; } = [];
    }
}