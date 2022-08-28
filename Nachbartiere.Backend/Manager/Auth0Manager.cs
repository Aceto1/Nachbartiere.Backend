using Auth0.ManagementApi;
using Nachbartiere.Backend.Helper;
using Nachbartiere.Backend.Model;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace Nachbartiere.Backend.Manager
{
    public static class Auth0Manager
    {
        private static string accessToken;
        private static bool isLoading = false;

        static Auth0Manager()
        {
            accessToken = GetAccessToken();
            Client = new ManagementApiClient(accessToken, new Uri(Program.Configuration["Auth0:Audience"]));
        }

        private static string GetAccessToken()
        {
            var httpClient = new HttpClient();
            var configSection = Program.Configuration.GetSection("Auth0");

            string body = "grant_type=client_credentials&" +
                          $"client_id={configSection.GetSection("ClientId").Value}&" +
                          $"client_secret={configSection.GetSection("ClientSecret").Value}&" +
                          $"audience={configSection.GetSection("Audience").Value}";

            HttpContent httpContent = new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded");

            httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/x-www-form-urlencoded");

            var response = httpClient.PostAsync("https://nachbartiere.eu.auth0.com/oauth/token", httpContent);

            response.Wait();

            var stringTask = response.Result.Content.ReadAsStringAsync();

            stringTask.Wait();

            var accessTokenResult = JsonSerializer.Deserialize<AccessTokenResult>(stringTask.Result);

            return accessTokenResult.AccessToken;
        }

        private static ManagementApiClient client;

        public static ManagementApiClient Client
        {
            get
            {
                if (JWTHelper.IsTokenExpired(accessToken))
                {
                    if (isLoading)
                    {
                        while (isLoading)
                            Thread.Sleep(25);
                    }
                    else
                    {
                        isLoading = true;
                        accessToken = GetAccessToken();
                        client = new ManagementApiClient(accessToken, new Uri(Program.Configuration["Auth0:Audience"]));
                        isLoading = false;
                    }
                }

                return client;
            }
            private set
            {
                client = value;
            }
        }
    }
}
