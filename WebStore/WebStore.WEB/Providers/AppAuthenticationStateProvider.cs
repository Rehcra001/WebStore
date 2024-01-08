using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace WebStore.WEB.Providers
{
    public class AppAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly ILocalStorageService _localStorageService;
        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private const string JWT_TOKEN = "bearerToken";

        public AppAuthenticationStateProvider(ILocalStorageService localStorageService)
        {
            _localStorageService = localStorageService;
        }

        
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            try
            {
                string savedToken = await _localStorageService.GetItemAsync<string>(JWT_TOKEN);

                if (String.IsNullOrWhiteSpace(savedToken))
                {
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);
                DateTime expiry = jwtSecurityToken.ValidTo;

                if (expiry < DateTime.UtcNow)
                {
                    await _localStorageService.RemoveItemAsync(JWT_TOKEN);
                    return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                }

                //We get claims from token and build authenticated user object
                var claims = ParseClaims(jwtSecurityToken);
                var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));
                return new AuthenticationState(user);

            }
            catch (Exception)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
        }

        private IList<Claim> ParseClaims(JwtSecurityToken jwtSecurityToken)
        {
            IList<Claim> claims = jwtSecurityToken.Claims.ToList();

            // The value of the tokenContent.Subject is the users email
            claims.Add(new Claim(ClaimTypes.Name, jwtSecurityToken.Subject));

            return claims;
        }

        internal async Task SignIn()
        {
            string savedToken = await _localStorageService.GetItemAsync<string>(JWT_TOKEN);

            JwtSecurityToken jwtSecurityToken = _jwtSecurityTokenHandler.ReadJwtToken(savedToken);

            var claims = ParseClaims(jwtSecurityToken);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims, "jwt"));

            Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(user));
            NotifyAuthenticationStateChanged(authenticationState);
        }

        internal void SignOut()
        {
            ClaimsPrincipal nobody = new ClaimsPrincipal(new ClaimsIdentity());

            Task<AuthenticationState> authenticationState = Task.FromResult(new AuthenticationState(nobody));
            NotifyAuthenticationStateChanged(authenticationState);
        }
    }
}
