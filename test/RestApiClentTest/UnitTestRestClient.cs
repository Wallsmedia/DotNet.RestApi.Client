// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2017. All rights reserved.
// Wallsmedia LTD 2015-2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Tests of Simple Rest API Client
// Dot NET Core Rest API client

using DotNet.RestApi.Client;
using System;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization;
using System.Threading;
using Xunit;

namespace RestApiClentTest
{
    [DataContract(Namespace = "http://puchase.Interface.org/Purchase.Order")]
    public class PurchaseOrder
    {
        public PurchaseOrder()
        {
            billTo = new Address() { street = "Bill to Address" };
            shipTo = new Address() { street = "Ship to  Address" };
        }
        [DataMember]
        public Address billTo;
        [DataMember]
        public Address shipTo;
    }

    [DataContract(Namespace = "http://puchase.Interface.org/Purchase.Order.Address")]
    public class Address
    {
        [DataMember]
        public string street;
    }

    public class UnitTestRestClient
    {

        [Fact]
        public void SendJson()
        {
            using (TestWebHost host = new TestWebHost())
            {
                host.StarWebHost("http://*:15000");
                Thread.Sleep(1000);
                Uri baseUri = new Uri("http://localhost:15000");

                RestApiClient client = new RestApiClient(baseUri, request =>
                {
                    request.Headers.Add("CustomHeader", "CustomHeaderValue");
                }
                );

                PurchaseOrder sendObj = new PurchaseOrder();

                HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

                string send = RestApiClientExtensions.GetJsonString(sendObj);
                string json = response.Content.ReadAsStringAsync().Result;
                string rest = RequestGRabber.Message;

                PurchaseOrder respObj = response.DeseriaseJsonResponse<PurchaseOrder>();

                Assert.Equal(send, json);
                Assert.Equal(rest, json);
                Assert.Equal("CustomHeaderValue", response.Headers.GetValues("CustomHeader").First());

            }
        }

        [Fact]
        public void SendXml()
        {
            using (TestWebHost host = new TestWebHost())
            {
                host.StarWebHost("http://*:15001");
                Thread.Sleep(1000);
                Uri baseUri = new Uri("http://localhost:15001");

                RestApiClient client = new RestApiClient(baseUri);

                PurchaseOrder sendObj = new PurchaseOrder();

                HttpResponseMessage response = client.SendXmlRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

                PurchaseOrder respObj = response.DeseriaseXmlResponse<PurchaseOrder>();

                string send = RestApiClientExtensions.GetXmlString(sendObj);
                string xml = response.Content.ReadAsStringAsync().Result;
                string rest = RequestGRabber.Message;

                Assert.Equal(send, xml);
                Assert.Equal(rest, xml);

            }
        }


        [Fact]
        public void SendDcXml()
        {
            using (TestWebHost host = new TestWebHost())
            {
                host.StarWebHost("http://*:15002");
                Thread.Sleep(1000);
                Uri baseUri = new Uri("http://localhost:15002");

                RestApiClient client = new RestApiClient(baseUri);

                PurchaseOrder sendObj = new PurchaseOrder();

                HttpResponseMessage response = client.SendDcXmlRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

                PurchaseOrder respObj = response.DeseriaseDcXmlResponse<PurchaseOrder>();

                string send = RestApiClientExtensions.GetDcXmlString(sendObj);
                string xml = response.Content.ReadAsStringAsync().Result;
                string rest = RequestGRabber.Message;

                Assert.Equal(send, xml);
                Assert.Equal(rest, xml);
            }
        }
    }
}
