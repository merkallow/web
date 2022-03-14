using System.Text.RegularExpressions;

namespace Merkallow.Web.ViewModels
{
    public static class AddressExtensions
    {
        public static bool IsValidAddress(this string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            if(address.Length == 40)
            {
                return new Regex(@"^[a-fA-F0-9]+$").IsMatch(address);
            }
            else if(address.Length == 42 && address.ToLower().StartsWith("0x"))
            {
                var hex = address.Substring(2);
                return new Regex(@"^[a-fA-F0-9]+$").IsMatch(hex);
            }
            else 
                return false;
        }
    }
}
