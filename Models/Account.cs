using System.ComponentModel.DataAnnotations;

namespace moneyManagerBE.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();
    }
}