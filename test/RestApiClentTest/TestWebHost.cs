// \\     |/\  /||
//  \\ \\ |/ \/ ||
//   \//\\/|  \ || 
// Copyright © Alexander Paskhin 2013-2017. All rights reserved.
// Wallsmedia LTD 2015-2017:{Alexander Paskhin}
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
// Tests of Simple Rest API Client
// DotNET Core Rest API client

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;

namespace RestApiClentTest
{
    public class TestWebHost : IDisposable
    {
        public IHost Host2 { get; set; }
        public string Message { get; private set; }


        public void StarWebHost(string serverUrl = "http://localhost:15000")
        {

            var host = Host.CreateDefaultBuilder()
              .ConfigureServices(services =>
                services.AddResponseCompression()
                )
                .ConfigureWebHost(builder =>
              {
                  builder.UseKestrel()
                  .UseUrls(serverUrl)
                  .Configure(app =>
                  {
                      app.UseResponseCompression();
                      app.Run( async (context) =>
                      {
                          foreach (var h in context.Request.Headers)
                          {
                              context.Response.Headers.Add(h);
                          }
                          MemoryStream ms = new MemoryStream();
                          await context.Request.Body.CopyToAsync(ms);
                          StreamReader sr = new StreamReader(ms);
                          ms.Position = 0;
                          Message = sr.ReadToEnd();
                          ms.Position = 0;
                          context.Response.StatusCode = StatusCodes.Status200OK;
                          await ms.CopyToAsync(context.Response.Body);
                      });
                  });
              })
              .Build();
            Host2 = host;
            host.Start();
        }



        #region IDisposable Support
        private bool disposedValue = false; // To detect redundant calls

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    Host2?.Dispose();
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
}