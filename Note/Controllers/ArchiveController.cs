using System.Net;
using Business.Services;
using Business.Services;
using Dto.Models;
using Entities;
using Microsoft.AspNetCore.Mvc;
using Note.Attributes;

namespace Note.Controllers
{
    public class ArchiveController:BaseApi
    {
        
        private readonly IArchiveService _archiveService;

        public ArchiveController(IArchiveService archiveService)
        {
            _archiveService = archiveService;
        }

        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> Get(int id)
        {
            var response = await _archiveService.Get(id);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetArchiveList()
        {
            var response = await _archiveService.GetAll();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPost("[action]")]
        public async Task<IActionResult> Add([FromBody]ArchiveModel archive)
        {
            var response = await _archiveService.Add(archive);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpPut("[action]")]  
        public async Task<IActionResult> Update([FromBody]ArchiveModel archive)
        {
            var response = await _archiveService.Update(archive);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpDelete("[action]/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var response = await _archiveService.Delete(id);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAllArchivesAndCategories()
        {
            var response = await _archiveService.GetAllArchivesAndCategories();
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }
        [Authorization(RoleID = 0)]
        [HttpGet("[action]")]
        public async Task<IActionResult> GetArchiveListByCategory(int id)
        {
            var response = await _archiveService.GetArchiveListByCategory(id);
            if (response.StatusCode == (int)HttpStatusCode.OK)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

    }
}