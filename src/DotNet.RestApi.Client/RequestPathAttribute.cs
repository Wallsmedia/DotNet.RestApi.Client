// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2018. All rights reserved.
// Wallsmedia LTD 2015-2018:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Simple Rest API Client
// Dot NET Core Rest API client

using System;
using System.Linq;

namespace DotNet.RestApi.Client
{
    /// <summary>
    /// DEfines the Uri path for Web Rest Api request
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class RequestPathAttribute : Attribute
    {
        /// <summary>
        /// Gets the uri string path for the request. 
        /// </summary>
        public string RestApiPath { get; }

        /// <summary>
        /// Constructs the class.
        /// </summary>
        /// <param name="path">The request path.</param>
        public RequestPathAttribute(string path)
        {
            RestApiPath = path;
        }

        /// <summary>
        /// Extracts the request path from the object.
        /// </summary>
        /// <param name="restRequest">The request object.</param>
        /// <returns></returns>
        public static string GetRestApiPath(object restRequest)
        {
            if (restRequest.GetType().GetCustomAttributes(typeof(RequestPathAttribute),true).FirstOrDefault() is RequestPathAttribute attribute)
            {
                return attribute.RestApiPath;
            }
            return String.Empty;
        }

    }
}
