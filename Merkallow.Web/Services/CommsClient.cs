using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace Merkallow.Web.Services
{
    public interface IDoComms
    {
        Task<Project> GetProject(int id);
    }

    public class CommsClient : IDoComms
    {
        string _apiUrl;
        AppState _appState;
        IToastService _toast;
        HttpClient _http;

        public CommsClient(ClientConfig config, AppState state, IToastService toast)
        {
            _apiUrl = config.ApiUrl;
            _appState = state;
            _toast = toast;
            _http = new HttpClient();
        }

        public async Task<Project> GetProject(int id)
        {
            var uri = _apiUrl + $"/projects/{id}";
            var data = await _http.GetFromJsonAsync<Project>(uri);
            Console.WriteLine(data);
            return data;
        }
    }
}
