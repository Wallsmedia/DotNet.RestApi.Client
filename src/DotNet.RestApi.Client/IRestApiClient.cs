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

namespace DotNet.RestApi.Client
{
    public interface IRestApiClient
    {
        /// <summary>
        /// Cancellation token source. It can be used to cancel send operation.
        /// </summary>
        CancellationTokenSource CancellationTokenSource { get; }

        /// <summary>
        /// Gets the underling <see cref="HttpClient"/> client.
        /// </summary>
        HttpClient Client { get; set; }

        /// <summary>
        /// Sends the JSON REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="json">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendJsonRequest<T>(HttpMethod method, Uri uri, T json);

        /// <summary>
        /// Sends the XML REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="xml">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendXmlRequest<T>(HttpMethod method, Uri uri, T xml);

        /// <summary>
        /// Sends the Data Contract XML REST WEB API request.
        /// </summary>
        /// <typeparam name="T">The generic object type.</typeparam>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST WEB resource.</param>
        /// <param name="dcxml">The class object instance to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendDcXmlRequest<T>(HttpMethod method, Uri uri, T dcxml);

        /// <summary>
        /// Sends the XML REST WEB API request.
        /// </summary>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST  WEB resource.</param>
        /// <param name="json">The JSON string to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendJsonRequest(HttpMethod method, Uri uri, string json);

        /// <summary>
        /// Sends the XML REST API request.
        /// </summary>
        /// <param name="method">The standard HTTP method.</param>
        /// <param name="uri">The uniform resource identifier (URI) to the REST  WEB resource.</param>
        /// <param name="xml">The XML string to send.</param>
        /// <returns>Returns <see cref="HttpResponseMessage"/> task object representing the asynchronous operation.</returns>
        Task<HttpResponseMessage> SendXmlRequest(HttpMethod method, Uri uri, string xml);
    }
}