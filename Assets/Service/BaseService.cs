using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public abstract class BaseService : IDisposable
{

    protected string url = "https://hero.opm.one";
    protected HubConnection connection;

    public abstract string HubName { get; }

    public event EventHandler<string> ServiceError;

    static bool serializerRegistered = false;

    public BaseService()
    {
        url = "http://127.0.0.1:54244";

#if UNITY_EDITOR
        url = "http://127.0.0.1:54244";
#endif

        try
        {
            connection = new HubConnectionBuilder()
                  .WithUrl($"{url}/{HubName}", options =>
                  {
                      options.AccessTokenProvider = () =>
                      {
                          //Debug.Log("Token : " + GameContext.Instance.Token);
                          return Task.FromResult(GameContext.Instance.Token);
                      };
                  })
                  .WithAutomaticReconnect()
                  .Build();

            Start();
        }
        catch (Exception ex)
        {
            Debug.LogException(ex);
            if (ServiceError != null)
            {
                ServiceError(this, ex.Message);
            }
        }
    }

    /// <summary>
    /// Start the connection call after initialize the connection
    /// </summary>
    /// <returns></returns>
    protected abstract Task Start();

    public async virtual void Dispose()
    {
        if (connection != null && connection.State != HubConnectionState.Disconnected)
        {
            await connection.StopAsync();
        }
    }
}
