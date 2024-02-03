namespace moneyManagerBE.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string IconName { get; set; } = string.Empty;
        public string IconSet { get; set; } = string.Empty;
        public DateTimeOffset CreatedDate { get; set; } = new DateTimeOffset();

        public int UserId { get; set; }
    }

    public class Icon
    {
        public string Name { get; set; } = string.Empty;
        public string Set { get; set; } = string.Empty;
    }
}