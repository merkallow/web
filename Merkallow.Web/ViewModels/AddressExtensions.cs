using Nethereum.Util;
using System.Text.RegularExpressions;

namespace Merkallow.Web.ViewModels
{
    public static class AddressExtensions
    {
        public static bool IsValidAddress(this string address)
        {
            Regex r = new Regex("^(0x){1}[0-9a-fA-F]{40}$");
            // Doesn't match length, prefix and hex
            if (!r.IsMatch(address))
                return false;
            // It's all lowercase, so no checksum needed
            else if (address == address.ToLower())
                return true;
            // Do checksum
            else
                return new AddressUtil().IsChecksumAddress(address);

            //if (string.IsNullOrWhiteSpace(address))
            //    return false;

            //if(address.Length == 40)
            //{
            //    return new Regex(@"^[a-fA-F0-9]+$").IsMatch(address);
            //}
            //else if(address.Length == 42 && address.ToLower().StartsWith("0x"))
            //{
            //    var hex = address.Substring(2);
            //    return new Regex(@"^[a-fA-F0-9]+$").IsMatch(hex);
            //}
            //else 
            //    return false;
        }
    }
}
