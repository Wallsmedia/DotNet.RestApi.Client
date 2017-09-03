// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2017. All rights reserved.
// Wallsmedia LTD 2015-2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Simple Rest API Client
// Dot NET Core Rest API client

using System.Net.Http;
using System.Xml;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.IO;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System.IO.Compression;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System;
using System.Xml.Linq;

namespace DotNet.RestApi.Client
{
    /// <summary>
    /// Simple WEB Rest API client class extensions and helpers.
    /// </summary>
    public static class RestApiClientExtensions
    {
        /// <summary>
        /// Process response if it has gzip compression.
        /// </summary>
        /// <param name="response">The HTTP response message including the status code and data.</param>
        /// <returns> The task object representing the asynchronous operation.</returns>
        public static async Task<string> ReadContentAsStringGzip(this HttpResponseMessage response)
        {
            // Check whether response is compressed
            if (response.Content.Headers.ContentEncoding.Any(x => x.ToLower() == "gzip"))
            {
                // Decompress gzip stream
                using (var s = await response.Content.ReadAsStreamAsync())
                {
                    using (var decompressed = new GZipStream(s, CompressionMode.Decompress))
                    {
                        using (var reader = new StreamReader(decompressed))
                        {
                            return await reader.ReadToEndAsync();
                        }
                    }
                }
            }
            return await response.Content.ReadAsStringAsync();
        }

        /// <summary>
        /// Process response if it has gzip compression.
        /// </summary>
        /// <param name="response">The HTTP response message including the status code and data.</param>
        /// <returns> The stream that represents the content .</returns>
        public static async Task<Stream> ReadContentAsStreamGzip(this HttpResponseMessage response)
        {
            // Check whether response is compressed
            if (response.Content.Headers.ContentEncoding.Any(x => x.ToLower() == "gzip"))
            {
                // Decompress gzip stream
                var s = await response.Content.ReadAsStreamAsync();
                return new GZipStream(s, CompressionMode.Decompress);
            }
            return await response.Content.ReadAsStreamAsync();
        }

        /// <summary>
        /// Applies the Accept-Encoding "gzip" for HTTP request. 
        /// </summary>
        /// <param name="request">The HTTP request message.</param>
        public static void ApplyAcceptEncodingSettingGZip(HttpRequestMessage request)
        {
            const string methodName = "gzip";
            bool found = false;
            foreach (StringWithQualityHeaderValue encoding in request.Headers.AcceptEncoding)
            {
                if (methodName.Equals(encoding.Value, StringComparison.OrdinalIgnoreCase))
                {
                    found = true;
                    break;
                }
            }

            if (!found)
            {
                request.Headers.AcceptEncoding.Add(new StringWithQualityHeaderValue(methodName));
            }
        }

        /// <summary>
        /// Extracts the JSON object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T DeseriaseJsonResponse<T>(this HttpResponseMessage response)
        {
            string respStr = response.ReadContentAsStringGzip().Result;
            if (!string.IsNullOrWhiteSpace(respStr))
            {
                return GetJsonObject<T>(respStr);
            }
            return default(T);
        }

        /// <summary>
        /// Extracts the XML object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T DeseriaseXmlResponse<T>(this HttpResponseMessage response)
        {
            string respStr = response.ReadContentAsStringGzip().Result;
            if (!string.IsNullOrWhiteSpace(respStr))
            {
                return GetXmlObject<T>(respStr);
            }
            return default(T);
        }

        /// <summary>
        /// Extracts the Data Contract XML object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="response">The HTTP response message including the status code and data.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T DeseriaseDcXmlResponse<T>(this HttpResponseMessage response)
        {
            string respStr = response.ReadContentAsStringGzip().Result;
            if (!string.IsNullOrWhiteSpace(respStr))
            {
                return GetDcXmlObject<T>(respStr);
            }
            return default(T);
        }

        /// <summary>
        /// Extracts the Data Contract XML object from the HTTP response message.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="response">The HTTP response message including the status code and dataX.</param>
        /// <returns>XML element.</returns>
        public static XElement ParseXmlResponse(this HttpResponseMessage response)
        {
            using (var respStr = response.ReadContentAsStreamGzip().Result)
            {
                return  XElement.Load(respStr);
            }
        }

        /// <summary>
        /// Extracts the JSON object from the string.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="json">The serialized object string.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T GetJsonObject<T>(string json)
        {
            using (var str = new StringReader(json))
            using (var jsonReader = new JsonTextReader(str))
            {
                JsonSerializer serializer = new JsonSerializer();
                T res = (T)serializer.Deserialize(jsonReader, typeof(T));
                return res;
            }
        }

        /// <summary>
        /// Extracts the XML object from the string.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="xml">The serialized object string.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T GetXmlObject<T>(string xml)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (var str = new StringReader(xml))
            {
                T res = (T)serializer.Deserialize(str);
                return res;
            }
        }

        /// <summary>
        /// Extracts the XML object from the string.
        /// </summary>
        /// <typeparam name="T">The expected type of the response object.</typeparam>
        /// <param name="xml">The serialized object string.</param>
        /// <returns>The deserialized object of the type.</returns>
        public static T GetDcXmlObject<T>(string dcXml)
        {
            DataContractSerializer serializer = new DataContractSerializer(typeof(T));
            using (var str = new StringReader(dcXml))
            using (var xml = XmlReader.Create(str))
            {
                T res = (T)serializer.ReadObject(xml);
                return res;
            }
        }

        /// <summary>
        /// Serialize the object into the JSON string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The instance of the object.</param>
        /// <returns>The serialized string.</returns>
        public static string GetJsonString<T>(T json)
        {
            string serialized;
            using (StringWriter ms = new StringWriter())
            {
                JsonSerializer serializer = new JsonSerializer();
                serializer.Serialize(ms, json);
                serialized = ms.ToString();
            }
            return serialized;
        }

        /// <summary>
        /// Serialize the object into the XML string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The instance of the object.</param>
        /// <returns>The serialized string.</returns>
        public static string GetXmlString<T>(T xml)
        {
            string serialized;
            using (MemoryStream ms = new MemoryStream())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                serializer.Serialize(ms, xml);
                ms.Position = 0;
                using (var sr = new StreamReader(ms))
                {
                    serialized = sr.ReadToEnd();
                }
            }
            return serialized;
        }

        /// <summary>
        /// Serialize the object into the DcXML string.
        /// </summary>
        /// <typeparam name="T">The object type.</typeparam>
        /// <param name="json">The instance of the object.</param>
        /// <returns>The serialized string.</returns>
        public static string GetDcXmlString<T>(T dcxml)
        {
            string serialized;
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    DataContractSerializer serializer = new DataContractSerializer(typeof(T));
                    serializer.WriteObject(ms, dcxml);
                    ms.Position = 0;
                    using (var sr = new StreamReader(ms))
                    {
                        serialized = sr.ReadToEnd();
                    }
                }
            }

            return serialized;
        }

    }
}
