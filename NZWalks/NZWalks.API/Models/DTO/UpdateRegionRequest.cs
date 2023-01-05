namespace NZWalks.API.Models.DTO
{
    public class UpdateRegionRequest
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public double Area { get; set; }
        public double Lat { get; set; }
        public double Long { get; set; }
        public long Population { get; set; }

        //if for example you do not want to update the Area, or the Code of the region, then you may exclude it from this 
        //      list and it wont be updated
    }
}
