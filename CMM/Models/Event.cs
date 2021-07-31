using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CMM.Models
{
    public class Event
    {
        [Key]
        public int ConcertID { get; set; }

        [Required]
        [Url]
        public string ConcertPoster { get; set; }

        [Required]
        [StringLength(250)]
        public string ConcertMusician { get; set; }

        [Required]
        [Url]
        public string ConcertLink { get; set; }

        [Required]
        [StringLength(250)]
        public string ConcertName { get; set; }

        [Required]
        [StringLength(1000)]
        public string ConcertDescription { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime ConcertDateTime { get; set; }

        [Required]
        [Range(0, 999.99)]
        public decimal ConcertPrice { get; set; }

        [Required]
        public int TicketLimit { get; set; }

        [Required]
        public string ConcertStatus {get; set;}

        [Required]
        [DefaultValue(true)]
        public Boolean ConcertVisibility { get; set; }
    }
}
