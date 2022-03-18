using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace Merkallow.Web.Services
{
    public interface IDoAddresses
    {
        Task<List<Address>> Get(int id);
        Task<List<Address>> Add(string address, int projectId);
        Task<bool> Delete(int addressId);
        Task<bool> Update(string address, int addressId);
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

        public async Task<List<Address>> Add(string address, int projectId)
        {
            var uri = _apiUrl + $"/addresses/{projectId}";
            Console.WriteLine($"callin: {uri}");
            var request = new AddAddressRequest() { Addresses = new string[] { address.ToLower() } };

            var result = await _http.PostAsJsonAsync<AddAddressRequest>(uri, request);

            var content = await result.Content.ReadAsStringAsync();
            var resultAddress = JsonSerializer.Deserialize<List<Address>>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            return resultAddress;
        }

        public async Task<bool> Delete(int addressId)
        {
            var uri = _apiUrl + $"/addresses/{addressId}";
            Console.WriteLine($"callin: {uri}");

            var result = await _http.DeleteAsync(uri);

            if (result.IsSuccessStatusCode)
                return true;
            else return false;
        }

        public async Task<bool> Update(string address, int addressId)
        {
            var uri = _apiUrl + $"/addresses/{addressId}";
            Console.WriteLine($"callin: {uri}");

            var request = new UpdateAddressRequest() { Address = address.ToLower() };
            var result = await _http.PutAsJsonAsync<UpdateAddressRequest>(uri, request);

            if (result.IsSuccessStatusCode)
                return true;
            else return false;
        }

    }
}
