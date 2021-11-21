using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using MP.DocumentDB.Interfaces;
using MP.SimpleTokens.Common.Ethereum.Interfaces;
using MP.SimpleTokens.Common.Models.Tokens;
using MP.SimpleTokens.Identity.Models;
using Nethereum.ABI.FunctionEncoding.Attributes;
using Nethereum.Contracts;
using Nethereum.StandardNonFungibleTokenERC721;
using Nethereum.StandardNonFungibleTokenERC721.ContractDefinition;
using Nethereum.Web3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;

namespace MP.SimpleTokens.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ILogger<TokenController> _logger;
        private readonly IEthereumService _ethereumService;

        public TokenController(ILogger<TokenController> logger, IEthereumService ethereumService)
        {
            _logger = logger;
            _ethereumService = ethereumService;
        }

        [HttpPost]
        public async Task<IActionResult> GetUsersForToken([FromServices] IDocumentCollection<IdentityInfo> collection, TokenInfo token)
        {
            if (token.Type == TokenType.Ethereum)
            {
                if (token.BlockchainInfo == null)
                    return BadRequest();

                var owner = await _ethereumService.GetTokenOwnerAddress(token.BlockchainInfo);

                var storedOwner = await collection.GetFirstOrDefault(c => c.BlockchainAddress == owner);

                var tokenURI = await _ethereumService.GetTokenURI(token.BlockchainInfo);

                switch (token.BlockchainInfo.TokenVendor)
                {
                    case TokenVendor.Mintable:

                        break;
                    default:
                        break;
                }

                var transfers = await _ethereumService.GetTokenTransactionHistory(token.BlockchainInfo);

                return Ok(new { owner, tokenURI, storedOwner, transfers });
            }


            return Ok();
        }
    }




}
