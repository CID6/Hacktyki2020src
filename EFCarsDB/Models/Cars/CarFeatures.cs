using System;
using System.Collections.Generic;

namespace EFCarsDB.Models
{
    public partial class CarFeatures
    {
        public CarFeatures()
        {
            CarFeatureCarModel = new HashSet<CarFeatureCarModel>();
            CarProductCarFeature = new HashSet<CarProductCarFeature>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<CarFeatureCarModel> CarFeatureCarModel { get; set; }
        public virtual ICollection<CarProductCarFeature> CarProductCarFeature { get; set; }
    }
}
