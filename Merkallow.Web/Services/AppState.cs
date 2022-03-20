using Microsoft.AspNetCore.Components;

namespace Merkallow.Web.Services
{
    public class AppState
    {
        public string AccountId { get; private set; }
        public long ChainId { get; private set; }

        public string BearerToken { get; private set; } // = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJwYXlsb2FkIjp7ImlkIjozLCJwdWJsaWNBZGRyZXNzIjoiMHg0N2I0MDE2MGY3MmM0MzIxZTA4ZGU4Yjk1ZTI2MmU5MDJjOTkxY2QzIn0sImlhdCI6MTY0NzEzNjE1Nn0.sjxSBNzDCuSCPH1vYGOBwVfaDCkB0E-inbWH6VBN8Rw";
        public bool IsAuthenticated => !string.IsNullOrEmpty(AccountId) && !string.IsNullOrEmpty(BearerToken);
        public string AccountToShow()
        {
            if ((string.IsNullOrEmpty(AccountId)))
                return string.Empty;
            else
                return $"{AccountId.Substring(0, 6)}...{AccountId.Substring(AccountId.Length - 4)}";
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


        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
