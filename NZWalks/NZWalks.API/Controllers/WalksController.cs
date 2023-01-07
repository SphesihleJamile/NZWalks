using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WalksController : Controller
    {
        private readonly IWalkRepository walkRepository;
        private readonly IMapper mapper;

        public WalksController(IWalkRepository walkRepository, IMapper mapper)
        {
            this.walkRepository = walkRepository;
            this.mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllWalksAsync()
        {
            //Get data from database
            var walks = await walkRepository.GetAllAsync();
            //Convert domain to dto
            var walksDTO = mapper.Map<List<Models.DTO.Walk>>(walks);
            //return Ok status
            return Ok(walksDTO);
        }

        [HttpGet]
        [Route("{id:guid}")]
        [ActionName("GetWalkAsync")]
        public async Task<IActionResult> GetWalkAsync(Guid id)
        {
            //Get data from database
            var walks = await walkRepository.GetAsync(id);

            //if null return 404 status
            if (walks == null)
                return NotFound();

            //Convert domain model to DTO
            var walksDTO = mapper.Map<Models.DTO.Walk>(walks);

            //return dto
            return Ok(walksDTO);
        }

        [HttpPost]
        public async Task<IActionResult> AddWalkAsync([FromBody] Models.DTO.AddWalkRequest addWalkRequest)
        {
            //Convert DTO to Domain model
            var walk = new Models.Domain.Walk()
            {
                Name = addWalkRequest.Name,
                Length = addWalkRequest.Length,
                RegionId = addWalkRequest.RegionId,
                WalkDifficultyId = addWalkRequest.WalkDifficultyId
            };

            //Pass domain object to repository to persist this
            walk = await walkRepository.AddAsync(walk);

            //Convert the domain model back to a DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            //return CreatedAtAction status
            return CreatedAtAction(nameof(GetWalkAsync), new { id = walkDTO.Id }, walkDTO);
        }

        [HttpPut]
        [Route("{id:guid}")]
        public async Task<IActionResult> UpdateWalkAsync([FromRoute] Guid id, 
            [FromBody] Models.DTO.UpdateWalkRequest updateWalkRequest)
        {
            //Convert DTO to domain object
            var walk = new Models.Domain.Walk()
            {
                Name = updateWalkRequest.Name,
                Length = updateWalkRequest.Length,
                RegionId = updateWalkRequest.RegionId,
                WalkDifficultyId = updateWalkRequest.WalkDifficultyId
            };  

            //pass details to repository - get domain object in respone (or null)
            walk = await walkRepository.UpdateAsync(id, walk);

            //Handle Null (not found)
            if (walk == null)
                return NotFound();

            //Convert back domain to dto
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            //return response
            return Ok(walkDTO);
        }

        [HttpDelete]
        [Route("{id:guid}")]
        public async Task<IActionResult> DeleteWalkAsync([FromRoute] Guid id)
        {
            //Get walk from database : it will be deleted from the database and an object will be returned
            var walk = await walkRepository.DeleteAsync(id);

            //if null, return 404 : the id does not match any walk object in the database
            if (walk == null)
                return NotFound();

            //convert response to DTO
            var walkDTO = mapper.Map<Models.DTO.Walk>(walk);

            //return Ok status
            return Ok(walkDTO);
        }
    }
}
