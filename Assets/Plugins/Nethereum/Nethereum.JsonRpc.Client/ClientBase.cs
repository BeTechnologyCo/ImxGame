#if !DOTNET35
using System;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Nethereum.JsonRpc.Client.RpcMessages;

namespace Nethereum.JsonRpc.Client
{
    public abstract class ClientBase : IClient
    {

        public static TimeSpan ConnectionTimeout { get; set; } = TimeSpan.FromSeconds(20.0);

        public RequestInterceptor OverridingRequestInterceptor { get; set; }

        public async UniTask<T> SendRequestAsync<T>(RpcRequest request, string route = null)
        {
            if (OverridingRequestInterceptor != null)
                return
                    (T)
                    await OverridingRequestInterceptor.InterceptSendRequestAsync(SendInnerRequestAsync<T>, request, route)
                        ;
            return await SendInnerRequestAsync<T>(request, route);
        }

        public virtual async UniTask<RpcRequestResponseBatch> SendBatchRequestAsync(RpcRequestResponseBatch rpcRequestResponseBatch)
        {
            var responses = await SendAsync(rpcRequestResponseBatch.GetRpcRequests());
            rpcRequestResponseBatch.UpdateBatchItemResponses(responses);
            return rpcRequestResponseBatch;
        }

        public async UniTask<T> SendRequestAsync<T>(string method, string route = null, params object[] paramList)
        {
            if (OverridingRequestInterceptor != null)
                return
                    (T)
                    await OverridingRequestInterceptor.InterceptSendRequestAsync(SendInnerRequestAsync<T>, method, route,
                        paramList);
            return await SendInnerRequestAsync<T>(method, route, paramList);
        }

        protected void HandleRpcError(RpcResponseMessage response, string reqMsg)
        {
            if (response.HasError)
                throw new RpcResponseException(new RpcError(response.Error.Code, response.Error.Message + ": " + reqMsg,
                    response.Error.Data));
        }

        private async UniTask<T> SendInnerRequestAsync<T>(RpcRequestMessage reqMsg,
                                                       string route = null)
        {
            var response = await SendAsync(reqMsg, route);
            HandleRpcError(response, reqMsg.Method);
            try
            {
                return response.GetResult<T>();
            }
            catch (FormatException formatException)
            {
                throw new RpcResponseFormatException("Invalid format found in RPC response", formatException);
            }
        }

        protected virtual UniTask<T> SendInnerRequestAsync<T>(RpcRequest request, string route = null)
        {
            var reqMsg = new RpcRequestMessage(request.Id,
                                               request.Method,
                                               request.RawParameters);
            return SendInnerRequestAsync<T>(reqMsg, route);
        }

        protected virtual UniTask<T> SendInnerRequestAsync<T>(string method, string route = null,
            params object[] paramList)
        {
            var request = new RpcRequestMessage(Guid.NewGuid().ToString(), method, paramList);
            return SendInnerRequestAsync<T>(request, route);
        }

        public virtual async UniTask SendRequestAsync(RpcRequest request, string route = null)
        {
            var response =
                await SendAsync(
                        new RpcRequestMessage(request.Id, request.Method, request.RawParameters), route)
                    ;
            HandleRpcError(response, request.Method);
        }

        protected abstract UniTask<RpcResponseMessage> SendAsync(RpcRequestMessage rpcRequestMessage, string route = null);
        protected abstract UniTask<RpcResponseMessage[]> SendAsync(RpcRequestMessage[] requests);
        public virtual async UniTask SendRequestAsync(string method, string route = null, params object[] paramList)
        {
            var request = new RpcRequestMessage(Guid.NewGuid().ToString(), method, paramList);
            var response = await SendAsync(request, route);
            HandleRpcError(response, method);
        }
    }
}
#endif