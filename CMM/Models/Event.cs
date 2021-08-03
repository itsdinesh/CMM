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
        [Display(Name = "Concert ID")]
        public int ConcertID { get; set; }

        [Display(Name = "Concert Poster")]
        public string ConcertPoster { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Concert Musician")]
        public string ConcertMusician { get; set; }

        [Required]
        [Url]
        [Display(Name = "Concert Link")]
        public string ConcertLink { get; set; }

        [Required]
        [StringLength(250)]
        [Display(Name = "Concert Name")]
        public string ConcertName { get; set; }

        [Required]
        [StringLength(1000)]
        [Display(Name = "Concert Description")]
        public string ConcertDescription { get; set; }

        [Required]
        [Display(Name = "Concert Date")]
        public DateTime ConcertDateTime { get; set; }

        [Required]
        [Range(0, 999.99)]
        [Display(Name = "Concert Price")]
        public decimal ConcertPrice { get; set; }

        [Required]
        [Display(Name = "Ticket Limit")]
        public int TicketLimit { get; set; }

        [Required]
        [Display(Name = "Ticket Purchased")]
        public int TicketPurchased { get; set; }

        [Required]
        [Display(Name = "Concert Status")]
        public string ConcertStatus {get; set;}

        [Required]
        [Display(Name = "Visibility")]
        [DefaultValue(true)]
        public Boolean ConcertVisibility { get; set; }
    }
}
