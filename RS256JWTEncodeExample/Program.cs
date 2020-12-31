using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RS256JWTEncodeExample.Enums;

namespace RS256JWTEncodeExample
{
    class Program
    {
        static async Task Main(String[] args)
        {
            String token = "token";
            String url = "url";
            Dictionary<String, String> parameters = new Dictionary<String, String>()
            {
                //all query parameter here like below
                { "productId", "123" }
            };

            String jsonPayload = "some json data here...";
            String encodedBody = RS256Encoder.GetSignBodyPayloadForRequest(jsonPayload);

            String response = await ClientWrapper.SendRequestGet(token, url, encodedBody, parameters, ApiRequest.ContentTypes.TEXT);

            Console.WriteLine(response);
            Console.ReadKey();
        }
    }
}
