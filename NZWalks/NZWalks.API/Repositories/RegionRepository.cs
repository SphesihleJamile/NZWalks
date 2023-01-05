using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDbContext nZWalksDbContext;

        public RegionRepository(NZWalksDbContext nZWalksDbContext)
        {
            this.nZWalksDbContext = nZWalksDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = new Guid(); //Here we are overriding the Id so that it can be configured by te API
            await nZWalksDbContext.AddAsync(region); //We are then telling Entity Framework to add the region to the database
            await nZWalksDbContext.SaveChangesAsync(); //We are then telling EF to save all the changes to the database
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == id);
            if(region == null)
            {
                return null;
            }

            //Delete from database
            nZWalksDbContext.Regions.Remove(region);
            await nZWalksDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDbContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid Id)
        {
            return await nZWalksDbContext.Regions.FirstOrDefaultAsync(r => r.Id == Id);
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            //Find region by id from database through entity framework
            var existing_region = await nZWalksDbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            //if region is not found, return null
            if(existing_region == null)
            {
                return null;
            }

            //if region is found, update the region with the new data

            existing_region.Code = region.Code;
            existing_region.Name = region.Name;
            existing_region.Area = region.Area;
            existing_region.Lat = region.Lat;
            existing_region.Long = region.Long;
            existing_region.Population = region.Population;

            //Save all the changes to the database
            await nZWalksDbContext.SaveChangesAsync();
            
            //return region
            return existing_region;
        }
    }
}
