# Dot Net Core Rest Web Api Client
The convenient Web Rest Api Client for the Rest Web Service at the Docker Container. 

#### Version 3.0.0
-Add support of .Net Core 3.0
-Add support of .Net Standard 2.1

#### Version 2.4.0
- Add support of IHttpClientFactory 
- Add support of IHttpClientFactory 

### Nuget Package: 
https://www.nuget.org/packages/DotNet.RestApi.Client/


## Example of using with IHttpClientFactory ASP.NET Core

- Include the nuget package into the project [Microsoft.Extensions.Http](https://www.nuget.org/packages/Microsoft.Extensions.Http)
- Include the nuget package into the project [DotNet.RestApi.Client](https://www.nuget.org/packages/DotNet.RestApi.Client)

Add to the configuration initialization:

``` c#
.ConfigureServices((webHostBuilderContext, services) =>
  {
        services.AddHttpClient<RestApiClient>();
   })
```

##### Using "Polly" 
- Include the nuget package into the project [Microsoft.Extensions.Polly](https://www.nuget.org/packages/Microsoft.Extensions.Polly)

``` c#
.ConfigureServices((webHostBuilderContext, services) =>
  {
        services.AddTransient<RestApiClient>();
        services.AddHttpClient<RestApiClient>().AddTransientHttpErrorPolicy(p => p.RetryAsync(3));
   })
```

For more details of using Polly use link below:
- https://github.com/App-vNext/Polly
- http://www.thepollyproject.org 


In the controller:

``` c#
    private RestApiClient _restApiClient;

    /// <summary>
    /// Constructs the class.
    /// </summary>
    /// <param name="configuration">The capture processor configuration.</param>
    /// <param name="logFactory">The logger factory.</param>
    public MarketCaptureController(RestApiClient restApiClient,
        MarketCaptureProcessorServiceConfiguration configuration,
        ILoggerFactory logFactory)
    {
        _restApiClient = restApiClient;
        restApiClient.ConfigureHttpRequstMessage = RestApiClientExtensions.ApplyAcceptEncodingSettingGZip;

        Configuration = configuration;
        _logFactory = logFactory;
        _logInfo = _logFactory?.CreateLogger(Assembly.GetExecutingAssembly().GetName().Name);

    }

    private void GetBitCoinRate(object state)
    {

        try
        {
            Uri baseUri = new Uri(Configuration.BitCoinRequestUrl);
            baseUri = new Uri("https://api.coinmarketcap.com/v1/ticker/bitcoin/?convert=EUR");
            var response = _restApiClient.SendJsonRequest(HttpMethod.Get, baseUri, null).GetAwaiter().GetResult();
            var bitcoin = response.DeseriaseJsonResponseAsync<TickerBitcoin[]>().GetAwaiter().GetResult();
            LastMarketTop = bitcoin[0];
            LogMessageResources.TraceBitCoinPrice(_logInfo, bitcoin[0].LastUpdatedUTC, bitcoin[0].price_eur, null);
        }
        catch (Exception e)
        {
            LogMessageResources.OperationException(_logInfo, nameof(MarketCaptureProcessor), e);
        }
    }

```

## Example how to call with JSON:

``` c#
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri);

PurchaseOrder sendObj = new PurchaseOrder();

HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

PurchaseOrder respObj = response.DeseriaseJsonResponse<PurchaseOrder>();

```

## Example how to call with XML:

``` c#
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri, request =>
   {
      request.Headers.Add("CustomHeader", "CustomHeaderValue");
   });

PurchaseOrder sendObj = new PurchaseOrder();

Uri relUri = new Uri(RequestPathAttribute.GetRestApiPath(sendObj), UriKind.Relative);
HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Post, relUri, sendObj).Result;

PurchaseOrder respObj = response.DeseriaseXmlResponse<PurchaseOrder>();

```

## Example how to call with using gzip:

``` c#
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri, request =>
   {
      request.Headers.Add("CustomHeader", "CustomHeaderValue");
      RestApiClientExtensions.ApplyAcceptEncodingSettingGZip(request);
   });

PurchaseOrder sendObj = new PurchaseOrder();

HttpResponseMessage response = client.SendXmlRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

PurchaseOrder respObj = response.DeseriaseXmlResponse<PurchaseOrder>();

```
#### Where Web Service should have:

``` c#
 public void ConfigureServices(IServiceCollection services)
 {
 ...
     services.AddResponseCompression();
 ...
 }
 public void Configure(IApplicationBuilder app)
 {
 ...
     app.UseResponseCompression();
 ...
 }
```

## Example how to call with Data Contract XML:

``` c#
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri);

PurchaseOrder sendObj = new PurchaseOrder();
Uri relUri = new Uri(RequestPathAttribute.GetRestApiPath(sendObj), UriKind.Relative);
HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Post, relUri, sendObj).Result;

PurchaseOrder respObj = response.DeseriaseDcXmlResponse<PurchaseOrder>();

```

#### Where the Models:

``` c#
   [DataContract (Namespace ="http://purchase.Interface.org/Purchase.Order")]
   [RequestPath("add/purchase/order")]
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


    [DataContract(Namespace = "http://purchase.Interface.org/Purchase.Order.Address")]
    public class Address
    {
        [DataMember]
        public string street;
    }
 ```
