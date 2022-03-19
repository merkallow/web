namespace Merkallow.Web.ViewModels
{
    public class User
    {
        public int Id { get; set; }
        public int Nonce { get; set; }
        public string PublicAddress { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
    }
}