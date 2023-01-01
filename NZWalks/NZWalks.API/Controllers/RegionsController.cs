using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RegionsController : Controller
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

            //Return DTO regions
            var regionsDTO = new List<Models.DTO.Region>();
            regions.ToList().ForEach(domainRegion =>
            {
                var regionDTO = new Models.DTO.Region()
                {
                    Id = domainRegion.Id,
                    Name = domainRegion.Name,
                    Code = domainRegion.Code,
                    Area = domainRegion.Area,
                    Lat = domainRegion.Lat,
                    Long = domainRegion.Long,
                    Population = domainRegion.Population
                };
                regionsDTO.Add(regionDTO);
            });

            return Ok(regionsDTO); //An OK response is a 200 HTTP response
        }
    }
}
