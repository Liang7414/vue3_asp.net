namespace vote_system.Models
{
    public class LoginModel
    {
        public string IdNumber { get; set; }
        public string Password { get; set; }
    }

    public class VoteModel
    {
        public string IdNumber { get; set; }
        public int VoteChoice { get; set; }
    }
}
