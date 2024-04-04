﻿using MediPortaApi.Entities;
using MediPortaApi.Helpers;
using MediPortaApi.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace MediPortaApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TagController : ControllerBase
    {
        private readonly ILogger<TagController> _logger;
        private readonly ITagService _tagService;

        public TagController(ILogger<TagController> logger, ITagService tagService) 
        {
            _logger = logger;
            _tagService = tagService;
        }

        [HttpGet("RefreshTags")]
        public async Task<ActionResult> RefreshTags()
        {
            await _tagService.RefreshTagsAsync();

            return Ok();
        }

        [HttpGet("GetTags")]
        public async Task<ActionResult<List<Tag>>> GetTags(string sortBy, bool sortDesc)
        {
            if(sortBy != SortOptions.Name && sortBy != SortOptions.Percentage)
            {
                return BadRequest("Please provide valid sorting options : 'name' or 'percentage'");
            }

            var result = await _tagService.GetTagsAsync(sortBy, sortDesc);

            return Ok(result);
        }
    }
}
