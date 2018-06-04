// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2018. All rights reserved.
// Wallsmedia LTD 2015-2018:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Simple Rest API Client
// Dot NET Core Rest API client

using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Text;

namespace DotNet.RestApi.Client
{
    /// <summary>
    /// Simple REST API client.
    /// </summary>
    public class RestApiClient : IDisposable
    {
        public static readonly MediaTypeHeaderValue ApplicationJson = MediaTypeHeaderValue.Parse("application/json");
        public static readonly MediaTypeWithQualityHeaderValue ApplicationJsonQ = MediaTypeWithQualityHeaderValue.Parse("application/json");

        public static readonly MediaTypeHeaderValue ApplicationXml = MediaTypeHeaderValue.Parse("application/xml");
        public static readonly MediaTypeWithQualityHeaderValue ApplicationXmlQ = MediaTypeWithQualityHeaderValue.Parse("application/xml");

        Uri _baseUri;
        HttpClient _client;
        Action<HttpRequestMessage> _configureRequst;

        /// <summary>
        /// Cancellation token source. It can be used to cancel send operation.
        /// </summary>
        public CancellationTokenSource CancellationTokenSource { get; } = new CancellationTokenSource();

        /// <summary>
        /// Gets the underling <see cref="HttpClient"/> client.
        /// </summary>
        public HttpClient Client { get => EnsureHttpClient(); set => _client = value; }


        /// <summary>
        /// Constructs the simple Rest WEB API client.
        /// </summary>
        /// <param name="baseUri">The base Uri to the WEB API service</param>
        /// <param name="configureRequst">The optional conflagration delegate for the request message.</param>
        public RestApiClient(Uri baseUri, Action<HttpRequestMessage> configureRequst = null)
        {
            _baseUri = baseUri;
            _configureRequst = configureRequst;
        }

        /// <summary>
        /// Create new client or return created.
        /// </summary>
        /// <returns>HTTP client.</returns>
        private HttpClient EnsureHttpClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
                _client.BaseAddress = _baseUri;
            }
            return _client;
        }

        /// <summary>
        /// Sends the JSON REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="json">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> SendJsonRequest<T>(HttpMethod method, Uri uri, T json)
        {
            string serialized = string.Empty;
            if (json != null)
            {
                serialized = RestApiClientExtensions.GetJsonString(json);
            }
            return SendJsonRequest(method, uri, serialized);
        }


        /// <summary>
        /// Sends the XML REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="xml">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> SendXmlRequest<T>(HttpMethod method, Uri uri, T xml)
        {
            string serialized = string.Empty;
            if (xml != null)
            {
                serialized = RestApiClientExtensions.GetXmlString(xml);
            }
            return SendXmlRequest(method, uri, serialized);
        }

        /// <summary>
        /// Sends the Data Contract XML REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="dcxml">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> SendDcXmlRequest<T>(HttpMethod method, Uri uri, T dcxml)
        {
            string serialized = string.Empty;
            if (dcxml != null)
            {
                serialized = RestApiClientExtensions.GetDcXmlString(dcxml);
            }
            return SendXmlRequest(method, uri, serialized);
        }

        /// <summary>
        /// Sends the XML REST WEB API request.
        /// </summary>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST  WEB resource.</param>
        /// <param name="json">The JSON string to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> SendJsonRequest(HttpMethod method, Uri uri, string json)
        {
            var client = EnsureHttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Accept.Add(ApplicationJsonQ);
            request.Method = method;
            request.RequestUri = uri;
            request.Content = new StringContent(json, Encoding.UTF8, ApplicationJson.MediaType);
            //request.Content.Headers.ContentType = ApplicationJson;
            _configureRequst?.Invoke(request);
            return client.SendAsync(request, CancellationTokenSource.Token);
        }

        /// <summary>
        /// Sends the XML REST API request.
        /// </summary>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST  WEB resource.</param>
        /// <param name="xml">The XML string to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        public Task<HttpResponseMessage> SendXmlRequest(HttpMethod method, Uri uri, string xml)
        {
            var client = EnsureHttpClient();
            HttpRequestMessage request = new HttpRequestMessage();
            request.Headers.Accept.Add(ApplicationXmlQ);
            request.Method = method;
            request.RequestUri = uri;
            request.Content = new StringContent(xml, Encoding.UTF8, ApplicationXml.MediaType);
            //request.Content.Headers.ContentType = ApplicationXml;
            _configureRequst?.Invoke(request);
            return client.SendAsync(request, CancellationTokenSource.Token);
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Client?.Dispose();
                }
                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion

    }
}
