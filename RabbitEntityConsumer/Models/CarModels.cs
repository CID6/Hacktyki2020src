using System;
using System.Collections.Generic;

namespace RabbitEntityConsumer.Models
{
    public partial class CarModels
    {
        public CarModels()
        {
            CarFeatureCarModel = new HashSet<CarFeatureCarModel>();
            CarProducts = new HashSet<CarProducts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int ManufacturerId { get; set; }

        public virtual Manufacturers Manufacturer { get; set; }
        public virtual ICollection<CarFeatureCarModel> CarFeatureCarModel { get; set; }
        public virtual ICollection<CarProducts> CarProducts { get; set; }
    }
}
