using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;

namespace Merkallow.Web.Services
{
    public interface IDoProjects
    {
        Task<Project> Get(int id);
        Task<List<Project>> Get();
        Task<Project> Create(string name);
        Task<ProjectRoot> Generate(int projectId);
    }

    public class ProjectsClient : IDoProjects
    {
        string _apiUrl;
        AppState _appState;
        IToastService _toast;
        HttpClient _http;

        public ProjectsClient(ClientConfig config, AppState state, IToastService toast)
        {
            _apiUrl = config.ApiUrl;
            _appState = state;
            _toast = toast;
            _http = new HttpClient();
            _http.DefaultRequestHeaders.Add("Authorization", $"Bearer {_appState.BearerToken}");
        }

        public async Task<Project> Get(int id)
        {
            var uri = _apiUrl + $"/projects/{id}";
            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<Project>(uri);
            Console.WriteLine($"got: {data.Name}");
            return data;
        }

        public async Task<List<Project>> Get()
        {
            var uri = _apiUrl + $"/projects";
            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<Project[]>(uri);
            Console.WriteLine($"got: {data.Count()} projects");
            return data.OrderByDescending(o => o.Id).ToList();
        }

        public async Task<Project> Create(string name)
        {
            var uri = _apiUrl + $"/projects";
            var request = new ProjectCreateRequest() { Name = name };
            var result = await _http.PostAsJsonAsync<ProjectCreateRequest>(uri, request);

            var content = await result.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<Project>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            Console.WriteLine(data.Name);
            return data;
        }

        public async Task<ProjectRoot> Generate(int projectId)
        {
            var uri = _apiUrl + $"/projects/generate/{projectId}";
            var request = new GenerateTreeRequest() { ProjectId = projectId };
            var result = await _http.PostAsJsonAsync<GenerateTreeRequest>(uri, request);

            var content = await result.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<ProjectRoot>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));
            Console.WriteLine(data.Root);
            return data;
        }
    }
}
