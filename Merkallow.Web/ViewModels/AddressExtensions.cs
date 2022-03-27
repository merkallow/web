using Nethereum.Util;
using System.Text.RegularExpressions;

namespace Merkallow.Web.ViewModels
{
    public static class AddressExtensions
    {
        public static bool IsValidAddress(this string address)
        {
            if (string.IsNullOrWhiteSpace(address))
                return false;

            Regex r = new Regex("^(0x){1}[0-9a-fA-F]{40}$");
            if (!r.IsMatch(address))
                return false;
            else if (address == address.ToLower())
                return true;
            else
                return new AddressUtil().IsChecksumAddress(address);
        }
    }
}
