// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2017. All rights reserved.
// Wallsmedia LTD 2015-2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Tests of Simple Rest API Client
// Dot NET Core Rest API client

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace RestApiClentTest
{
    public class TestWebHost : IDisposable
    {
        public IWebHost Host { get; set; }
        public void StarWebHost(string serverUrl = "http://*:15000")
        {
            var host = new WebHostBuilder()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseUrls(serverUrl)
                .UseStartup<Startup>()
                .UseKestrel()
                .Build();
            host.Start();
        }

        public class Startup
        {
            public void ConfigureServices(IServiceCollection services)
            {
            }
            public void Configure(IApplicationBuilder app)
            {
                app.UseMiddleware<RequestGRabber>();
            }
        }

        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Host?.Dispose();
                }

                disposedValue = true;
            }
        }
        // This code added to correctly implement the disposable pattern.
        public void Dispose()
        {
            Dispose(true);
        }
        #endregion



    }

    internal class RequestGRabber
    {

        private readonly RequestDelegate _next;
        public static HttpContext HttpContext { get; set; }
        public static string Message { get; set; }
        public RequestGRabber(RequestDelegate next)
        {
            _next = next;
        }

        public Task Invoke(HttpContext context)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                context.Request.Body.CopyTo(ms);
                StreamReader sr = new StreamReader(ms);
                ms.Position = 0;
                Message = sr.ReadToEnd();
                ms.Position = 0;
                ms.CopyTo(context.Response.Body);
                context.Response.StatusCode = StatusCodes.Status200OK;

                //await _next(context);
            }
            catch
            {
            }

            return Task.CompletedTask;
        }
    }
}