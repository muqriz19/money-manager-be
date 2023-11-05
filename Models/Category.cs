namespace moneyManagerBE.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; } = string.Empty;
        public string Icon { get; set; } = string.Empty;
    }

    public class Icon
    {
        public string Name { get; set; } = string.Empty;
        public string IconSet { get; set; } = string.Empty;
    }
}