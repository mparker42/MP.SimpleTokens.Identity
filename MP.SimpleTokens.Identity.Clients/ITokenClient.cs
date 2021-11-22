using MP.SimpleTokens.Identity.Contracts;
using Refit;

namespace MP.SimpleTokens.Identity.Clients
{
    public interface ITokenClient
    {
        [Post("/token/public/blockchain/transactions/get")]
        Task<IDictionary<string, PublicIdentity>> GetPublicUsersForBlockchainTokenTransactions(IEnumerable<TokenTransaction> tokenTransactions);
    }
}