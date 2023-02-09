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
    private static string serviceUrl = "https://localhost:7062/api/register";


    static RegisterService()
    {
        serviceUrl = "https://localhost:7062/api/register";

#if UNITY_EDITOR
        serviceUrl = "https://localhost:7062/api/register";
#endif

        //GameContext.Instance.Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJlbWFpbCI6IjB4NjcwY0FjZjQ4QjY4NWVCMUFGOGRjNzNDNThBQWJkMzBhQTM1OTU4RSIsImp0aSI6ImIzMzJlMzMyLTAxMWQtNGJiZi1iMGFjLTU3MmJjMWU2YmM4MCIsImV4cCI6MTY3NTk0NTE3NX0.1HGytwdV0ZWu5Jyv7XudX5qg31EYWIncmSeQPhYas7o";

    }

    public static async Task<PlayerDto> CreatePlayer(string name)
    {
        Debug.Log("CreatePlayer");

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GameContext.Instance.Token);

            var response = await client.PostAsJsonAsync<PlayerName>(serviceUrl + "/CreatePlayer", new PlayerName()
            {
                Name = name
            });

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
    }

    public static async Task<PlayerDto> GetPlayer()
    {
        Debug.Log("GetPlayer");

        using (var client = new HttpClient())
        {
            client.DefaultRequestHeaders.Add("Authorization", "Bearer " + GameContext.Instance.Token);

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
}
