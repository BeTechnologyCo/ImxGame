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


public class RegisterService
{
    private static Timer aTimer;
    static HttpClient client = new HttpClient();
    private static string serviceUrl = "https://localhost:7062/api/register";


    static RegisterService()
    {
        serviceUrl = "https://localhost:7062/api/register";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7062/api/register";
#endif

    }

    public static async Task<PlayerDto> CreatePlayer(string name)
    {
        Debug.Log("CreatePlayer");

        var response = await client.PostAsJsonAsync<PlayerDto>(serviceUrl + "/CreatePlayer", new PlayerDto() { Name = name });

        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<PlayerDto>(res);
            Debug.Log("player created");

            return payload;
        }
        else
        {
            throw new InvalidOperationException("Error server " + response.ReasonPhrase);
        }
    }

    public static async Task<PlayerDto> GetPlayer()
    {
        Debug.Log("GetPlayer");

        var url = $"{serviceUrl}/GetPlayer";

        var response = await client.GetAsync(url);

        if (response.IsSuccessStatusCode)
        {
            var res = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<PlayerDto>(res);
            Debug.Log("player loaded");

            return payload;
        }
        else
        {
            throw new InvalidOperationException("Error server " + response.ReasonPhrase);
        }
    }
}

public class TokenResponse
{
    [JsonProperty("token")]
    public string Token { get; set; }
}

public class ErrorResponse
{
    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_description")]
    public string Description { get; set; }
}

public class LoginVM
{
    public string Signer { get; set; } // Ethereum account that claim the signature
    public string Signature { get; set; } // The signature
    public string Message { get; set; } // The plain message
}

public class UserVM
{
    public string Account { get; set; } // Unique account name (the Ethereum account)
    public string Name { get; set; } // The user name
    public string Email { get; set; } // The user Email
}

public class ConnectionVM
{
    public string Account { get; set; }
    public Guid Nonce { get; set; }
    public DateTime DateTime { get; set; }
}

public class MessageVM
{
    public string Account { get; set; }
    public string Message { get; set; }
}