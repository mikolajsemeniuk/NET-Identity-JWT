namespace app.Models
{
    public class Todo
    {
        public int TodoId { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
    }
}