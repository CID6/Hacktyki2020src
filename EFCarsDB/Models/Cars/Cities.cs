using System;
using System.Collections.Generic;

namespace EFCarsDB.Models
{
    public partial class Cities
    {
        public Cities()
        {
            CarFactories = new HashSet<CarFactories>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public int CountryId { get; set; }

        public virtual Countries Country { get; set; }
        public virtual ICollection<CarFactories> CarFactories { get; set; }
    }
}
