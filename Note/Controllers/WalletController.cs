using System.Net;
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Note.Attributes;

namespace Note.Controllers
{
    public class WalletController:BaseApi
    {
        private readonly IWalletService _walletService;

        public WalletController(IWalletService walletService)
        {
            _walletService = walletService;
        }
        
        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetBalance()
        {
            var response = await _walletService.Balance();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> DepositMoney([FromBody] int amount)
        {
            var response = await _walletService.DepositMoney(amount);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> WithdrawMoney([FromBody] int amount)
        {
            var response = await _walletService.WithdrawMoney(amount);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        


    }
}