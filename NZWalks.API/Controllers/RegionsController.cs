using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;
using NZWalks.API.Models.DTO;
using AutoMapper;
 
// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace NZWalks.API.Controllers;

[ApiController]
[Route("[Controller]")]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository regionRepository;
    private readonly IMapper mapper;

    public IMapper Mapper { get; }

    public RegionsController(IRegionRepository regionRepository, IMapper mapper)
    {
        this.regionRepository = regionRepository;
        this.mapper = mapper;
        Mapper = mapper;
    }


    [HttpGet]
    public async Task<IActionResult> GetAllRegionsAsync()
    {
        var regions = await regionRepository.GetAllAsync();

        //return DTO regions

        //var regionsDTO = new List<Models.DTO.Region>();
        //regions.ToList().ForEach(region =>
        //{
        //    var regionDTO = new Models.DTO.Region()
        //    {
        //        Id = region.Id,
        //        Code = region.Code,
        //        Name = region.Name,
        //        Area = region.Area,
        //        Lat = region.Lat,
        //        Long = region.Long,
        //        Population = region.Population,
        //    };

        //    regionsDTO.Add(regionDTO);
        //});
        var regionsDTO = mapper.Map<List<Models.DTO.Region>>(regions);

        return Ok(regionsDTO);
    }

    [HttpGet]
    [Route("{id:guid}")]
    [ActionName("GetRegionAsync")]
    public async Task<IActionResult> GetRegionAsync(Guid id)
    {
        var region = await regionRepository.GetAsync(id);
        if (region == null )
        {
            return NotFound();
        }

        var regionDTO = mapper.Map<Models.DTO.Region>(region);

        return Ok(regionDTO);
    }

    [HttpPost]
    public async Task<IActionResult> AddRegionAsync(Models.DTO.AddRegionRequest addRegionRequest)
    {
        //Request(DTO) to Domain Model
        var region = new Models.Domain.Region()
        {
            Code = addRegionRequest.Code,
            Name = addRegionRequest.Name,
            Area = addRegionRequest.Area,
            Lat = addRegionRequest.Lat,
            Long = addRegionRequest.Long,
            Population = addRegionRequest.Population 
        };

        //Pass Details to repo

        await regionRepository.AddAsync(region);

        //Convert to DTO
        var regionDTO = new Models.DTO.Region()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            Area = region.Area,
            Lat = region.Lat,
            Long = region.Long,
            Population = region.Population
        };

        return CreatedAtAction(nameof(GetRegionAsync),new {id = regionDTO.Id}, regionDTO );

    }

    [HttpDelete]
    [Route("{id:guid}")]
    public async Task<IActionResult> DeleteRegionAsync(Guid id)
    {
        //get region from database

        var region = await regionRepository.DeleteAsync(id);

        //if null notFound
        if(region == null)
        {
            return NotFound();
        }
        //Convert to DTO

        var regionDTO = new Models.DTO.Region
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            Area = region.Area,
            Lat = region.Lat,
            Long = region.Long,
            Population = region.Population
        };
        //return OK Response
        return Ok(region);
    }

    [HttpPut]
    [Route("{id:guid}")]
   
    public async Task<IActionResult> UpdateAsync([FromRoute]Guid id,[FromBody]Models.DTO.UpdateRegionRequest updateRegionRequest)
    {

        //Convert DTO to domain model
        var region = new Models.Domain.Region()
        {
            Code = updateRegionRequest.Code,
            Name = updateRegionRequest.Name,
            Area = updateRegionRequest.Area,
            Lat = updateRegionRequest.Lat,
            Long = updateRegionRequest.Long,
            Population = updateRegionRequest.Population
        };
        //update region using repo

        region = await regionRepository.UpdateAsync(id, region);
        // if its null then not found
        if (region == null)
        {
            return NotFound();
        }
        //convert domain back to dto
        var regionDTO = new Models.DTO.Region()
        {
            Id = region.Id,
            Code = region.Code,
            Name = region.Name,
            Area = region.Area,
            Lat = region.Lat,
            Long = region.Long,
            Population = region.Population
        };
        //return ok response
        return Ok(regionDTO);

    }
}
