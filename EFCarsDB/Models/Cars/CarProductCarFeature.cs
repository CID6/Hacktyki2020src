namespace EFCarsDB.Models
{
    public partial class CarProductCarFeature
    {
        public int CarProductId { get; set; }
        public int InstalledFeatureId { get; set; }

        public virtual CarProducts CarProduct { get; set; }
        public virtual CarFeatures InstalledFeature { get; set; }
    }
}
