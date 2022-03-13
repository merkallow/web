using Microsoft.AspNetCore.Components;

namespace Merkallow.Web.Services
{
    public class AppState
    {
        public string AccountId { get; private set; } = string.Empty;
        public long ChainId { get; private set; }
        public bool IsAuthenticated => !string.IsNullOrEmpty(AccountId);
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


        public event Action<ComponentBase, string> Statechanged;
        private void NotifyStateChanged(ComponentBase source, string Property) =>
            Statechanged?.Invoke(source, Property);
    }
}
