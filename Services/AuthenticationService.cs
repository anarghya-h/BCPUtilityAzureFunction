using BCPUtilityAzureFunction.Models.Configs;
using IdentityModel.Client;
using Microsoft.Extensions.Hosting;
//using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using Microsoft.Extensions.Configuration;

namespace BCPUtilityAzureFunction.Services
{
    public sealed class AuthenticationService : IHostedService, IDisposable
    {
        #region Private members variables
        private readonly SdxConfig sdxConfig;
        private readonly IConfiguration config;
        private readonly ILogger logger;
        private Timer refreshTokenTimer;
        #endregion

        #region Public members
        public TokenResponse tokenResponse { get; set; }
        #endregion

        #region Constructors
        public AuthenticationService(SdxConfig sdxConfig, ILogger logger, IConfiguration config)
        {
            this.logger = logger;
            this.sdxConfig = sdxConfig;
            this.config = config;
        }
        #endregion

        #region Public Methods
        public Task StartAsync(CancellationToken cancellationToken)
        {
            tokenResponse = GetOAuthTokenClientCredentialsFlow(sdxConfig.AuthServerAuthority, sdxConfig.ServerResourceID);
            return Task.CompletedTask;
        }
        public Task GetAccessTokenAsync(CancellationToken cancellationToken)
        {
            tokenResponse = GetOAuthTokenClientCredentialsFlow(sdxConfig.AuthServerAuthority, sdxConfig.ServerResourceID);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            logger.Information("Stoping token refresh thread...");

            this.refreshTokenTimer?.Change(Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            logger.Information("AuthenticationService disposing...");

            this.refreshTokenTimer?.Dispose();
        }
        #endregion

        #region Private Methods
        private TokenResponse GetOAuthTokenClientCredentialsFlow(string authServerAuthority, string serverResourceID)
        {
            var client = new HttpClient();

            var discoveryDocument = client.GetDiscoveryDocumentAsync(new DiscoveryDocumentRequest
            {
                Address = authServerAuthority,
                Policy =
                    {
                        ValidateEndpoints = false,
                        ValidateIssuerName = false,
                        RequireHttps = true
                    }
            }).Result;


            var parameters = new Parameters(string.IsNullOrEmpty(serverResourceID) ? new Dictionary<string, string>() : new Dictionary<string, string>() { { "Resource", serverResourceID } });

            var response = client.RequestClientCredentialsTokenAsync(new ClientCredentialsTokenRequest
            {
                Address = discoveryDocument.TokenEndpoint,
                ClientId = config["SDxConfig:AuthClientId"],
                ClientSecret = config["SDxConfig:AuthClientSecret"],
                Scope = serverResourceID,
                Parameters = parameters
            }).Result;

            if (!string.IsNullOrWhiteSpace(response.Error))
            {
                logger.Error(response.Error);
                throw new ArgumentNullException(response.Error);
            }

            if (!string.IsNullOrWhiteSpace(response.AccessToken))
            {
                logger.Information("Token obtained successfully");
            }

            //TokenExpirationCall(response.ExpiresIn, authServerAuthority, authClientId, authClientSecret, serverResourceID);

            return response;
        }

        private void TokenExpirationCall(int timeTillExpires, string authServerAuthority, string authClientId, string authClientSecret, string serverResourceID)
        {
            // Sets token refresh callback before the token expires so that we don't get 401's by using expired tokens
            this.refreshTokenTimer = new Timer(_ =>
            {
                logger.Information("Token expiring obtaining new one...");
                this.tokenResponse = GetOAuthTokenClientCredentialsFlow(authServerAuthority,serverResourceID);
            }
                , null, (int)TimeSpan.FromSeconds(timeTillExpires - 30).TotalMilliseconds, Timeout.Infinite);
            logger.Information("Refresh token timer initialized successfully");
        }

        #endregion
    }
}
