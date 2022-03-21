﻿using Merkallow.Web.ViewModels;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace Merkallow.Web.Services
{
    public class AppState
    {
        public string AccountId { get; private set; }
        public long ChainId { get; private set; }

        public string BearerToken { get; private set; } // = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYXlsb2FkIjp7ImlkIjozLCJwdWJsaWNBZGRyZXNzIjoiMHg0N2I0MDE2MGY3MmM0MzIxZTA4ZGU4Yjk1ZTI2MmU5MDJjOTkxY2QzIn0sImlhdCI6MTY0NzEzNjE1Nn0.sjxSBNzDCuSCPH1vYGOBwVfaDCkB0E-inbWH6VBN8Rw";
        public bool IsAuthenticated => !string.IsNullOrEmpty(AccountId) && !string.IsNullOrEmpty(BearerToken);

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

        public void SetAccount(ComponentBase source, string accountId)
        {
            this.AccountId = accountId.ToLower();
            NotifyStateChanged(source, "AccountId");
        }

        public void SetChain(ComponentBase source, long chainId)
        {
            this.ChainId = chainId;
            NotifyStateChanged(source, "ChainId");
        }

        public void SetToken(ComponentBase source, string accessToken)
        {
            this.BearerToken = accessToken;
            NotifyStateChanged(source, "BearerToken");
        }

        //public async Task Login(ComponentBase source, )
        //{
        //    CurrentUser = new AuthenticatedUser()
        //    {
        //        BearerToken = token,
        //        PublicAddress = address,
        //        Nonce =
        //    };
        //    NotifyStateChanged(source, "CurrentUser");
        //    await _js.InvokeVoidAsync("setCookie", "token", tokenResult.AccessToken);
        //}

        public async Task Logout(ComponentBase source)
        {
            this.BearerToken = null;
            this.AccountId = null;
            NotifyStateChanged(source, "BearerToken");
            NotifyStateChanged(source, "AccountId");
            Console.WriteLine("AppState.Logout");
            await _js.InvokeVoidAsync("eraseCookie", "token");
        }


        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
