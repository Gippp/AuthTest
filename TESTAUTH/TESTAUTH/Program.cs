using DotNetOpenAuth.OAuth2;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;

namespace TESTAUTH
{
    class Program
    {
        static void Main(string[] args)
        {
            var serverDescription = new AuthorizationServerDescription
            {
                TokenEndpoint = new Uri("https://auth.cloud-test.renlife.com/auth/realms/test/protocol/openid-connect/token")
            };
            var client = new WebServerClient(serverDescription)
            {
                ClientIdentifier = "creatio_crm",
                ClientCredentialApplicator = ClientCredentialApplicator.PostParameter("Тут должен быть секрет")
            };

            // Получаем токен.
            var token = client.GetClientAccessToken().AccessToken.ToString();
            Console.WriteLine(token);

            // стучимся в сервис. без токена возращает 401 ошибку, с токеном 404 (т.е. авторизация прошла).
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var result = httpClient.GetAsync("https://storage.cloud-test.renlife.com/auth").Result;

            Console.WriteLine(result);
        }
    }
}
