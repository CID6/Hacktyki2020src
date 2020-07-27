using System;
using System.Collections.Generic;

namespace RabbitEntityConsumer.Models
{
    public partial class CarProducts
    {
        public CarProducts()
        {
            CarProductCarFeature = new HashSet<CarProductCarFeature>();
        }

        public int Id { get; set; }
        public short Year { get; set; }
        public string Vin { get; set; }
        public int CarModelId { get; set; }
        public int FactoryId { get; set; }

        public virtual CarModels CarModel { get; set; }
        public virtual CarFactories Factory { get; set; }
        public virtual ICollection<CarProductCarFeature> CarProductCarFeature { get; set; }
    }
}
