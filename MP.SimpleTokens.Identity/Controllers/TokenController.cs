using Microsoft.AspNetCore.Mvc;
using MP.DocumentDB.Interfaces;
using MP.SimpleTokens.Common.Ethereum.Interfaces;
using MP.SimpleTokens.Identity.Contracts;
using MP.SimpleTokens.Identity.Models;

namespace MP.SimpleTokens.Identity.Controllers
{
    [ApiController]
    [Route("token")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IEthereumService _ethereumService;

        public TokenController(ILogger<TokenController> logger, IEthereumService ethereumService)
        {
            _logger = logger;
            _ethereumService = ethereumService;
        }

        [HttpPost("public/blockchain/transactions/get")]
        [ProducesResponseType(typeof(IDictionary<string, PublicIdentity>), 200)]
        public async Task<IActionResult> GetPublicUsersForBlockchainTokenTransactions(
            [FromServices] IDocumentCollection<IdentityInfo> collection,
            IEnumerable<TokenTransaction> tokenTransactions
        )
        {
            var owners = new Dictionary<string, PublicIdentity>();

            async Task AddPublicIdentity(string blockChainAddress)
            {
                _logger.LogInformation($"Checking the mongo db for identity with blockchain address {blockChainAddress}");

                PublicIdentity storedOwner = await collection.GetFirstOrDefault(c => c.BlockchainAddress == blockChainAddress);

                if (storedOwner == null)
                {
                    _logger.LogInformation($"Identity not found with blockchain address {blockChainAddress}, returning an anonymous armadillo");
                    storedOwner = new PublicIdentity { Name = "Anonymous Armadillo" };
                }

                owners.Add(blockChainAddress, storedOwner);
            }

            foreach (var transfer in tokenTransactions)
            {
                if (
                    !string.IsNullOrWhiteSpace(transfer.To) &&
                    !owners.ContainsKey(transfer.To)
                )
                {
                    await AddPublicIdentity(transfer.To);
                }

                if (
                    !string.IsNullOrWhiteSpace(transfer.From) &&
                    !owners.ContainsKey(transfer.From)
                )
                {
                    await AddPublicIdentity(transfer.From);
                }
            }

            return Ok(owners);
        }
    }
}
