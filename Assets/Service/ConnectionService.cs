using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using UnityEngine;
using System.Timers;


public class ConnectionService
{
    private static Timer aTimer;
    static HttpClient client = new HttpClient();
    private static string serviceUrl = "https://localhost:7062/api/login";


    static ConnectionService()
    {
        serviceUrl = "https://localhost:7062/api/login";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7062/api/login";
#endif

    }

    public static async Task CreateAccountAsync(string userName, string email, string password)
    {

        Debug.Log("CreateAccountAsync");
        var response = await client.PostAsJsonAsync(serviceUrl + "/Account/Register", new { email, password });

        // Ignore 409 responses, as they indicate that the account already exists.
        if (response.StatusCode == HttpStatusCode.Conflict)
        {
            return;
        }

        response.EnsureSuccessStatusCode();
        Debug.Log("Account created");
    }

    public static async Task<string> GetTokenAsync(LoginVM loginInfo)
    {
        Debug.Log("GetTokenAsync");

        var response = await client.PostAsJsonAsync<LoginVM>(serviceUrl + "/CreateToken", loginInfo);

        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<TokenResponse>(res);
            Debug.Log("token created");

            return payload.Token;
        }
        else
        {
            throw new InvalidOperationException("Error server " + response.ReasonPhrase);
        }
    }

    public static async Task<MessageVM> RequestConnection(string account)
    {
        Debug.Log("GetTokenAsync");

        var url = $"{serviceUrl}/RequestConnection?account={account}";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<MessageVM>(res);
            Debug.Log("requestconnection");

            return payload;
        }
        else
        {
            throw new InvalidOperationException("Error server " + response.ReasonPhrase);
        }
    }
}
