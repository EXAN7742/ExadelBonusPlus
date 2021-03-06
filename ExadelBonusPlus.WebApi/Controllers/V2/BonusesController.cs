﻿using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ExadelBonusPlus.Services.Models;
using Swashbuckle.AspNetCore.Annotations;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ExadelBonusPlus.WebApi.v2
{
    [ApiController]
    [ApiVersion("2")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Authorize]
    public class BonusesController : ControllerBase
    {
        private readonly ILogger<BonusController> _logger;
        private readonly IBonusService _BonusService;
        
        public BonusesController(ILogger<BonusController> logger, IBonusService BonusService)
        {
            _logger = logger;
            _BonusService = BonusService;
        }

        [HttpPost]
        [Route("/api/v{version:apiVersion}/vendors/{vendorId:Guid}/[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus added ", Type = typeof(ResultDto<BonusDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<BonusDto>>> AddBonusAsync([FromBody] AddBonusDto Bonus)
        {
            var userId = User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value;
            return Ok(await _BonusService.AddBonusAsync(Bonus, Guid.Parse(userId)));
        }

        [HttpGet]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus with filters and sorting", Type = typeof(ResultDto<List<BonusDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<IEnumerable<BonusDto>>>> FindBonusesAsync([FromQuery] BonusFilter bonusFilter)
        {
            return Ok(await _BonusService.FindBonusesAsync(bonusFilter));
        }

        [HttpGet]
        [Route("/api/v{version:apiVersion}/vendors/{vendorId:Guid}/[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus by ID", Type = typeof(ResultDto<BonusDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<BonusDto>>> GetBonusByIdAsync([FromRoute] Guid id)
        {
            return Ok(await _BonusService.FindBonusByIdAsync(id));
        }

        [HttpPut]
        [Route("/api/v{version:apiVersion}/vendors/{vendorId:Guid}/[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus updated ", Type = typeof(ResultDto<BonusDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<BonusDto>>> UpdateBonusAsync([FromRoute][Required] Guid id, [FromBody][Required] UpdateBonusDto Bonus)
        {
            return Ok(await _BonusService.UpdateBonusAsync(id, Bonus));
        }

        [HttpDelete]
        [Route("/api/v{version:apiVersion}/vendors/{vendorId:Guid}/[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus deleted ", Type = typeof(ResultDto<BonusDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<BonusDto>>> DeleteBonusAsync([FromRoute] Guid id)
        {
           return Ok(await _BonusService.DeleteBonusAsync(id));
        }

        [HttpPatch]
        [Route("{id}/status/{isActive:bool}")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus activated", Type = typeof(ResultDto<BonusDto>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<BonusDto>>> ActivateBonusAsync([FromRoute] Guid id, bool isActive)
        {
            if (isActive)
            {
                return Ok(await _BonusService.ActivateBonusAsync(id));
            }
            else
            {
                return Ok(await _BonusService.DeactivateBonusAsync(id));
            }
            
        }

        [HttpGet]
        [Route("tags")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Tags", Type = typeof(ResultDto<List<string>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<List<String>>>> GetBonusTagsAync()
        {
            return Ok(await _BonusService.GetBonusTagsAsync());
        }

        [HttpGet]
        [Route("cities")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Cities", Type = typeof(ResultDto<List<string>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<List<String>>>> GetCitiesAync()
        {
            return Ok(await _BonusService.GetCitiesAsync());
        }

        [HttpGet]
        [Route("statistic")]
        [SwaggerResponse((int)HttpStatusCode.OK, Description = "Bonus statistic with filters and sorting", Type = typeof(ResultDto<List<BonusStatisticDto>>))]
        [SwaggerResponse((int)HttpStatusCode.InternalServerError)]
        public async Task<ActionResult<ResultDto<IEnumerable<BonusDto>>>> GetBonusStatisticAsync([FromQuery] BonusFilter bonusFilter)
        {
            return Ok(await _BonusService.GetBonusStatisticAsync(bonusFilter));
        }
    }
}