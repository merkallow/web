using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace Merkallow.Web.Services
{
    public interface IDoAddresses
    {
        Task<List<Address>> Get(int id);
        Task<Address> Add(string address, int projectId);
    }

    public class AddressClient : IDoAddresses
    {
        string _apiUrl;
        AppState _appState;
        IToastService _toast;
        HttpClient _http;

        public AddressClient(ClientConfig config, AppState state, IToastService toast)
        {
            _apiUrl = config.ApiUrl;
            _appState = state;
            _toast = toast;
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_appState.BearerToken}");
        }

        public async Task<List<Address>> Get(int id)
        {
            var uri = _apiUrl + $"/addresses/{id}";
            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<Address[]>(uri);
            Console.WriteLine($"got: {data.Count()} addresses");
            return data.ToList();
        }

        public async Task<Address> Add(string address, int projectId)
        {
            var uri = _apiUrl + $"/addresses/{projectId}";
            Console.WriteLine($"callin: {uri}");
            var request = new AddAddressRequest() { Addresses = new string[] { address } };

            var result = await _http.PostAsJsonAsync<AddAddressRequest>(uri, request);

            var content = await result.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<int>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            var resultAddress = new Address()
            {
                Id = 0,
                ProjectId = projectId,
                PublicAddress = address
            };
            return resultAddress;
        }

    }
}
