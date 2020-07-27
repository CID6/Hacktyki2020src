using System;
using System.Collections.Generic;

namespace RabbitEntityConsumer.Models
{
    public partial class CarFactories
    {
        public CarFactories()
        {
            CarProducts = new HashSet<CarProducts>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CityId { get; set; }
        public int ManufacturerId { get; set; }

        public virtual Cities City { get; set; }
        public virtual Manufacturers Manufacturer { get; set; }
        public virtual ICollection<CarProducts> CarProducts { get; set; }
    }
}
