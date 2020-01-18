// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2019. All rights reserved.
// Wallsmedia LTD 2019:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using DotNet.RestApi.Client;

namespace WebRestApiAbstractions.Market
{
    [RequestPath("rest-api/market/ok/coin")]
    public class TickerBitcoinRequest
    {
        public enum TickerCommand
        {
            GetLastTop
        }
        public TickerCommand Request { get; set; }
    }


}
