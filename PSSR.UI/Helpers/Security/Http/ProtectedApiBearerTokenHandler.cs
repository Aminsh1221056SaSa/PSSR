using IdentityModel.Client;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace PSSR.UI.Helpers.Security.Http
{
    public class ProtectedApiBearerTokenHandler : DelegatingHandler
    {
        private readonly IIdentityServerClient _identityServerClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ProtectedApiBearerTokenHandler(
        IIdentityServerClient identityServerClient, IHttpContextAccessor _accessor)
        {
            _identityServerClient = identityServerClient
                ?? throw new ArgumentNullException(nameof(identityServerClient));

            _httpContextAccessor = _accessor
               ?? throw new ArgumentNullException(nameof(identityServerClient));

        }
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // request the access token
            //var accessToken = await _identityServerClient.RequestClientCredentialsTokenAsync();
            var accessToken = await _httpContextAccessor.HttpContext.GetTokenAsync("access_token");

            // set the bearer token to the outgoing request
            request.SetBearerToken(accessToken);

            // Proceed calling the inner handler, that will actually send the request
            // to our protected api
            return await base.SendAsync(request, cancellationToken);
        }
    }
}
