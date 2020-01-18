// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2019. All rights reserved.
// Wallsmedia LTD 2019:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace WebRestApiServer.Controllers
{
    public static class LogMessageResources
    {

        public static Action<ILogger, string, Exception> OperationException => LoggerMessage.Define<string>
        (LogLevel.Error, 1, "Operation '{OpName}' throws Exception.");

        public static Action<ILogger, string, Exception> StartHostProcess => LoggerMessage.Define<string>
        (LogLevel.Information, 2, "Start '{Process}' hosted process .");

        public static Action<ILogger, string, Exception> StopHostProcess => LoggerMessage.Define<string>
        (LogLevel.Information, 3, "Stop '{Process}' hosted process .");

        public static Action<ILogger, DateTime, decimal, Exception> TraceBitCoinPrice => LoggerMessage.Define<DateTime, decimal>
        (LogLevel.Trace, 4, "BitCoin Time {Time} ; Price :'{Process}'EUR.");

        public static Action<ILogger, string, string, Exception> ValidationError => LoggerMessage.Define<string, string>
        (LogLevel.Trace, 5, "Validation Errors for operation '{Method}', Reason: '{Reason}' .");

        public static Action<ILogger, string, string, decimal, Exception> TraceAddOrder => LoggerMessage.Define<string, string, decimal>
       (LogLevel.Trace, 6, "Add new Trade Order Trader Name: {TraderName}; Order Side: {Side} ; Amount: {Amount} ");

    }
}