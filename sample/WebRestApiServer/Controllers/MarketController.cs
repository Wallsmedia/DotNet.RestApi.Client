// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2019. All rights reserved.
// Wallsmedia LTD 2019:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using DotNet.RestApi.Client;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebRestApiAbstractions.Market;

namespace WebRestApiServer.Controllers
{
    [Route("rest-api/[controller]")]
    [ApiController]
    public class MarketController : ControllerBase
    {
        private RestApiClient _restApiClient;
        private ILoggerFactory _logFactory;
        private ILogger _logInfo;

        public MarketController(RestApiClient restApiClient, ILoggerFactory logFactory)
        {
            _restApiClient = restApiClient;
            restApiClient.ConfigureHttpRequstMessage = RestApiClientExtensions.ApplyAcceptEncodingSettingGZip;
            _logFactory = logFactory;
            _logInfo = _logFactory?.CreateLogger(Assembly.GetExecutingAssembly().GetName().Name);
        }

        public TickerBitcoin LastMarketTop { get; private set; } = null;


        [HttpGet("ok")]
        public ActionResult GetBitCoinRate()
        {
            Uri baseUri = new Uri("https://api.coinmarketcap.com/v1/ticker/bitcoin/?convert=EUR");
            RequestInfo(baseUri);
            return new JsonResult(LastMarketTop);
        }

        [HttpGet("fault")]
        public ActionResult NotGetBitCoinRate()
        {
            Uri baseUri = new Uri("https://api.coin.com/v1/ticker/bitcoin/?convert=EUR");
            RequestInfo(baseUri);
            return new JsonResult(LastMarketTop);
        }

        [HttpPost("ok/coin")]
        public ActionResult PostBitCoinRate(TickerBitcoinRequest request)
        {
            Uri baseUri = new Uri("https://api.coinmarketcap.com/v1/ticker/bitcoin/?convert=EUR");
            RequestInfo(baseUri);
            return new JsonResult(LastMarketTop);
        }

        [HttpPost("ok/coin/fault")]
        public ActionResult NotPostBitCoinRate(TickerBitcoinRequest request)
        {
            Uri baseUri = new Uri("https://api.coin.com/v1/ticker/bitcoin/?convert=EUR");
            RequestInfo(baseUri);
            return new JsonResult(LastMarketTop);
        }

        private void RequestInfo(Uri baseUri)
        {
            try
            {
                var response = _restApiClient.SendJsonRequest(HttpMethod.Get, baseUri, null).GetAwaiter().GetResult();
                var bitcoin = response.DeseriaseJsonResponseAsync<TickerBitcoin[]>().GetAwaiter().GetResult();
                LastMarketTop = bitcoin[0];
                LogMessageResources.TraceBitCoinPrice(_logInfo, bitcoin[0].LastUpdatedUTC, bitcoin[0].price_eur, null);
            }
            catch (Exception e)
            {
                LogMessageResources.OperationException(_logInfo, nameof(MarketController), e);
            }
        }
    }
}