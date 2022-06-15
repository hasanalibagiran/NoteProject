using System.Net;
using Business.Services.Market;
using Microsoft.AspNetCore.Mvc;
using Note.Attributes;

namespace Note.Controllers
{
    public class MarketPlaceController:BaseApi
    {

        private readonly IMarketService _marketService;

        public MarketPlaceController(IMarketService marketService)
        {
            _marketService = marketService;
        }



        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetArchivesForSale()
        {
            var response = await _marketService.ArchivesForSale();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        
        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategoriesForSale()
        {
            var response = await _marketService.CategoriesForSale();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyArchive([FromBody] int archiveId)
        {
            var response = await _marketService.BuyArchive(archiveId);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> BuyCategory([FromBody] int categoryId)
        {
            var response = await _marketService.BuyCategory(categoryId);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> SellArchive([FromBody] int archiveId, int price)
        {
            var response = await _marketService.SellArchive(archiveId,price);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> SellCategory([FromBody] int categoryId,int price)
        {
            var response = await _marketService.SellCategory(categoryId,price);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> ShowMyItemsForSale()
        {
            var response = await _marketService.ShowMyItemsForSale();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }




    }
}