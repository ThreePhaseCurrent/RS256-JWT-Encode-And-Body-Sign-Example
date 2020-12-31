using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using RS256JWTEncodeExample.Enums;
using RS256JWTEncodeExample.Extentions;

namespace RS256JWTEncodeExample
{
    public static class ClientWrapper
    {
        private static async Task<String> SendRequest(HttpMethod method, String url, String token, String content,
            ApiRequest.ContentTypes contentType = ApiRequest.ContentTypes.JSON)
        {
            String responseString = String.Empty;

            SecurityProtocolType securityProtocolBackup = ServicePointManager.SecurityProtocol;
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12; // other security protocols doesn't supported
                using HttpClient httpClient = new HttpClient();
                using HttpRequestMessage request = new HttpRequestMessage(method, url);
                if (!String.IsNullOrEmpty(content))
                {
                    request.Content = new StringContent(content);
                    request.Content.Headers.Clear();
                    request.Content.Headers.Add("Content-Type", contentType.ToDescriptionString());
                }

                if (!String.IsNullOrEmpty(token))
                {
                    request.Headers.Add("Authorization", String.Concat("Token ", token));
                }

                using HttpResponseMessage response = await httpClient.SendAsync(request);
                responseString = await response.Content.ReadAsStringAsync();
                if (!response.IsSuccessStatusCode)
                {
                    //TODO logging or else...
                }
            }
            catch (Exception ex)
            {
                //TODO logging
            }
            finally
            {
                ServicePointManager.SecurityProtocol = securityProtocolBackup;
            }

            return responseString;
        }

        public static async Task<String> SendRequestGet(String token, String url, String body, 
            Dictionary<String, String> parameters, ApiRequest.ContentTypes contentType)
        {
            String queryString = url;
            if (parameters?.Count > 0)
            {
                queryString = AddParametersToQuery(queryString, parameters);
            }
            return await SendRequest(HttpMethod.Get, queryString, token, body, contentType);
        }

        private static String AddParametersToQuery(String queryString, Dictionary<String, String> parameters)
        {
            queryString += "?";
            foreach (var parameter in parameters)
            {
                queryString += $"{parameter.Key}={parameter.Value}&";
            }
            queryString = queryString.TrimEnd('&');

            return queryString.Replace("\r\n", string.Empty);
        }
    }
}
