// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2016. All rights reserved.
// Wallsmedia LTD 2015-2017:{Alexander Paskhin}

//
// Process Manager Solution Client

using System;
using System.Linq;

namespace DotNet.RestApi.Client
{
    /// <summary>
    /// DEfines the Uri path for Web Rest Api request
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
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
            var attribute = restRequest.GetType().GetCustomAttributes(typeof(RequestPathAttribute),true).FirstOrDefault() as RequestPathAttribute;
            if (attribute != null)
            {
                return attribute.RestApiPath;
            }
            return String.Empty;
        }

    }
}
