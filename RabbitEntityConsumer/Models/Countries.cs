using System;
using System.Collections.Generic;

namespace RabbitEntityConsumer.Models
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
