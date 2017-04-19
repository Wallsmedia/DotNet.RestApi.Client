# Dot Net Core Web Rest Api Client
### Nuget Package: 
https://www.nuget.org/packages/DotNet.RestApi.Client/


Recently, have been released that cannot  find, over the "Nuget", the convenient Web Rest Api Client for the prototype Web Rest Web Service at the Docker Container. After spending hours of search have decide to create the simple and small one.

Example how call with JSON:

```
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri);

PurchaseOrder sendObj = new PurchaseOrder();

HttpResponseMessage response = client.SendJsonRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

PurchaseOrder respObj = response.DeseriaseJsonResponse<PurchaseOrder>();

```

Example how call with XML:

```
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri, request =>
   {
      request.Headers.Add("CustomHeader", "CustomHeaderValue");
   });

PurchaseOrder sendObj = new PurchaseOrder();

HttpResponseMessage response = client.SendXmlRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

PurchaseOrder respObj = response.DeseriaseXmlResponse<PurchaseOrder>();

```


Example how call with Data Contract XML:

```
Uri baseUri = new Uri("http://webServiceHost:15002");
RestApiClient client = new RestApiClient(baseUri);

PurchaseOrder sendObj = new PurchaseOrder();

HttpResponseMessage response = client.SendDcXmlRequest(HttpMethod.Post, new Uri("res", UriKind.Relative), sendObj).Result;

PurchaseOrder respObj = response.DeseriaseDcXmlResponse<PurchaseOrder>();

```


Where the Models:

```
   [DataContract (Namespace ="http://puchase.Interface.org/Purchase.Order")]
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
 ```
