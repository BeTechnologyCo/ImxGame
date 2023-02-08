﻿using System;
using System.Threading.Tasks; using Cysharp.Threading.Tasks;
using Nethereum.ABI.FunctionEncoding;
using Nethereum.Hex.HexTypes;
using Nethereum.JsonRpc.Client;
using Nethereum.RPC.Eth.Transactions;
using Nethereum.RPC.TransactionManagers;

namespace Nethereum.Contracts.TransactionHandlers
{
#if !DOTNET35

    public class TransactionEstimatorHandler<TFunctionMessage> :
        TransactionHandlerBase<TFunctionMessage>, 
        ITransactionEstimatorHandler<TFunctionMessage> where TFunctionMessage : FunctionMessage, new()
    {

        public TransactionEstimatorHandler(ITransactionManager transactionManager) : base(transactionManager)
        {

        }

        public async UniTask<HexBigInteger> EstimateGasAsync(string contractAddress, TFunctionMessage functionMessage = null)
        {
            if (functionMessage == null) functionMessage = new TFunctionMessage();
            SetEncoderContractAddress(contractAddress);
            var callInput = FunctionMessageEncodingService.CreateCallInput(functionMessage);
            try
            {
                if (TransactionManager.EstimateOrSetDefaultGasIfNotSet)
                {
                    return await TransactionManager.EstimateGasAsync(callInput);
                }

                return null;
            }
            catch(RpcResponseException rpcException)
            {
                ContractRevertExceptionHandler.HandleContractRevertException(rpcException);
                throw;
            }
            catch(Exception)
            {
                var ethCall = new EthCall(TransactionManager.Client);
                var result = await ethCall.SendRequestAsync(callInput);
                new FunctionCallDecoder().ThrowIfErrorOnOutput(result);
                throw;
            }
        }
    }
#endif
}