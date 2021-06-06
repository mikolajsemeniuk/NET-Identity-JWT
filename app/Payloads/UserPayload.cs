using System.Collections.Generic;

namespace app.Payloads
{
    public class UserPayload
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public ICollection<TodoPayload> Todos { get; set; } = new List<TodoPayload>();
    }
}