using MP.SimpleTokens.Identity.Contracts;

namespace MP.SimpleTokens.Identity.Models
{
    public class IdentityInfo : PublicIdentity
    {
        public SocialIdentity? SocialIdentity { get; set; }
        public string? BlockchainAddress { get; set; }
    }
}
