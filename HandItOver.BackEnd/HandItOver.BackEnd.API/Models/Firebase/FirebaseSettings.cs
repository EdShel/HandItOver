using Newtonsoft.Json;

namespace HandItOver.BackEnd.API.Models.Firebase
{
    public class FirebaseSettings
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
