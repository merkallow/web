using Merkallow.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Merkallow.Web.Services
{
    public class AppState
    {
        public long ChainId { get; private set; }
        public string BearerToken => CurrentUser?.BearerToken ?? string.Empty;
        public string AccountId => CurrentUser?.User?.PublicAddress ?? string.Empty;

        // = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYXlsb2FkIjp7ImlkIjozLCJwdWJsaWNBZGRyZXNzIjoiMHg0N2I0MDE2MGY3MmM0MzIxZTA4ZGU4Yjk1ZTI2MmU5MDJjOTkxY2QzIn0sImlhdCI6MTY0NzEzNjE1Nn0.sjxSBNzDCuSCPH1vYGOBwVfaDCkB0E-inbWH6VBN8Rw";
        public bool IsAuthenticated => CurrentUser != null && CurrentUser.User != null && !string.IsNullOrWhiteSpace(CurrentUser.BearerToken);
        
        public AuthenticatedUser CurrentUser { get; set; }
        public string AccountToShow()
        {
            if ((string.IsNullOrEmpty(AccountId)))
                return string.Empty;
            else
                return $"{AccountId.Substring(0, 6)}...{AccountId.Substring(AccountId.Length - 4)}";
        }

        private IJSRuntime _js;

        public AppState(IJSRuntime JS)
        {
            _js = JS;
        }

        public void SetChain(ComponentBase source, long chainId)
        {
            this.ChainId = chainId;
            NotifyStateChanged(source, "ChainId");
        }

        public async Task Login(ComponentBase source, User user, string token)
        {
            CurrentUser = new AuthenticatedUser()
            {
                User = user,
                BearerToken = token
            };
            
            NotifyStateChanged(source, "CurrentUser");
            await _js.InvokeVoidAsync("setCookie", "token", token);
            Console.WriteLine($"AppState.Login {user.PublicAddress}");
        }

        public async Task Logout(ComponentBase source)
        {
            CurrentUser = null;
            await _js.InvokeVoidAsync("eraseCookie", "token");

            NotifyStateChanged(source, "CurrentUser");
            NotifyStateChanged(source, "BearerToken");
            Console.WriteLine("AppState.Logout");
        }


        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
