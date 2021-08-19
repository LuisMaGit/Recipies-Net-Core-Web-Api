using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Api.Controllers.Helpers;
using Api.Extensions;
using Api.Filters.Common;
using Api.Filters.RecipieFilters;
using Api.Models.RecipieModel;
using Api.ModelsDto.Requests.Recipie;
using Api.ModelsDto.Responses.Recipie;
using Api.Services.RecipieService;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RecipieController : ControllerBase
    {
        private readonly IRecipieService _recipiesService;
        private readonly IMapper _mapperService;

        public RecipieController(IRecipieService recipieService, IMapper mapperService)
        {
            _recipiesService = recipieService;
            _mapperService = mapperService;
        }

        [HttpGet("all")]
        public async Task<ActionResult<IEnumerable<RecipieReadDto>>> GetAll()
        {
            var userId =  HttpContext.GetUserIdByClaim();
            var recipies = await _recipiesService.GetRecipiesAsync(userId);
            var dto = _mapperService.Map<List<RecipieReadDto>>(recipies);
            return Ok(dto);
        }

        [HttpGet("{id:int}")]
        [EnsureRecipieExistsFilter, EnsureUserOwnsRecipieFilter]
        public async Task<ActionResult<RecipieReadDto>> Get(int id)
        {
            var recipie = await _recipiesService.GetRecipieByIdAsync(id);
            var dto = _mapperService.Map<RecipieReadDto>(recipie);
            return Ok(dto);
        }

        [HttpPost]
        [ValidateModelFilter]
        public async Task<ActionResult<RecipieReadDto>> Post(RecipieCreateUpdateDto dto)
        {
            //GET USE ID
            var userId = HttpContext.GetUserIdByClaim();
            //MAP TO RECIPIE
            var recipie = _mapperService.Map<Recipie>(dto);
            recipie.UserId = userId;
            //SEND TO SERVICE
            var response = await _recipiesService.CreateRecipieAsync(recipie);
            //GET RESPONSE
            var responseDto = _mapperService.Map<RecipieReadDto>(response);
            //SET LOCATION
            var location =
                GeneralControllerHelper.GetCreatedLocation(HttpContext, $"recipie/{responseDto.RecipieId.ToString()}");
            return Created(location, responseDto);
        }

        [HttpDelete]
        [Route("{id:int}")]
        [EnsureRecipieExistsFilter, EnsureUserOwnsRecipieFilter]
        public async Task<ActionResult> Delete(int id)
        {
            await _recipiesService.DeleteRecipeAsync(id);
            return Ok();
        }

        [HttpDelete]
        [Route("hardDelete/{id:int}")]
        [EnsureRecipieExistsFilter, EnsureUserOwnsRecipieFilter]
        public async Task<ActionResult> HardDelete(int id)
        {
            await _recipiesService.HardDeleteRecipeAsync(id);
            return Ok();
        }

        [HttpPut]
        [Route("{id:int}")]
        [EnsureRecipieExistsFilter, EnsureUserOwnsRecipieFilter]
        public async Task<ActionResult<RecipieReadDto>> Put(int id, RecipieCreateUpdateDto dto)
        {
            var recipie = _mapperService.Map<Recipie>(dto);
            var updated = await _recipiesService.UpdateRecipieAsync(id, recipie);
            var outputDto = _mapperService.Map<RecipieReadDto>(updated);
            return Ok(outputDto);
        }
    }
}