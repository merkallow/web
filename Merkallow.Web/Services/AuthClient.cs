using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace Merkallow.Web.Services
{

    public interface IAuthenticate
    {
        Task<User[]> GetUser(string id);
    }
    public class AuthClient : IAuthenticate
    {
        string _apiUrl;
        AppState _appState;
        IToastService _toast;
        HttpClient _http;

        public AuthClient(ClientConfig config, AppState state, IToastService toast)
        {
            _apiUrl = config.ApiUrl;
            _appState = state;
            _toast = toast;
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_appState.BearerToken}");
        }

        public async Task<User[]> GetUser(string address)
        {
            //var uri = _apiUrl + $"/auth?publicAddress={address}";

            var builder = new UriBuilder(_apiUrl + "/users");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["publicAddress"] = address;
            builder.Query = query.ToString();
            string uri = builder.ToString();


            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<User[]>(uri);
            Console.WriteLine($"got: {data.First().Id} {data.First().Nonce} {data.First().Username}");
            return data;
        }
    }
}