﻿using Blazored.Toast.Services;
using Merkallow.Web.ViewModels;
using System.Net.Http.Json;
using System.Text.Json;
using System.Web;

namespace Merkallow.Web.Services
{

    public interface IAuthenticate
    {
        Task<User[]> FindUser(string id);
        // authorized
        Task<User[]> GetUser(string address);
        Task<User> CreateUser(string address);
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

        public async Task<User[]> FindUser(string address)
        {
            var builder = new UriBuilder(_apiUrl + "/users");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["publicAddress"] = address;
            builder.Query = query.ToString();
            string uri = builder.ToString();


            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<User[]>(uri);
            if(data.Length > 0)
            {
                Console.WriteLine("got: ");
                foreach(var user in data)
                {
                    Console.WriteLine($"user: {user.Id} {user.PublicAddress}");
                }
            }
            else { Console.WriteLine("No such user there"); }
            return data;
        }

        public async Task<User[]> GetUser(string address)
        {
            var builder = new UriBuilder(_apiUrl + "/users");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["publicAddress"] = address;
            builder.Query = query.ToString();
            string uri = builder.ToString();


            Console.WriteLine($"callin: {uri}");
            var data = await _http.GetFromJsonAsync<User[]>(uri);

            if (data.Length > 0)
            {
                Console.WriteLine("got: ");
                foreach (var user in data)
                {
                    Console.WriteLine($"user: {user.Id} {user.PublicAddress}");
                }
            }
            else { Console.WriteLine("No such user there"); }
            return data;
        }

        public async Task<User> CreateUser(string address)
        {
            var builder = new UriBuilder(_apiUrl + "/users");
            var query = HttpUtility.ParseQueryString(builder.Query);
            query["publicAddress"] = address;
            builder.Query = query.ToString();
            string uri = builder.ToString();


            var user = new User() { PublicAddress = address, Nonce = new Random().Next(1000, 9999) };
            // POST User

            Console.WriteLine($"callin: {uri}");
            var result = await _http.PostAsJsonAsync<User>(uri, user);
             
            var content = await result.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<User>(content, new JsonSerializerOptions(JsonSerializerDefaults.Web));

            Console.WriteLine($"created: {data.Id} {data.Nonce} {data.Username}");
            return data;
        }

        
    }
}