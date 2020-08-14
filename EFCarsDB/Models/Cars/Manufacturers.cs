using System;
using System.Collections.Generic;

namespace EFCarsDB.Models
{
    public partial class Manufacturers
    {
        public Manufacturers()
        {
            CarFactories = new HashSet<CarFactories>();
            CarModels = new HashSet<CarModels>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<CarFactories> CarFactories { get; set; }
        public virtual ICollection<CarModels> CarModels { get; set; }
    }
}
