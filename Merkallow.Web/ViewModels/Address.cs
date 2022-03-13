namespace Merkallow.Web.ViewModels
{
    public class Address
    {
        public int Id { get; set; }
        public string PublicAddress { get; set; }
        public int ProjectId { get; set; }
    }

    public class AddAddressRequest
    {
        public string[] Addresses { get; set; }
    }
}
