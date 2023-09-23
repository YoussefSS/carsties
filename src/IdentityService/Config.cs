using Duende.IdentityServer.Models;

namespace IdentityService;

public static class Config
{
    public static IEnumerable<IdentityResource> IdentityResources =>
        new IdentityResource[]
        {
            new IdentityResources.OpenId(),
            new IdentityResources.Profile(),
        };

    public static IEnumerable<ApiScope> ApiScopes =>
        new ApiScope[]
        {
            new ApiScope("auctionApp", "Auction app full access"),
        };


    public static IEnumerable<Client> Clients =>
        new Client[]
        {
            // This is how we give tokens. We use openId which gives us 2 tokens, one is the ID token and the other is the access token
            // The ID token contains information about the user
            // The access token is the key that allows our client to request resources from our resource server (auction service)
            // Note that this config is just for development, so don't worry about the lack of security.
            new Client
            {
                ClientId = "postman",
                ClientName = "Postman",
                AllowedScopes = {"openid", "profile", "auctionApp"}, // these are the resources defined above. We want this returned to the client in a token
                RedirectUris = {"https://www.getpostman.com/oauth2/callback"}, // this won't do anything in postman
                ClientSecrets = new[] {new Secret("NotASecret".Sha256())}, // the client secret we'll use in postman
                AllowedGrantTypes = {GrantType.ResourceOwnerPassword} // authentication flow
            }
        };
}
