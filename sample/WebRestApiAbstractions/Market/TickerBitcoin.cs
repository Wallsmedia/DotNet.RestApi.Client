// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2019. All rights reserved.
// Wallsmedia LTD 2019:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Newtonsoft.Json;

namespace WebRestApiAbstractions.Market
{


    /// <summary>
    /// Represents the 3rd party ticker object of BiTCoin incl. euro price.
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class TickerBitcoin
    {
        [JsonProperty]
        public string id { get; set; }

        [JsonProperty]
        public string name { get; set; }

        [JsonProperty]
        public string symbol { get; set; }

        [JsonProperty]
        public string rank { get; set; }

        [JsonProperty]
        public decimal price_usd { get; set; }

        [JsonProperty]
        public decimal price_btc { get; set; }

        [JsonProperty("24h_volume_usd")]
        public decimal t24h_volume_usd { get; set; }

        [JsonProperty]
        public decimal market_cap_usd { get; set; }

        [JsonProperty]
        public decimal price_eur { get; set; }

        [JsonProperty("24h_volume_eur")]
        public decimal t24h_volume_eur { get; set; }

        [JsonProperty]
        public decimal market_cap_eur { get; set; }

        [JsonProperty]
        public decimal available_supply { get; set; }

        [JsonProperty]
        public decimal total_supply { get; set; }

        [JsonProperty]
        public decimal max_supply { get; set; }

        [JsonProperty]
        public decimal percent_change_1h { get; set; }

        [JsonProperty]
        public decimal percent_change_24h { get; set; }

        [JsonProperty]
        public decimal percent_change_7d { get; set; }

        [JsonProperty]
        public long last_updated { get; set; }

        DateTime Fecha = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        [JsonIgnore]
        public DateTime LastUpdatedUTC
        {
            get
            {
                return Fecha + TimeSpan.FromSeconds(last_updated);
            }

        }

        [JsonProperty]
        public string error { get; set; }

    }

}
