using FirebaseAdmin;
using FirebaseAdmin.Messaging;
using Google.Apis.Auth.OAuth2;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HandItOver.BackEnd.DAL.Repositories
{
    public class FirebaseRepository
    {
        public FirebaseRepository(IConfiguration configuration)
        {
            var firebaseOptions = configuration.GetSection("Firebase").Get<FirebaseSettings>();
            var firebaseOptionsJson = JsonConvert.SerializeObject(firebaseOptions);
            var firebaseApp = FirebaseApp.Create(new AppOptions
            {
                Credential = GoogleCredential.FromJson(firebaseOptionsJson)
            });
        }

        public Task SendMessageAsync(string clientToken, string title, string body)
        {
            var message = new Message
            {
                Notification = new Notification
                {
                    Title = title,
                    Body = body
                },
                Token = clientToken
            };
            return FirebaseMessaging.DefaultInstance.SendAsync(message);
        }

        private class FirebaseSettings
        {
            [JsonProperty("type")]
            public string Type { get; set; } = null!;

            [JsonProperty("project_id")]
            public string ProjectId { get; set; } = null!;

            [JsonProperty("private_key_id")]
            public string PrivateKeyId { get; set; } = null!;

            [JsonProperty("private_key")]
            public string PrivateKey { get; set; } = null!;

            [JsonProperty("client_email")]
            public string ClientEmail { get; set; } = null!;

            [JsonProperty("client_id")]
            public string ClientId { get; set; } = null!;

            [JsonProperty("auth_uri")]
            public string AuthUri { get; set; } = null!;

            [JsonProperty("token_uri")]
            public string TokenUri { get; set; } = null!;

            [JsonProperty("auth_provider_x509_cert_url")]
            public string AuthProviderX509CertUrl { get; set; } = null!;

            [JsonProperty("client_x509_cert_url")]
            public string ClientX509CertUrl { get; set; } = null!;
        }
    }
}
