#if NETCOREAPP2_0

using System.Threading;
using System.Threading.Tasks;
using FastEndpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.eShopWeb.ApplicationCore.Interfaces;
using Microsoft.eShopWeb.Infrastructure.Identity;

namespace Microsoft.eShopWeb.PublicApi.AuthEndpoints
{
    /// <summary>
    /// Authenticates a user
    /// </summary>
    public class AuthenticateEndpoint : Endpoint<AuthenticateRequest, AuthenticateResponse>
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ITokenClaimsService _tokenClaimsService;

        public AuthenticateEndpoint(SignInManager<ApplicationUser> signInManager,
            ITokenClaimsService tokenClaimsService)
        {
            _signInManager = signInManager;
            _tokenClaimsService = tokenClaimsService;
        }

        public override void Configure()
        {
            Post("api/authenticate");
            AllowAnonymous();
            Description(d =>
                d.WithSummary("Authenticates a user")
                 .WithDescription("Authenticates a user")
                 .WithName("auth.authenticate")
                 .WithTags("AuthEndpoints"));
        }

        public override async Task<AuthenticateResponse> ExecuteAsync(AuthenticateRequest request, CancellationToken cancellationToken = default(CancellationToken))
        {
            var response = new AuthenticateResponse(request.CorrelationId());

            var result = await _signInManager.PasswordSignInAsync(request.Username, request.Password, false, true);

            response.Result = result.Succeeded;
            response.IsLockedOut = result.IsLockedOut;
            response.IsNotAllowed = result.IsNotAllowed;
            response.RequiresTwoFactor = result.RequiresTwoFactor;
            response.Username = request.Username;

            if (result.Succeeded)
            {
                response.Token = await _tokenClaimsService.GetTokenAsync(request.Username);
            }

            return response;
        }
    }
}

#endif
