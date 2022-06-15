using System.Net;
using Business.Services;
using Business.Services;
using Dto.Models;
using Microsoft.AspNetCore.Mvc;
using Note.Attributes;

namespace Note.Controllers
{
    public class CategoryController:BaseApi
    {
        
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _categoryService.Get(id);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetCategoryList()
        {
            var response = await _categoryService.GetAll();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody]CategoryModel category)
        {
            var response = await _categoryService.Add(category);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody]CategoryModel category)
        {
            var response = await _categoryService.Update(category);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpDelete("[action]")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _categoryService.Delete(id);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }




    }
}