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

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
namespace NZWalks.API.Controllers;

[ApiController]            
[Route("[Controller]")]
public class RegionsController : ControllerBase
{
    private readonly IRegionRepository regionRepository;
    public RegionsController(IRegionRepository regionRepository)
    {
        this.regionRepository = regionRepository;
    }
    [HttpGet]
    public IActionResult GetAllRegions()
    {
        var regions = regionRepository.GetAll();

        //return DTO regions

        var regionsDTO = new List<Models.DTO.Region>();
        regions.ToList().ForEach(region =>
        {
            var regionDTO = new Models.DTO.Region()
            {
                Id = region.Id,
                Code = region.Code,
                Name = region.Name,
                Area = region.Area,
                Lat = region.Lat,
                Long = region.Long,
                Population = region.Population,
            };

            regionsDTO.Add(regionDTO);
        });

        return Ok(regionsDTO);
    }
   
}
