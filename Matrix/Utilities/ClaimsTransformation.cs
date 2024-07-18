using Common.Constants;
using Common.Enums;
using Infrastructure.Interfaces.Utilities;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Matrix.Helper;
using System.Net;
using System.Security.Claims;
using System.Text.Json;

namespace Matrix.Utilities
{
    public class ClaimsTransformation : IClaimsTransformation
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenUtility _tokenService;
        public ClaimsTransformation(HttpClient httpClient,
                                    ITokenUtility tokenService,
                                    IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _tokenService = tokenService;
        }

        public async Task<ClaimsPrincipal> TransformAsync(ClaimsPrincipal principal)
        {
            try
            {
                var accessToken = _httpContextAccessor.HttpContext.Request.Headers[CommonNames.Authorization];
                string accessTokenText = accessToken.ToString();
                string accessTokenValue = accessTokenText.Split(' ')[1];

                int userId = Convert.ToInt32(principal.FindFirstValue(ClaimTypes.NameIdentifier));

                var identity = (ClaimsIdentity)principal.Identity;
                if (identity.IsAuthenticated)
                {
                    List<Claim> userClaims;
                    if (identity.HasClaim(i => i.Type == "sub"))
                    {
                        var idSrvClaim = identity.Claims.Single(c => c.Type == "sub");
                        identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, idSrvClaim.Value));
                        identity.RemoveClaim(idSrvClaim);
                    }
                }
                return principal;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private async Task<List<Claim>> GetUserClaims(string accessToken)
        {
            try
            {
                List<Claim> userClaims = null;
                if (_httpClient != null)
                {
                    userClaims = new List<Claim>();
                    _httpClient.DefaultRequestHeaders.Add("Authorization", accessToken);

                    var result = await _httpClient.GetAsync("");

                    if (result.IsSuccessStatusCode)
                    {
                        var resultingPayload = result.Content.ReadAsStringAsync().Result;
                        var deserializedResult = Newtonsoft.Json.JsonConvert.DeserializeObject<IDictionary<string, string>>(resultingPayload);

                        if (deserializedResult != null && deserializedResult.Count() > 0)
                        {
                            userClaims = deserializedResult.Select(i => new Claim(i.Key, i.Value)).ToList();
                        }
                    }
                }

                return userClaims;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
