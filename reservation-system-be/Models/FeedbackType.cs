namespace reservation_system_be.Models
{
    public class FeedbackType
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public ICollection<FeedbackType>? FeedbackTypes { get; set; }
    }
}
