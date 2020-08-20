using System.Collections.Generic;

namespace EFCarsDB.Models
{
    public partial class Countries
    {
        public Countries()
        {
            Cities = new HashSet<Cities>();
            Manufacturers = new HashSet<Manufacturers>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }

        public virtual ICollection<Cities> Cities { get; set; }
        public virtual ICollection<Manufacturers> Manufacturers { get; set; }
    }
}
