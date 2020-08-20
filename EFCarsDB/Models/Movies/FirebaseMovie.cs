using System;
using System.ComponentModel.DataAnnotations;

namespace EFCarsDB.Models
{
    public class FirebaseMovie
    {
        public string FirebaseID { get; set; }
        public string Title { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        public DateTime ReleaseDate { get; set; }
        public string Genre { get; set; }

        public string Price { get; set; }
    }
}
