

namespace Market.Models
{
    public enum Roles
    {
        Admin,
        StoreWorker,
        Worker
    }

    public class User
    {
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
